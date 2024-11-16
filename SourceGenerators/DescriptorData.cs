using System.Text;

namespace StardewUI.SourceGenerators;

internal readonly record struct ObjectDescriptorWithInheritanceData
{
    public readonly ObjectDescriptorData Self;
    public readonly EquatableArray<ObjectDescriptorData> Ancestors;

    public ObjectDescriptorWithInheritanceData(
        ObjectDescriptorData self,
        EquatableArray<ObjectDescriptorData> ancestors
    )
    {
        Self = self;
        Ancestors = ancestors;
    }

    public readonly (string declaringTypeName, string propertyName) GetDefaultOutlet()
    {
        var propertyName = Self.GetDefaultOutletPropertyName();
        if (!string.IsNullOrEmpty(propertyName))
        {
            return (Self.BaseName, propertyName!);
        }
        foreach (var ancestor in Ancestors)
        {
            propertyName = ancestor.GetDefaultOutletPropertyName();
            if (!string.IsNullOrEmpty(propertyName))
            {
                return (ancestor.BaseName, propertyName!);
            }
        }
        return ("", "");
    }

    public readonly IEnumerable<(string outletName, string declaringTypeName, string propertyName)> GetNamedOutlets()
    {
        foreach (var namedOutlet in Self.GetNamedOutlets())
        {
            yield return namedOutlet;
        }
        foreach (var ancestor in Ancestors)
        {
            foreach (var namedOutlet in ancestor.GetNamedOutlets())
            {
                yield return namedOutlet;
            }
        }
    }
}

internal readonly record struct ObjectDescriptorData
{
    public string DefinitionName => BaseName + TypeParameterList;
    public string DescriptorDefinitionName => $"{BaseName}Descriptor{TypeParameterList}";
    public string DescriptorName => $"{BaseName}Descriptor{TypeArgumentList}";
    public string ExpandedName => BaseName + TypeArgumentList;
    public string FullDefinition => $"{NamespaceName}.{BaseName}{TypeParameterList}";
    public string FullName => $"{NamespaceName}.{BaseName}{TypeArgumentList}";
    public string TypeArgumentList => FormatTypeArguments(true);
    public string TypeParameterList => FormatTypeArguments(false);

    public readonly string BaseName;
    public readonly string FileName;
    public readonly string NamespaceName;
    public readonly EquatableArray<TypeArgumentData> TypeArguments;
    public readonly bool IsView;
    public readonly EquatableArray<EventDescriptorData> Events;
    public readonly EquatableArray<FieldDescriptorData> Fields;
    public readonly EquatableArray<MethodDescriptorData> Methods;
    public readonly EquatableArray<PropertyDescriptorData> Properties;

    public ObjectDescriptorData(
        string name,
        string namespaceName,
        EquatableArray<TypeArgumentData> typeArguments,
        bool isView,
        EquatableArray<EventDescriptorData> events,
        EquatableArray<FieldDescriptorData> fields,
        EquatableArray<PropertyDescriptorData> properties,
        EquatableArray<MethodDescriptorData> methods
    )
    {
        BaseName = name;
        NamespaceName = namespaceName;
        TypeArguments = typeArguments;
        FileName = TypeArguments.Count > 0 ? $"{BaseName}`{TypeArguments.Count}" : BaseName;
        IsView = isView;
        Events = events;
        Fields = fields;
        Properties = properties;
        Methods = methods;
    }

    public readonly string? GetDefaultOutletPropertyName()
    {
        foreach (var property in Properties)
        {
            if (property.IsOutlet && string.IsNullOrEmpty(property.OutletName))
            {
                return property.Name;
            }
        }
        return null;
    }

    public readonly IEnumerable<(string outletName, string declaringTypeName, string propertyName)> GetNamedOutlets()
    {
        foreach (var property in Properties)
        {
            if (property.IsOutlet && !string.IsNullOrEmpty(property.OutletName))
            {
                yield return (property.OutletName!, BaseName, property.Name);
            }
        }
    }

    private readonly string FormatTypeArguments(bool resolvedNames = false)
    {
        if (TypeArguments.Count == 0)
        {
            return "";
        }
        var sb = new StringBuilder();
        sb.Append('<');
        for (int i = 0; i < TypeArguments.Count; i++)
        {
            if (i > 0)
            {
                sb.Append(", ");
            }
            var argument = TypeArguments[i];
            sb.Append(resolvedNames ? argument.ArgumentTypeName : argument.ParameterName);
        }
        sb.Append('>');
        return sb.ToString();
    }
}

internal readonly record struct TypeArgumentData
{
    public readonly string ParameterName;
    public readonly string ArgumentTypeName;
    public readonly EquatableArray<string> ConstraintNames;

    public TypeArgumentData(string parameterName, string argumentTypeName, EquatableArray<string> constraintNames)
    {
        ParameterName = parameterName;
        ArgumentTypeName = argumentTypeName;
        ConstraintNames = constraintNames;
    }
}

internal readonly record struct EventDescriptorData
{
    public readonly string Name;
    public readonly string HandlerTypeName;
    public readonly string ArgsTypeName;
    public readonly int DelegateParameterCount;

    public EventDescriptorData(string name, string handlerTypeName, string argsTypeName, int delegateParameterCount)
    {
        Name = name;
        HandlerTypeName = handlerTypeName;
        ArgsTypeName = argsTypeName;
        DelegateParameterCount = delegateParameterCount;
    }
}

internal readonly record struct FieldDescriptorData
{
    public readonly string Name;
    public readonly string TypeName;

    public FieldDescriptorData(string name, string typeName)
    {
        Name = name;
        TypeName = typeName;
    }
}

internal readonly record struct MethodDescriptorData
{
    public readonly string Name;
    public readonly string ReturnTypeName;
    public readonly EquatableArray<MethodParameterData> Parameters;

    public MethodDescriptorData(string name, string returnTypeName, EquatableArray<MethodParameterData> parameters)
    {
        Name = name;
        ReturnTypeName = returnTypeName;
        Parameters = parameters;
    }
}

internal readonly record struct MethodParameterData
{
    public readonly string Name;
    public readonly string TypeName;
    public readonly bool IsOptional;
    public readonly object? DefaultValue;

    public MethodParameterData(string name, string typeName, bool isOptional, object? defaultValue)
    {
        Name = name;
        TypeName = typeName;
        IsOptional = isOptional;
        DefaultValue = defaultValue;
    }
}

internal readonly record struct PropertyDescriptorData
{
    public readonly string Name;
    public readonly string TypeName;
    public readonly bool HasGetter;
    public readonly bool HasSetter;
    public readonly bool IsAutoImplemented;
    public readonly bool IsOutlet;
    public readonly string? OutletName;

    public PropertyDescriptorData(
        string name,
        string typeName,
        bool hasGetter,
        bool hasSetter,
        bool isAutoImplemented,
        bool isOutlet,
        string? outletName
    )
    {
        Name = name;
        TypeName = typeName;
        HasGetter = hasGetter;
        HasSetter = hasSetter;
        IsAutoImplemented = isAutoImplemented;
        IsOutlet = isOutlet;
        OutletName = outletName;
    }
}
