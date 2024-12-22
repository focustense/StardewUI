using System.Collections.Immutable;
using System.Text;

namespace StardewUI.SourceGenerators;

internal static class DescriptorCodeGenerator
{
    public static string GenerateDescriptorFactory(ImmutableArray<ObjectDescriptorWithInheritanceData> descriptors)
    {
        var sb = new StringBuilder();
        sb.AppendLine("namespace StardewUI.Framework.Descriptors");
        sb.AppendLine("{");
        sb.AppendLine(1, "public static partial class DescriptorFactory");
        sb.AppendLine(1, "{");
        sb.AppendLine(2, "internal static readonly Dictionary<Type, IObjectDescriptor> PrecompiledDescriptors = new()");
        sb.AppendLine(2, "{");
        foreach (var descriptor in descriptors)
        {
            sb.AppendLine(
                3,
                $"{{typeof({descriptor.Self.FullName}), "
                    + $"new {descriptor.Self.NamespaceName}.{descriptor.Self.DescriptorName}()}},"
            );
        }
        sb.AppendLine(2, "};");
        sb.AppendLine(1, "}");
        sb.AppendLine("}");
        return sb.ToString();
    }

    public static string GenerateMembersOnlyDescriptor(ObjectDescriptorData data)
    {
        var sb = new StringBuilder();
        sb.AppendLine("using System;");
        sb.AppendLine("using StardewUI.Framework.Descriptors;");
        sb.AppendLine();
        sb.AppendLine($"namespace {data.NamespaceName}");
        sb.AppendLine("{");
        sb.AppendLine(1, $"internal static class {data.DescriptorDefinitionName}");
        WriteConstraints(sb, data);
        sb.AppendLine(1, "{");
        WriteMemberDescriptors(sb, data);
        sb.AppendLine(1, "}");
        sb.AppendLine("}");
        return sb.ToString();
    }

    public static string GenerateObjectDescriptor(ObjectDescriptorWithInheritanceData data)
    {
        var sb = new StringBuilder();
        sb.AppendLine("using System;");
        sb.AppendLine("using System.Diagnostics.CodeAnalysis;");
        sb.AppendLine("using StardewUI.Framework.Descriptors;");
        sb.AppendLine();
        sb.AppendLine($"namespace {data.Self.NamespaceName}");
        sb.AppendLine("{");
        var descriptorInterface = data.Self.IsView ? "IViewDescriptor" : "IObjectDescriptor";
        sb.AppendLine(1, $"internal class {data.Self.DescriptorDefinitionName} : {descriptorInterface}");
        WriteConstraints(sb, data.Self);
        sb.AppendLine(1, "{");
        sb.AppendLine(2, $"public Type TargetType => typeof({data.Self.FullName});");
        sb.AppendLine();
        sb.AppendLine(2, "public bool SupportsChangeNotifications => true;");
        sb.AppendLine();
        sb.AppendLine(2, "public bool TryGetEvent(string name, [MaybeNullWhen(false)] out IEventDescriptor evt)");
        sb.AppendLine(2, "{");
        sb.AppendLine(3, "evt = name switch");
        sb.AppendLine(3, "{");
        foreach (var evt in data.Self.Events)
        {
            sb.AppendLine(4, $"\"{evt.Name}\" => Events.{evt.Name},");
        }
        foreach (var ancestor in data.Ancestors)
        {
            foreach (var evt in ancestor.Events)
            {
                sb.AppendLine(4, $"\"{evt.Name}\" => {ancestor.DescriptorName}.Events.{evt.Name},");
            }
        }
        sb.AppendLine(4, "_ => null");
        sb.AppendLine(3, "};");
        sb.AppendLine(3, "return evt is not null;");
        sb.AppendLine(2, "}"); // TryGetEvent
        sb.AppendLine();
        sb.AppendLine(
            2,
            "public bool TryGetProperty(string name, [MaybeNullWhen(false)] out IPropertyDescriptor property)"
        );
        sb.AppendLine(2, "{");
        sb.AppendLine(3, "property = name switch");
        sb.AppendLine(3, "{");
        sb.AppendLine(4, $"\"this\" => ThisPropertyDescriptor<{data.Self.ExpandedName}>.Instance,");
        foreach (var property in data.Self.Properties)
        {
            sb.AppendLine(4, $"\"{property.Name}\" => Properties.{property.Name},");
        }
        foreach (var ancestor in data.Ancestors)
        {
            foreach (var property in ancestor.Properties)
            {
                sb.AppendLine(4, $"\"{property.Name}\" => {ancestor.DescriptorName}.Properties.{property.Name},");
            }
        }
        sb.AppendLine(4, "_ => null");
        sb.AppendLine(3, "};");
        sb.AppendLine(3, "return property is not null;");
        sb.AppendLine(2, "}"); // TryGetProperty
        sb.AppendLine();
        sb.AppendLine(2, "public bool TryGetMethod(string name, [MaybeNullWhen(false)] out IMethodDescriptor method)");
        sb.AppendLine(2, "{");
        sb.AppendLine(3, "method = name switch");
        sb.AppendLine(3, "{");
        foreach (var method in data.Self.Methods)
        {
            sb.AppendLine(4, $"\"{method.Name}\" => Methods.{method.Name},");
        }
        foreach (var ancestor in data.Ancestors)
        {
            foreach (var method in ancestor.Methods)
            {
                sb.AppendLine(4, $"\"{method.Name}\" => {ancestor.DescriptorName}.Methods.{method.Name},");
            }
        }
        sb.AppendLine(4, "_ => null");
        sb.AppendLine(3, "};");
        sb.AppendLine(3, "return method is not null;");
        sb.AppendLine(2, "}"); // TryGetMethod
        sb.AppendLine();
        if (data.Self.IsView)
        {
            var (defaultOutletDeclaringTypeName, defaultOutletPropertyName) = data.GetDefaultOutlet();
            string defaultOutletAccessor = "null";
            if (!string.IsNullOrEmpty(defaultOutletPropertyName))
            {
                var prefix =
                    defaultOutletDeclaringTypeName != data.Self.BaseName
                        ? $"{defaultOutletDeclaringTypeName}Descriptor."
                        : "";
                defaultOutletAccessor = $"{prefix}Properties.{defaultOutletPropertyName}";
            }
            sb.AppendLine(
                2,
                "public bool TryGetChildrenProperty(string outletName, [MaybeNullWhen(false)] out IPropertyDescriptor property)"
            );
            sb.AppendLine(2, "{");
            sb.AppendLine(3, "property = !string.IsNullOrEmpty(outletName)");
            sb.AppendLine(4, "? outletName.ToLowerInvariant() switch");
            sb.AppendLine(4, "{");
            foreach (var (outletName, declaringTypeName, propertyName) in data.GetNamedOutlets())
            {
                var prefix = declaringTypeName != data.Self.BaseName ? $"{declaringTypeName}Descriptor." : "";
                sb.AppendLine(5, $"\"{outletName.ToLowerInvariant()}\" => {prefix}Properties.{propertyName},");
            }

            sb.AppendLine(5, "_ => null");
            sb.AppendLine(4, "}");
            sb.AppendLine(4, $": {defaultOutletAccessor};");
            sb.AppendLine(3, "return property is not null;");
            sb.AppendLine(2, "}"); // TryGetMethod
            sb.AppendLine();
        }
        WriteMemberDescriptors(sb, data.Self);
        sb.AppendLine(1, "}");
        sb.AppendLine("}");
        return sb.ToString();
    }

    private static void WriteConstraints(StringBuilder sb, ObjectDescriptorData data)
    {
        bool wroteWhere = false;
        foreach (var typeArg in data.TypeArguments)
        {
            if (typeArg.ConstraintNames.Count == 0)
            {
                continue;
            }
            if (!wroteWhere)
            {
                sb.AppendLine(1, "where");
                wroteWhere = true;
            }
            var constraintList = string.Join(", ", typeArg.ConstraintNames);
            sb.AppendLine(2, $"{typeArg.ParameterName} : {constraintList}");
        }
    }

    private static void WriteMemberDescriptors(StringBuilder sb, ObjectDescriptorData data)
    {
        sb.AppendLine(2, "public static class Events");
        sb.AppendLine(2, "{");
        foreach (var evt in data.Events)
        {
            sb.AppendLine(3, $"public static readonly IEventDescriptor {evt.Name} =");
            sb.AppendLine(4, $"new PrecompiledEventDescriptor<{data.FullDefinition}, {evt.HandlerTypeName}>");
            sb.AppendLine(4, "(");
            sb.AppendLine(5, $"\"{evt.Name}\",");
            sb.AppendLine(5, $"(target, handler) => target.{evt.Name} += handler,");
            sb.AppendLine(5, $"(target, handler) => target.{evt.Name} -= handler,");
            sb.AppendLine(5, $"typeof({evt.ArgsTypeName})");
            sb.AppendLine(4, ");");
        }
        sb.AppendLine(2, "}");
        sb.AppendLine();

        sb.AppendLine(2, "public static class Properties");
        sb.AppendLine(2, "{");
        foreach (var field in data.Fields)
        {
            sb.AppendLine(3, $"public static readonly IPropertyDescriptor<{field.TypeName}> {field.Name} =");
            sb.AppendLine(4, $"new PrecompiledPropertyDescriptor<{data.FullDefinition}, {field.TypeName}>");
            sb.AppendLine(4, "(");
            sb.AppendLine(5, $"\"{field.Name}\",");
            sb.AppendLine(5, "true,");
            sb.AppendLine(5, "false,");
            sb.AppendLine(5, $"target => target.{field.Name},");
            sb.AppendLine(5, $"(target, value) => target.{field.Name} = value");
            sb.AppendLine(4, ");");
        }
        foreach (var property in data.Properties)
        {
            sb.AppendLine(3, $"public static readonly IPropertyDescriptor<{property.TypeName}> {property.Name} =");
            sb.AppendLine(4, $"new PrecompiledPropertyDescriptor<{data.FullDefinition}, {property.TypeName}>");
            sb.AppendLine(4, "(");
            sb.AppendLine(5, $"\"{property.Name}\",");
            sb.AppendLine(5, "false,");
            sb.AppendLine(5, (property.IsAutoImplemented ? "true" : "false") + ",");
            sb.AppendLine(5, property.HasGetter ? $"target => target.{property.Name}," : "null,");
            sb.AppendLine(5, property.HasSetter ? $"(target, value) => target.{property.Name} = value" : "null");
            sb.AppendLine(4, ");");
        }
        sb.AppendLine(2, "}");
        sb.AppendLine();

        sb.AppendLine(2, "public static class Methods");
        sb.AppendLine(2, "{");
        foreach (var method in data.Methods)
        {
            bool isVoid = method.ReturnTypeName == "void";
            string returnTypeName = isVoid ? "object" : method.ReturnTypeName;
            sb.AppendLine(3, $"public static readonly IMethodDescriptor<{returnTypeName}> {method.Name} =");
            sb.AppendLine(4, $"new PrecompiledMethodDescriptor<{data.FullName}, {returnTypeName}>");
            sb.AppendLine(4, "(");
            sb.AppendLine(5, $"\"{method.Name}\",");
            sb.AppendLine(5, "[");
            foreach (var param in method.Parameters)
            {
                sb.AppendLine(6, $"typeof({param.TypeName}),");
            }
            sb.AppendLine(5, "],");
            sb.AppendLine(5, "[");
            foreach (var param in method.Parameters)
            {
                if (param.IsOptional)
                {
                    string defaultValueExpression = param.DefaultValue switch
                    {
                        bool b => b ? "true" : "false",
                        string s => "\"" + s + "\"",
                        _ => param.DefaultValue?.ToString() ?? "null",
                    };
                    sb.AppendLine(6, defaultValueExpression + ",");
                }
            }
            sb.AppendLine(5, "],");
            sb.AppendLine(5, "(target, arguments) =>");
            sb.AppendLine(5, "{");
            sb.AppendIndent(6);
            if (!isVoid)
            {
                sb.Append("return ");
            }
            sb.AppendLine($"target.{method.Name}");
            sb.AppendLine(6, "(");
            int paramIndex = 0;
            foreach (var param in method.Parameters)
            {
                string trail = paramIndex < method.Parameters.Count - 1 ? "," : "";
                sb.AppendLine(7, $"({param.TypeName})arguments[{paramIndex++}]{trail}");
            }
            sb.AppendLine(6, ");");
            if (isVoid)
            {
                sb.AppendLine(6, "return null;");
            }
            sb.AppendLine(5, "}");
            sb.AppendLine(4, ");");
        }
        sb.AppendLine(2, "}");
    }
}

file static class StringBuilderExtensions
{
    private static readonly string indent = "    ";

    public static void AppendIndent(this StringBuilder sb, int level)
    {
        for (int i = 0; i < level; i++)
        {
            sb.Append(indent);
        }
    }

    public static void AppendLine(this StringBuilder sb, int indentLevel, string text)
    {
        sb.AppendIndent(indentLevel);
        sb.AppendLine(text);
    }
}
