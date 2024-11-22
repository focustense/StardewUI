using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace StardewUI.SourceGenerators;

[Generator]
public class DescriptorGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx =>
            ctx.AddSource(
                "GenerateDescriptorAttribute.g.cs",
                SourceText.From(AttributeCodeGenerator.GenerateDescriptorSource, Encoding.UTF8)
            )
        );

        var viewDescriptorsProvider = context
            .SyntaxProvider.ForAttributeWithMetadataName(
                "StardewUI.GenerateDescriptorAttribute",
                predicate: static (s, _) => true,
                transform: static (ctx, _) => GetObjectDescriptorData(ctx.SemanticModel, ctx.TargetNode)
            )
            .Where(static data => data is not null)
            .Select((data, _) => data!.Value)
            .Collect();
        context.RegisterImplementationSourceOutput(
            viewDescriptorsProvider,
            (spc, viewDescriptors) =>
            {
                var visitedTypes = new HashSet<(string, string)>();
                foreach (var viewDescriptor in viewDescriptors)
                {
                    string code = DescriptorCodeGenerator.GenerateObjectDescriptor(viewDescriptor);
                    spc.AddSource($"{viewDescriptor.Self.FileName}.g.cs", SourceText.From(code, Encoding.UTF8));
                    visitedTypes.Add((viewDescriptor.Self.NamespaceName, viewDescriptor.Self.FileName));
                }
                // Doing ancestors in a second loop makes things a little cleaner with visited types; we don't have to
                // worry about duplication in the prior loop.
                foreach (var viewDescriptor in viewDescriptors)
                {
                    foreach (var ancestor in viewDescriptor.Ancestors)
                    {
                        if (visitedTypes.Contains((ancestor.NamespaceName, ancestor.FileName)))
                        {
                            continue;
                        }
                        string code = DescriptorCodeGenerator.GenerateMembersOnlyDescriptor(ancestor);
                        spc.AddSource($"{ancestor.FileName}.g.cs", SourceText.From(code, Encoding.UTF8));
                        visitedTypes.Add((ancestor.NamespaceName, ancestor.FileName));
                    }
                }
                string factoryCode = DescriptorCodeGenerator.GenerateDescriptorFactory(viewDescriptors);
                spc.AddSource($"DescriptorFactory.g.cs", SourceText.From(factoryCode, Encoding.UTF8));
            }
        );
    }

    private static AttributeData? FindAttribute(ISymbol symbol, string namespaceName, string name)
    {
        foreach (var attr in symbol.GetAttributes())
        {
            if (
                attr.AttributeClass?.ContainingNamespace.ToString() == namespaceName
                && attr.AttributeClass.Name == name
            )
            {
                return attr;
            }
        }
        return null;
    }

    private static ObjectDescriptorWithInheritanceData? GetObjectDescriptorData(
        SemanticModel semanticModel,
        SyntaxNode typeSyntax
    )
    {
        if (semanticModel.GetDeclaredSymbol(typeSyntax) is not INamedTypeSymbol viewSymbol)
        {
            return null;
        }
        ObjectDescriptorData? self = null;
        var parents = ImmutableArray.CreateBuilder<ObjectDescriptorData>();
        for (var type = viewSymbol; type is not null; type = type.BaseType)
        {
            if (type.ContainingNamespace.ToString() == "System" && type.Name == "Object")
            {
                break;
            }
            bool isSelf = SymbolEqualityComparer.Default.Equals(type, viewSymbol);
            var events = ImmutableArray.CreateBuilder<EventDescriptorData>();
            var fields = ImmutableArray.CreateBuilder<FieldDescriptorData>();
            var properties = ImmutableArray.CreateBuilder<PropertyDescriptorData>();
            var methods = ImmutableArray.CreateBuilder<MethodDescriptorData>();
            var members = type.GetMembers();
            foreach (var member in members)
            {
                if (member.DeclaredAccessibility != Accessibility.Public || member.IsStatic || member.IsOverride)
                {
                    continue;
                }
                if (member is IFieldSymbol field)
                {
                    fields.Add(new(field.Name, field.Type.ToDisplayString(NullableFlowState.NotNull)));
                }
                else if (member is IPropertySymbol property && IsPropertySupported(property))
                {
                    properties.Add(
                        new(
                            property.Name,
                            property.Type.ToDisplayString(NullableFlowState.NotNull),
                            property.GetMethod?.DeclaredAccessibility == Accessibility.Public,
                            property.SetMethod?.DeclaredAccessibility == Accessibility.Public,
                            IsAutoProperty(property),
                            IsOutletType(property.Type),
                            GetOutletName(property)
                        )
                    );
                }
                else if (member is IMethodSymbol method && IsMethodSupported(method))
                {
                    var parameters = ImmutableArray.CreateBuilder<MethodParameterData>();
                    foreach (var param in method.Parameters)
                    {
                        object? defaultValue = param.HasExplicitDefaultValue ? param.ExplicitDefaultValue : null;
                        parameters.Add(
                            new(
                                param.Name,
                                param.Type.ToDisplayString(NullableFlowState.NotNull),
                                param.IsOptional,
                                defaultValue
                            )
                        );
                    }
                    methods.Add(
                        new(
                            method.Name,
                            method.ReturnType.ToDisplayString(NullableFlowState.NotNull),
                            parameters.ToEquatable()
                        )
                    );
                }
                else if (member is IEventSymbol evt && IsEventSupported(evt))
                {
                    var argsType = GetEventArgsType(evt);
                    if (argsType is null)
                    {
                        continue;
                    }
                    var handlerTypeName = evt.Type.ToDisplayString(NullableFlowState.NotNull);
                    events.Add(new(evt.Name, handlerTypeName, argsType.ToDisplayString(NullableFlowState.NotNull), 2));
                }
            }
            var data = new ObjectDescriptorData(
                type.Name,
                type.ContainingNamespace.ToString(),
                GetTypeArguments(type),
                IsViewImplementation(type),
                events.ToEquatable(),
                fields.ToEquatable(),
                properties.ToEquatable(),
                methods.ToEquatable()
            );
            if (isSelf)
            {
                self = data;
            }
            else
            {
                parents.Add(data);
            }
        }
        return self is not null ? new(self.Value, parents.ToEquatable()) : null;
    }

    private static ITypeSymbol? GetEventArgsType(IEventSymbol evt)
    {
        foreach (var member in evt.Type.GetMembers())
        {
            if (member is IMethodSymbol method && method.Name == "Invoke")
            {
                return method.Parameters[1].Type;
            }
        }
        return null;
    }

    private static string? GetOutletName(IPropertySymbol property)
    {
        var outletAttribute = FindAttribute(property, "StardewUI.Widgets", "OutletAttribute");
        if (outletAttribute is null)
        {
            return null;
        }
        if (outletAttribute.ConstructorArguments.Length > 0)
        {
            return outletAttribute.ConstructorArguments[0].Value?.ToString();
        }
        foreach (var namedArg in outletAttribute.NamedArguments)
        {
            if (namedArg.Key == "name")
            {
                return namedArg.Value.ToString();
            }
        }
        return null;
    }

    private static EquatableArray<TypeArgumentData> GetTypeArguments(INamedTypeSymbol type)
    {
        if (!type.IsGenericType)
        {
            return EquatableArray<TypeArgumentData>.Empty;
        }
        var builder = ImmutableArray.CreateBuilder<TypeArgumentData>(type.TypeArguments.Length);
        for (int i = 0; i < type.TypeArguments.Length; i++)
        {
            var argType = type.TypeArguments[i].ToDisplayString(NullableFlowState.NotNull);
            var param = type.TypeParameters[i];
            var paramName = param.ToDisplayString(NullableFlowState.NotNull);
            var constraintNames = ImmutableArray.CreateBuilder<string>();
            if (param.HasValueTypeConstraint)
            {
                constraintNames.Add("struct");
            }
            else if (param.HasReferenceTypeConstraint)
            {
                constraintNames.Add("class");
            }
            else if (param.HasNotNullConstraint)
            {
                constraintNames.Add("notnull");
            }
            else if (param.HasUnmanagedTypeConstraint)
            {
                constraintNames.Add("unmanaged");
            }
            if (param.ConstraintTypes.Length > 0)
            {
                foreach (var constraintType in param.ConstraintTypes)
                {
                    constraintNames.Add(constraintType.ToDisplayString(NullableFlowState.NotNull));
                }
            }
            if (param.HasConstructorConstraint)
            {
                constraintNames.Add("new()");
            }
            builder.Add(new(paramName, argType, constraintNames.ToEquatable()));
        }
        return builder.ToEquatable();
    }

    private static bool IsAutoProperty(IPropertySymbol property)
    {
        return property
            .ContainingType.GetMembers()
            .Any(m =>
                m is IFieldSymbol field && SymbolEqualityComparer.Default.Equals(field.AssociatedSymbol, property)
            );
    }

    private static bool IsEventSupported(IEventSymbol evt)
    {
        return evt.AddMethod is not null && evt.RemoveMethod is not null;
    }

    private static bool IsMethodSupported(IMethodSymbol method)
    {
        if (method.IsGenericMethod || method.MethodKind != MethodKind.Ordinary)
        {
            return false;
        }
        foreach (var param in method.Parameters)
        {
            if (param.RefKind != RefKind.None)
            {
                return false;
            }
        }
        return true;
    }

    private static bool IsOutletType(ITypeSymbol type)
    {
        if (IsView(type))
        {
            return true;
        }
        foreach (var intf in type.AllInterfaces)
        {
            if (
                intf.ContainingNamespace.ToString() != "System.Collections.Generic"
                || intf.Name != "IEnumerable"
                || !intf.IsGenericType
            )
            {
                continue;
            }
            if (IsView(intf.TypeArguments[0]))
            {
                return true;
            }
        }
        return false;
    }

    private static bool IsPropertySupported(IPropertySymbol property)
    {
        return !property.IsIndexer;
    }

    private static bool IsView(ISymbol symbol)
    {
        return symbol.ContainingNamespace.ToString() == "StardewUI" && symbol.Name == "IView";
    }

    private static bool IsViewImplementation(ITypeSymbol type)
    {
        foreach (var intf in type.AllInterfaces)
        {
            if (IsView(intf))
            {
                return true;
            }
        }
        return false;
    }
}
