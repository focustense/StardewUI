﻿using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;
using LoxSmoke.DocXml;
using StardewUI;

const BindingFlags visibleBindingFlags =
    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

string outputDirectory = Path.GetFullPath("../../../../../docs/reference", Assembly.GetExecutingAssembly().Location);

var reader = new FixedUnindentXmlReader();
var uiAssembly = typeof(UI).Assembly;
var parallelOptions = new ParallelOptions();
var documentableTypes = uiAssembly
    .GetTypes()
    .Where(t => t.IsVisibleForDocumentation() && t.Namespace?.StartsWith(typeof(UI).Namespace!) == true)
    .ToArray();
await Parallel.ForEachAsync(
    documentableTypes,
    parallelOptions,
    async (typeToDocument, _) => await WriteTypeFile(typeToDocument)
);
(string kind, string title)[] namespaceSections =
[
    ("Class", "Classes"),
    ("Struct", "Structs"),
    ("Interface", "Interfaces"),
    ("Enum", "Enums"),
    ("Delegate", "Delegates"),
];
var typesByNamespace = documentableTypes.Where(t => !string.IsNullOrEmpty(t.Namespace)).ToLookup(t => t.Namespace!);
await Parallel.ForEachAsync(
    typesByNamespace,
    parallelOptions,
    async (nsGroup, _) => await WriteNamespaceToFile(nsGroup.Key, nsGroup)
);

void AppendMemberRemarks(StringBuilder sb, MemberInfo member)
{
    var remarks = GetMemberComment(
        member,
        (CommonComments c) => !string.IsNullOrWhiteSpace(c.Remarks) ? c.Remarks : null
    );
    if (string.IsNullOrEmpty(remarks))
    {
        return;
    }
    sb.AppendLine("##### Remarks");
    sb.AppendLine();
    sb.AppendLine(ReplaceTags(remarks, e => FormatParagraphText(e, member.ReflectedType!.Namespace!)).Trim());
    sb.AppendLine();
}

static void AppendParameterList(
    StringBuilder sb,
    IReadOnlyList<ParameterInfo> parameters,
    bool fullPath = false,
    bool escaped = true,
    bool collapsePrimitives = false,
    bool includeModifiers = false,
    bool includeNames = false
)
{
    var argumentTypes = parameters.Select(p => p.ParameterType).ToArray();
    AppendTypeList(
        sb,
        argumentTypes,
        fullPath,
        escaped: escaped,
        collapsePrimitives: collapsePrimitives,
        prefixSelector: (_, i) => GetPrefix(i),
        suffixSelector: (_, i) => includeNames ? parameters[i].Name : null
    );

    string? GetPrefix(int index)
    {
        if (!includeModifiers)
        {
            return null;
        }
        var param = parameters[index];
        if (param.IsOut)
        {
            return param.IsIn ? "ref" : "out";
        }
        else if (param.IsIn)
        {
            return "in";
        }
        return null;
    }
}

static void AppendTypeList(
    StringBuilder sb,
    IReadOnlyList<Type> types,
    bool fullPath = false,
    bool escaped = true,
    bool collapsePrimitives = false,
    Func<Type, int, string?>? prefixSelector = null,
    Func<Type, int, string?>? suffixSelector = null
)
{
    for (int i = 0; i < types.Count; i++)
    {
        if (i > 0)
        {
            sb.Append(", ");
        }
        if (prefixSelector?.Invoke(types[i], i) is string prefix && prefix.Length > 0)
        {
            sb.Append(prefix);
            sb.Append(' ');
        }
        sb.Append(FormatGenericTypeName(types[i], fullPath, escaped, collapsePrimitives: collapsePrimitives));
        if (suffixSelector?.Invoke(types[i], i) is string suffix && suffix.Length > 0)
        {
            sb.Append(' ');
            sb.Append(suffix);
        }
    }
}

static void AppendWithInheritance(
    StringBuilder sb,
    string text,
    Type? declaringType,
    MemberInfo? originalMember,
    Type referringType
)
{
    sb.Append(text);
    if (declaringType is not null && referringType != declaringType)
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            sb.Append("<br>");
        }
        sb.Append(@"<span class=""muted"" markdown>(Inherited from ");
        sb.Append(FormatTypeLink(declaringType, referringType.Namespace!));
        sb.Append(")</span>");
    }
    else if (originalMember?.DeclaringTypeOrDefinition() is Type originalType && referringType != originalType)
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            sb.Append("<br>");
        }
        sb.Append(@"<span class=""muted"" markdown>(Overrides ");
        sb.Append(FormatTypeLink(originalType, referringType.Namespace ?? ""));
        sb.Append('.');
        sb.Append(FormatMemberLink(originalMember, referringType.Namespace ?? ""));
        sb.Append(")</span>");
    }
}

static void AppendWithTypeArguments(
    StringBuilder sb,
    string name,
    IReadOnlyList<Type> types,
    bool fullPath = false,
    bool escaped = true
)
{
    var nameLength = name.LastIndexOf('`');
    if (nameLength < 0)
    {
        nameLength = name.Length;
    }
    sb.Append(name[0..nameLength]);
    sb.Append(escaped ? "&lt;" : "<");
    AppendTypeList(sb, types, fullPath: fullPath, escaped: escaped);
    sb.Append(escaped ? "&gt;" : ">");
}

static void AppendWrapped(StringBuilder sb, string s, ref int currentLength, int maxLength, int indentLength)
{
    int newLength = currentLength + s.Length;
    if (newLength > maxLength)
    {
        sb.AppendLine();
        sb.Append(new string(' ', indentLength));
        currentLength = 0;
    }
    sb.Append(s);
    currentLength += s.Length;
}

static string EscapeYaml(string value)
{
    const string specialCharacters = "{}[]|:";
    value = value.Replace("<", "&lt;").Replace(">", "&gt;");
    if (value.IndexOfAny(specialCharacters.ToCharArray()) >= 0)
    {
        value = '"' + value + '"';
    }
    return value;
}

static string FormatConstructorName(
    ConstructorInfo ctor,
    bool fullPath = false,
    bool escaped = true,
    bool collapsePrimitives = false,
    bool includeParameterNames = false
)
{
    var sb = new StringBuilder();
    if (ctor.ReflectedType!.IsGenericType)
    {
        var args = ctor.ReflectedType!.GetGenericArguments();
        AppendWithTypeArguments(sb, ctor.ReflectedType!.Name, args, fullPath, escaped);
    }
    else
    {
        sb.Append(ctor.ReflectedType!.Name);
    }
    sb.Append('(');
    AppendParameterList(
        sb,
        ctor.GetParameters(),
        fullPath,
        escaped: escaped,
        collapsePrimitives: collapsePrimitives,
        includeNames: includeParameterNames
    );
    sb.Append(')');
    return sb.ToString();
}

static string FormatExternalLink(string namespaceOrType, string? memberName, string linkName)
{
    string? url = GetExternalUrl(namespaceOrType, memberName);
    return !string.IsNullOrEmpty(url) ? FormatLink(linkName, url) : linkName;
}

static string FormatFieldVisibility(FieldInfo field)
{
    return FormatMemberVisibility(
        field,
        f => f.IsPublic,
        f => f.IsFamilyAndAssembly,
        f => f.IsFamilyOrAssembly,
        f => f.IsAssembly,
        f => f.IsFamily
    );
}

static string FormatGenericMethodName(
    MethodBase method,
    bool fullPath = false,
    bool escaped = true,
    bool collapsePrimitives = false,
    bool includeParameters = true,
    bool includeParameterModifiers = false,
    bool includeParameterNames = false
)
{
    if (method.IsGenericMethod && !method.IsGenericMethodDefinition && method is MethodInfo methodInfo)
    {
        return FormatGenericMethodName(
            methodInfo.GetGenericMethodDefinition(),
            fullPath,
            escaped,
            collapsePrimitives,
            includeParameters,
            includeParameterModifiers,
            includeParameterNames
        );
    }
    var sb = new StringBuilder();
    if (method.IsGenericMethodDefinition)
    {
        var args = method.GetGenericArguments();
        AppendWithTypeArguments(sb, method.Name, args, fullPath, escaped);
    }
    else
    {
        sb.Append(method.Name);
    }
    if (includeParameters)
    {
        sb.Append('(');
        AppendParameterList(
            sb,
            method.GetParameters(),
            fullPath,
            escaped,
            collapsePrimitives,
            includeParameterModifiers,
            includeParameterNames
        );
        sb.Append(')');
    }
    return sb.ToString();
}

static string FormatGenericTypeName(
    Type type,
    bool fullPath = false,
    bool escaped = true,
    bool baseNameOnly = false,
    bool collapsePrimitives = false,
    bool includeOuterClasses = false
)
{
    if (collapsePrimitives && type.IsPrimitive || type == typeof(string))
    {
        return GetPrimitiveTypeName(type);
    }
    if (type.IsArray)
    {
        return FormatGenericTypeName(
            type.GetElementType()!,
            fullPath,
            escaped,
            baseNameOnly,
            collapsePrimitives,
            includeOuterClasses
        );
    }
    if (type.IsGenericParameter)
    {
        return type.Name;
    }
    var outerTypePrefix =
        ((fullPath || includeOuterClasses) && type.DeclaringType is Type outerType)
            ? FormatGenericTypeName(outerType, fullPath, escaped, baseNameOnly, collapsePrimitives, includeOuterClasses)
                + '.'
            : "";
    if (type.IsGenericType || type.ContainsGenericParameters)
    {
        if (!type.ContainsGenericParameters && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            var valueType = type.GetGenericArguments()[0];
            return outerTypePrefix
                + FormatGenericTypeName(
                    valueType,
                    fullPath,
                    escaped,
                    baseNameOnly,
                    collapsePrimitives,
                    includeOuterClasses
                )
                + '?';
        }
        var args = type.GetGenericArguments();
        var baseName =
            (type.IsGenericType && type.ContainsGenericParameters && !type.IsGenericTypeDefinition)
                ? type.GetGenericTypeDefinition().FullName!
                : type.FullName ?? type.Name;
        // TODO: Relying on explicit "ordering" of closed generics and nested types is probably unreliable; shaving off
        // the closed generic portion first might cut off something important if the closed generic is INSIDE (after)
        // the nested type, rather than outside.
        // //
        // What should probably be done here instead is to stop using Type.FullName at all, and instead just combine
        // the Type.Name with Type.Namespace; together, these two will exclude all the strange artifacts like closed
        // generic params and assembly names and just give us the specific things we want, and there is already logic
        // for adding back generic params and outer types afterward.
        var genericArgsStartIndex = baseName.IndexOf('[');
        if (genericArgsStartIndex >= 0)
        {
            baseName = baseName[..genericArgsStartIndex];
        }
        var nestedTypeStartIndex = baseName.LastIndexOf('+');
        if (nestedTypeStartIndex >= 0)
        {
            baseName = baseName[(nestedTypeStartIndex + 1)..];
        }
        if (!fullPath && baseName.StartsWith(type.Namespace!))
        {
            baseName = baseName[(type.Namespace!.Length + 1)..];
        }
        else if (type.Namespace == "System" && baseName.StartsWith("System."))
        {
            baseName = baseName[7..];
        }
        if (baseNameOnly)
        {
            int genericStartPos = baseName.IndexOf('`');
            return genericStartPos >= 0 ? baseName[0..genericStartPos] : baseName;
        }
        var sb = new StringBuilder();
        sb.Append(outerTypePrefix);
        AppendWithTypeArguments(sb, baseName, args, fullPath, escaped);
        return sb.ToString();
    }
    var result = fullPath ? type.FullName ?? type.Name : outerTypePrefix + type.Name;
    result = result.Replace('+', '.');
    if (result.EndsWith('&'))
    {
        result = result[..^1];
    }
    return result;
}

static string FormatLink(string name, string url)
{
    return $"[{name}]({url})";
}

static string FormatMemberLink(MemberInfo memberInfo, string referringNamespace)
{
    bool canLink = memberInfo.IsVisibleForDocumentation();
    string rootNamespace = typeof(UI).Namespace!;
    string linkName = FormatMemberName(memberInfo, canLink);
    if (!canLink)
    {
        return QuoteCode(linkName);
    }
    var declaringType = memberInfo.DeclaringTypeOrDefinition()!;
    if (!declaringType.Namespace!.StartsWith(rootNamespace))
    {
        return FormatExternalLink(declaringType.FullName!, memberInfo.Name, linkName);
    }
    string typeFilePath = MakeRelativeUrl(GetTypeFilePath(declaringType, true), referringNamespace);
    string anchor = memberInfo is MethodInfo anchorMethod ? GetMethodAnchor(anchorMethod) : memberInfo.Name;
    anchor = SluggifyName(anchor);
    return FormatLink(linkName, $"{typeFilePath}#{anchor}");
}

static string FormatMemberName(MemberInfo memberInfo, bool escaped = false, bool includeMethodParameters = true)
{
    return memberInfo switch
    {
        MethodInfo method => FormatGenericMethodName(
            method,
            escaped: escaped,
            includeParameters: includeMethodParameters
        ),
        PropertyInfo property => FormatPropertyName(property, escaped: escaped),
        _ => memberInfo.Name,
    };
}

static string FormatMemberVisibility<T>(
    T member,
    Func<T, bool> isPublic,
    Func<T, bool> isFamilyAndAssembly,
    Func<T, bool> isFamilyOrAssembly,
    Func<T, bool> isAssembly,
    Func<T, bool> isFamily
)
    where T : MemberInfo
{
    if (isPublic(member))
    {
        return "public";
    }
    if (isFamilyAndAssembly(member))
    {
        return "private protected";
    }
    if (isFamilyOrAssembly(member))
    {
        return "protected internal";
    }
    if (isAssembly(member))
    {
        return "internal";
    }
    if (isFamily(member))
    {
        return "protected";
    }
    return "private";
}

static string FormatMethodVisibility(MethodBase method)
{
    return FormatMemberVisibility(
        method,
        m => m.IsPublic,
        m => m.IsFamilyAndAssembly,
        m => m.IsFamilyOrAssembly,
        m => m.IsAssembly,
        m => m.IsFamily
    );
}

static string FormatNamespaceLink(string ns, string referringNamespace)
{
    string rootNamespace = typeof(UI).Namespace!;
    if (!ns.StartsWith(rootNamespace))
    {
        return FormatExternalLink(ns, null, ns);
    }
    string path = MakeRelativeUrl(GetNamespaceFilePath(ns), referringNamespace);
    return FormatLink(ns, path);
}

static string FormatParagraphText(XElement element, string referringNamespace, ListNumberer? listNumberer = null)
{
    return element.Name.LocalName switch
    {
        "see" or "seealso" => element.Attribute("href")?.Value is string href
            ? FormatLink(!string.IsNullOrEmpty(element.Value) ? element.Value : href, href)
            : ResolveCref(
                element.Attribute("cref")?.Value ?? "",
                ns => FormatNamespaceLink(ns, referringNamespace),
                type => FormatTypeLink(type, referringNamespace),
                member => FormatMemberLink(member, referringNamespace),
                cref => $"`{cref}`"
            ),
        "paramref" or "typeparamref" => QuoteCode(element.Attribute("name")?.Value),
        "c" or "code" => QuoteCode(element.Value),
        "para" => ReplaceInnerElements() + Environment.NewLine + Environment.NewLine,
        "b" or "strong" => "**" + ReplaceInnerElements() + "**",
        "i" or "em" => "_" + ReplaceInnerElements() + "_",
        "list" => Environment.NewLine
            + ReplaceElements(
                element,
                e =>
                    FormatParagraphText(
                        e,
                        referringNamespace,
                        e.Attribute("type")?.Value == "number" ? new ListNumberer() : null
                    )
            )
            + Environment.NewLine
            + Environment.NewLine,
        "item" => Environment.NewLine
            + "  "
            + (listNumberer is not null ? listNumberer.ItemNumber++ : "-")
            + ' '
            + ReplaceElements(element, e => FormatParagraphText(e, referringNamespace)),
        _ => "",
    };

    string ReplaceInnerElements() =>
        ReplaceElements(element, e => FormatParagraphText(e, referringNamespace, listNumberer));
}

static string FormatPropertyName(
    PropertyInfo property,
    bool fullPath = false,
    bool escaped = true,
    bool collapsePrimitives = false
)
{
    var parameters = property.GetIndexParameters();
    if (parameters.Length == 0)
    {
        return property.Name;
    }
    var sb = new StringBuilder();
    sb.Append(property.Name);
    sb.Append('[');
    for (int i = 0; i < parameters.Length; i++)
    {
        if (i > 0)
        {
            sb.Append(", ");
        }
        sb.Append(FormatGenericTypeName(parameters[i].ParameterType, fullPath, escaped, true, collapsePrimitives));
    }
    sb.Append(']');
    return sb.ToString();
}

static string FormatSimpleText(XElement element)
{
    return element.Name.LocalName switch
    {
        "see" or "seealso" => element.Attribute("href")?.Value is string href
            ? !string.IsNullOrEmpty(element.Value)
                ? element.Value
                : href
            : ResolveCref(
                element.Attribute("cref")?.Value ?? "",
                ns => ns,
                type => FormatGenericTypeName(type, includeOuterClasses: true),
                member =>
                    FormatGenericTypeName(member.DeclaringType!, includeOuterClasses: true)
                    + '.'
                    + FormatMemberName(member, escaped: true, includeMethodParameters: false),
                null
            ),
        "paramref" or "typeparamref" => element.Attribute("name")?.Value ?? "",
        "c" or "code" => element.Value,
        _ => "",
    };
}

static string FormatTypeLink(
    Type type,
    string referringNamespace,
    string nameSuffix = "",
    bool includeOuterClasses = false
)
{
    string rootNamespace = typeof(UI).Namespace!;
    if (type.IsArray)
    {
        return FormatTypeLink(type.GetElementType()!, referringNamespace, "[]");
    }
    else if (!type.IsVisibleForDocumentation())
    {
        return QuoteCode(FormatGenericTypeName(type, fullPath: true, escaped: false));
    }

    var outerClassPrefix =
        includeOuterClasses && type.DeclaringType is not null
            ? FormatTypeLink(type.DeclaringType, referringNamespace, nameSuffix, includeOuterClasses) + '.'
            : "";
    if (!type.IsGenericType || type.ContainsGenericParameters)
    {
        string linkName = FormatGenericTypeName(type);
        return outerClassPrefix + FormatWithLinkName(type, linkName);
    }

    var sb = new StringBuilder();
    sb.Append(outerClassPrefix);
    var defType = type.GetGenericTypeDefinition();
    var defTypeName = FormatGenericTypeName(defType, baseNameOnly: true);
    var defLink = FormatWithLinkName(defType, defTypeName);
    sb.Append(defLink);
    sb.Append('<');
    var argTypes = type.GetGenericArguments();
    for (int i = 0; i < argTypes.Length; i++)
    {
        if (i > 0)
        {
            sb.Append(", ");
        }
        sb.Append(FormatTypeLink(argTypes[i], referringNamespace));
    }
    sb.Append('>');
    return sb.ToString();

    string FormatWithLinkName(Type type, string linkName)
    {
        if (type.IsGenericParameter)
        {
            return QuoteCode(type.Name);
        }
        if (!type.Namespace!.StartsWith(rootNamespace))
        {
            return FormatExternalLink(type.RevertGenerics().FullName!, null, linkName);
        }
        string typeFilePath = MakeRelativeUrl(GetTypeFilePath(type, true), referringNamespace);
        return FormatLink(linkName, typeFilePath);
    }
}

static string FormatTypeKind(Type type, bool detectRecords = false)
{
    if (type.IsInterface)
    {
        return "Interface";
    }
    if (type.IsEnum)
    {
        return "Enum";
    }
    if (type.IsValueType)
    {
        return "Struct";
    }
    if (type.IsAssignableTo(typeof(Delegate)))
    {
        return "Delegate";
    }
    // Simple record check that isn't exhaustive, i.e. won't work for struct records, but we don't really care, as it's
    // only for documentation.
    return (detectRecords && type.GetMethods().Any(m => m.Name == "<Clone>$")) ? "Record" : "Class";
}

static string FormatTypeVisibility(Type type)
{
    return FormatMemberVisibility(
        type,
        t => t.IsPublic || t.IsNestedPublic,
        t => t.IsNestedFamANDAssem,
        t => t.IsNestedFamORAssem,
        t => t.IsNestedAssembly,
        t => t.IsNestedFamily
    );
}

static IEnumerable<MemberInfo> GetBaseMembers(MemberInfo member)
{
    var type = member.ReflectedType ?? member.DeclaringType!;
    var baseTypeMembers =
        GetBaseTypeOrDefinition(type)
            ?.GetMember(member.Name, visibleBindingFlags)
            .Where(m => m.ToString() == member.ToString()) ?? [];
    foreach (var m in baseTypeMembers)
    {
        yield return m;
    }
    foreach (var interfaceType in type.GetInterfaces())
    {
        var interfaceMember = member switch
        {
            EventInfo evt => interfaceType.GetEvent(member.Name, visibleBindingFlags),
            FieldInfo field => interfaceType.GetField(member.Name, visibleBindingFlags),
            MethodInfo method => GetInterfaceMethodDefinition(interfaceType),
            PropertyInfo property => interfaceType.GetProperty(member.Name, visibleBindingFlags),
            _ => member,
        };
        if (interfaceMember is not null)
        {
            yield return interfaceMember;
        }
    }

    MemberInfo? GetInterfaceMethodDefinition(Type interfaceType)
    {
        var interfaceMap = type.GetInterfaceMap(interfaceType);
        var methodIndex = Array.IndexOf(interfaceMap.TargetMethods, member);
        if (methodIndex >= 0)
        {
            var interfaceMethod = interfaceMap.InterfaceMethods[methodIndex];
            return interfaceType.RevertGenerics().GetMemberWithSameMetadataDefinitionAs(interfaceMethod);
        }
        return null;
    }
}

static Type? GetBaseTypeOrDefinition(Type type)
{
    return type.BaseType?.RevertGenerics();
}

static IEnumerable<Type> GetBaseTypesAndInterfaces(Type type)
{
    if (GetBaseTypeOrDefinition(type) is Type baseType)
    {
        yield return type;
    }
    foreach (var interfaceType in type.GetInterfaces())
    {
        yield return interfaceType.RevertGenerics();
    }
}

static string GetDirectoryClimbPath(int depth)
{
    if (depth <= 0)
    {
        return "";
    }
    var sb = new StringBuilder();
    for (int i = 0; i < depth; i++)
    {
        sb.Append("../");
    }
    return sb.ToString();
}

MethodInfo[] GetDocumentableMethods(Type type, Predicate<MethodInfo>? extraPredicate = null)
{
    return type.GetMethods(visibleBindingFlags)
        .Where(m => m.IsVisibleForDocumentation() && extraPredicate?.Invoke(m) != false)
        .OrderBy(m => m.Name)
        .ToArray();
}

static string? GetExternalUrl(string namespaceOrType, string? memberName)
{
    if (namespaceOrType.StartsWith("Microsoft.Xna.Framework"))
    {
        var anchor = !string.IsNullOrEmpty(memberName) ? $"#{namespaceOrType.Replace('.', '_')}" : "";
        return $"https://docs.monogame.net/api/{SluggifyName(namespaceOrType, true)}.html{anchor}";
    }
    if (namespaceOrType.StartsWith("System"))
    {
        var memberPath = !string.IsNullOrEmpty(memberName)
            ? string.Concat(namespaceOrType, '.', memberName)
            : namespaceOrType;
        return $"https://learn.microsoft.com/en-us/dotnet/api/{SluggifyName(memberPath)}";
    }
    return null;
}

string GetFormattedSummary(MemberInfo member)
{
    var summary = GetMemberComment(member, (CommonComments c) => c.Summary);
    return ReplaceTags(summary ?? "", e => FormatParagraphText(e, member.ReflectedType!.Namespace!));
}

TResult? GetMaybeInherited<T, TComments, TResult>(
    T? memberOrType,
    Func<T, TComments?> getComments,
    Func<T, IEnumerable<T>> getDefaultBases,
    Func<TComments, TResult?> selector
)
    where T : MemberInfo
    where TComments : CommonComments
{
    if (memberOrType is null)
    {
        return default;
    }
    var comments = getComments(memberOrType);
    if (comments is null)
    {
        return default;
    }
    var result = selector(comments);
    if ((result is not null && !(result is string s && string.IsNullOrWhiteSpace(s))) || comments.Inheritdoc is null)
    {
        return result;
    }
    var cref = comments.Inheritdoc?.Cref ?? "";
    IEnumerable<T> searchMembersOrTypes = [];
    if (string.IsNullOrEmpty(cref))
    {
        searchMembersOrTypes = getDefaultBases(memberOrType);
    }
    else if (cref.StartsWith("T:"))
    {
        if (GetType(cref[2..]) is T referenced)
        {
            searchMembersOrTypes = [referenced];
        }
    }
    else
    {
        int argumentsPos = cref.IndexOf('(');
        int memberStartIndex = cref.LastIndexOf('.', argumentsPos >= 0 ? argumentsPos : cref.Length - 1);
        var type = GetType(cref[2..memberStartIndex]);
        var members = type?.GetMember(cref[(memberStartIndex + 1)..], visibleBindingFlags);
        searchMembersOrTypes = members?.OfType<T>() ?? [];
    }
    return searchMembersOrTypes
        .Select(m => GetMaybeInherited(m, getComments, getDefaultBases, selector))
        .FirstOrDefault();
}

TResult? GetMemberComment<TResult, TComments>(MemberInfo member, Func<TComments, TResult?> selector)
    where TComments : CommonComments
{
    return GetMaybeInherited<MemberInfo, TComments, TResult>(
        member,
        m => reader.GetMemberComments(m) as TComments,
        GetBaseMembers,
        selector
    );
}

static string GetMethodAnchor(MethodBase method)
{
    string methodName = method is ConstructorInfo ? method.ReflectedType!.Name : method.Name;
    int genericStartPos = methodName.IndexOf('`');
    if (method.ContainsGenericParameters && genericStartPos < 0)
    {
        genericStartPos = methodName.Length;
    }
    if (genericStartPos >= 0)
    {
        string baseName = methodName[0..genericStartPos];
        var genericArgs =
            method is ConstructorInfo ? method.ReflectedType!.GetGenericArguments() : method.GetGenericArguments();
        methodName =
            baseName
            + "<"
            + string.Join(", ", genericArgs.Select(t => FormatGenericTypeName(t, collapsePrimitives: true)))
            + ">";
    }
    var argumentTypeNames = method
        .GetParameters()
        .Select(p => FormatGenericTypeName(p.ParameterType, escaped: false, collapsePrimitives: true));
    return SluggifyName(string.Concat(methodName, '(', string.Join(", ", argumentTypeNames), ')'));
}

static string GetNamespaceFilePath(string ns)
{
    return ns.Replace('.', '/') + "/index.md";
}

static string GetPrimitiveTypeName(Type type)
{
    return type.Name switch
    {
        "Byte" => "byte",
        "SByte" => "sbyte",
        "Int16" => "short",
        "UInt16" => "ushort",
        "Int32" => "int",
        "UInt32" => "uint",
        "Int64" => "long",
        "UInt64" => "ulong",
        "Single" => "float",
        "Double" => "double",
        "Boolean" => "bool",
        "Char" => "char",
        "Decimal" => "decimal",
        "String" => "string",
        _ => type.FullName ?? type.Name,
    };
}

string GetPropertyAnchor(PropertyInfo property)
{
    var indexParams = property.GetIndexParameters();
    if (indexParams.Length == 0)
    {
        return SluggifyName(property.Name);
    }
    var sb = new StringBuilder();
    sb.Append(property.Name);
    sb.Append('[');
    for (int i = 0; i < indexParams.Length; i++)
    {
        if (i > 0)
        {
            sb.Append(", ");
        }
        sb.Append(FormatGenericTypeName(indexParams[i].ParameterType, escaped: false, collapsePrimitives: true));
    }
    sb.Append(']');
    return SluggifyName(sb.ToString());
}

static Type? GetType(string typeName)
{
    if (Type.GetType(typeName) is Type knownType)
    {
        return knownType;
    }
    if (typeName.StartsWith("Microsoft.Xna"))
    {
        return typeof(Microsoft.Xna.Framework.Vector2).Assembly.GetType(typeName);
    }
    if (typeName.StartsWith(typeof(UI).Namespace!))
    {
        var uiAssembly = typeof(UI).Assembly;
        while (true)
        {
            var uiType = uiAssembly.GetType(typeName);
            if (uiType is not null)
            {
                return uiType;
            }
            int lastSeparatorIndex = typeName.LastIndexOf('.');
            if (lastSeparatorIndex < 0)
            {
                break;
            }
            typeName = typeName[..lastSeparatorIndex] + '+' + typeName[(lastSeparatorIndex + 1)..];
        }
    }
    return AppDomain
        .CurrentDomain.GetAssemblies()
        .Select(a => a.GetType(typeName))
        .Where(t => t is not null)
        .FirstOrDefault();
}

TResult? GetTypeComment<TResult>(Type type, Func<TypeComments, TResult?> selector)
{
    return GetMaybeInherited(type, reader.GetTypeComments, GetBaseTypesAndInterfaces, selector);
}

static string GetTypeFilePath(Type type, bool fullName = false)
{
    string baseName = fullName ? type.RevertGenerics().FullName!.Replace('.', '/').Replace('+', '.') : type.Name;
    return SluggifyName(baseName) + ".md";
}

static string MakeRelativeUrl(string pathRelativeToRoot, string referringNamespace)
{
    var parts = pathRelativeToRoot.Split('/');
    var referringParts = referringNamespace.Split('.');
    int matchingDepth = parts
        .Zip(referringParts)
        .Where(x => x.First.Equals(x.Second, StringComparison.InvariantCultureIgnoreCase))
        .Count();
    int backtrackDepth = referringParts.Length - matchingDepth;
    return GetDirectoryClimbPath(backtrackDepth) + string.Join('/', parts[matchingDepth..]);
}

static string QuoteCode(string? text)
{
    return !string.IsNullOrWhiteSpace(text) ? $"`{text}`" : "";
}

static string ReplaceElements(XElement rootElement, Func<XElement, string> replace)
{
    var sb = new StringBuilder();
    foreach (var node in rootElement.Nodes())
    {
        if (node is XText text)
        {
            sb.Append(text.Value.ReplaceLineEndings().Replace(Environment.NewLine, " "));
        }
        else if (node is XElement element)
        {
            sb.Append(replace(element));
        }
    }
    return sb.ToString();
}

static string ReplaceTags(string s, Func<XElement, string> replace)
{
    var rootElement = XElement.Parse($"<root>{s}</root>");
    return ReplaceElements(rootElement, replace);
}

static string ResolveCref(
    string crefValue,
    Func<string, string> resolveNamespace,
    Func<Type, string> resolveType,
    Func<MemberInfo, string> resolveMember,
    Func<string, string>? resolveInvalid
)
{
    if (crefValue.IndexOf(':') != 1)
    {
        return crefValue;
    }
    char prefix = crefValue[0];
    crefValue = crefValue[2..];
    resolveInvalid ??= s => s;
    return prefix switch
    {
        'N' => resolveNamespace(crefValue),
        'T' => GetType(crefValue) is Type t ? resolveType(t) : resolveInvalid(crefValue),
        'F' or 'P' or 'M' or 'E' => GetMember(crefValue, prefix) is MemberInfo m
            ? resolveMember(m)
            : resolveInvalid(crefValue),
        _ => resolveInvalid(crefValue),
    };

    MemberInfo? GetMember(string crefValue, char prefix)
    {
        int argumentsPos = crefValue.IndexOf('(');
        int memberPos = crefValue.LastIndexOf('.', argumentsPos >= 0 ? argumentsPos : crefValue.Length - 1);
        if (memberPos < 0)
        {
            throw new ArgumentException(
                $"Invalid member reference '{crefValue}', missing a '.' separator.",
                nameof(crefValue)
            );
        }
        var type = GetType(crefValue[0..memberPos]);
        if (type is null)
        {
            return null;
        }
        var argIndex = crefValue.IndexOf('(');
        if (argIndex < 0)
        {
            argIndex = crefValue.Length;
        }
        var memberName = StripOuterTypeRefs(crefValue[(memberPos + 1)..argIndex]);
        return prefix switch
        {
            'F' => type.GetField(memberName, visibleBindingFlags),
            'P' => type.GetProperty(memberName, visibleBindingFlags),
            'E' => type.GetEvent(memberName, visibleBindingFlags),
            'M' => GetMethodParamTypes(memberName) is Type[] argTypes
                ? type.GetMethod(memberName, visibleBindingFlags, argTypes)
                : type.GetMethods(visibleBindingFlags).Where(method => method.Name == memberName).FirstOrDefault(),
            _ => null,
        };
    }

    Type[]? GetMethodParamTypes(string crefValue)
    {
        var startPos = crefValue.LastIndexOf('(');
        if (startPos < 0)
        {
            return null;
        }
        var endPos = crefValue.LastIndexOf(')');
        if (endPos < startPos)
        {
            return null;
        }
        return crefValue[(startPos + 1)..endPos].Split(',').Select(x => GetType(x.Trim())!).ToArray();
    }
}

static string SluggifyName(string name, bool preserveCase = false)
{
    const string ignoreChars = "()[]<>,&@?";
    var sb = new StringBuilder();
    foreach (char c in name)
    {
        if (ignoreChars.Contains(c))
        {
            continue;
        }
        sb.Append(
            c switch
            {
                '`' or ' ' => '-',
                _ => preserveCase ? c : char.ToLowerInvariant(c),
            }
        );
    }
    return sb.ToString();
}

static string StripOuterTypeRefs(string memberName)
{
    var sb = new StringBuilder();
    int genericDepth = 0;
    foreach (char c in memberName)
    {
        if (c == '`')
        {
            genericDepth++;
        }
        else if (genericDepth > 0 && char.IsDigit(c))
        {
            if (genericDepth == 1)
            {
                // It's an argument to the member itself, emit this.
                sb.Append('`');
                sb.Append(c);
                // Allow subsequent digits to be emitted naturally.
                // Generics shouldn't really have more than 10 args, but you never know...
                genericDepth = 0;
            }
            // More than 1 level deep means the argument refers to an outer type.
            // Don't emit these, just ignore them. Don't reset generic depth either, so that subsequent digits will also
            // be skipped until we reach a non-digit.
        }
        else
        {
            sb.Append(c);
            genericDepth = 0;
        }
    }
    return sb.ToString();
}

void WriteConstructorDetail(StringBuilder sb, ConstructorInfo ctor)
{
    WriteMethodDetail(sb, ctor);
}

void WriteConstructorDetails(StringBuilder sb, Type type)
{
    var constructors = type.GetConstructors(visibleBindingFlags).Where(c => c.IsVisibleForDocumentation()).ToArray();
    if (constructors.Length == 0)
    {
        return;
    }
    sb.AppendLine("### Constructors");
    sb.AppendLine();
    foreach (var ctor in constructors)
    {
        WriteConstructorDetail(sb, ctor);
        sb.AppendLine("-----");
        sb.AppendLine();
    }
}

void WriteConstructorTable(StringBuilder sb, Type type)
{
    var constructors = type.GetConstructors(visibleBindingFlags).Where(c => c.IsVisibleForDocumentation()).ToArray();
    if (constructors.Length == 0)
    {
        return;
    }
    sb.AppendLine("### Constructors");
    sb.AppendLine();
    WriteMemberTableHeader(sb);
    foreach (var ctor in constructors)
    {
        WriteMethodRow(sb, ctor);
    }
    sb.AppendLine();
}

void WriteDefinition(StringBuilder sb, Type type, TypeComments? typeComments)
{
    string ns = type.Namespace!;
    var baseType = GetBaseTypeOrDefinition(type);
    var baseInterfaces = baseType?.GetInterfaces().ToHashSet() ?? [];
    var declaredInterfaces = type.GetInterfaces()
        .Where(t => t.IsVisibleForDocumentation() && !baseInterfaces.Contains(t))
        .ToArray();
    sb.AppendLine("## Definition");
    sb.AppendLine();
    sb.AppendLine(@"<div class=""api-definition"" markdown>");
    sb.AppendLine();
    sb.Append("Namespace: ");
    sb.Append(FormatNamespaceLink(ns, ns));
    sb.AppendLine("  ");
    sb.Append("Assembly: ");
    sb.Append(Path.GetFileName(type.Assembly.Location));
    sb.AppendLine("  ");
    // TODO: Use Mono.Cecil.Pdb to get source file, if we can figure out how to emit the PDB with SMAPI's builder.
    sb.AppendLine();
    sb.AppendLine("</div>");
    sb.AppendLine();
    var summary = GetTypeComment(type, c => !string.IsNullOrWhiteSpace(c.Summary) ? c.Summary : null);
    var parameterComments = GetTypeComment(type, c => c.Parameters.Count > 0 ? c.Parameters : null);
    if (!string.IsNullOrWhiteSpace(summary))
    {
        sb.AppendLine(ReplaceTags(summary, e => FormatParagraphText(e, ns)));
        sb.AppendLine();
    }
    sb.AppendLine("```cs");
    foreach (var attribute in type.GetCustomAttributes(false))
    {
        if (
            attribute.GetType().Namespace?.StartsWith("System.Runtime.CompilerServices") == true
            || attribute.GetType().Namespace?.StartsWith("System.Reflection") == true
        )
        {
            continue;
        }
        sb.Append('[');
        var attributeName = FormatGenericTypeName(attribute.GetType(), fullPath: true, escaped: false);
        if (attributeName.EndsWith("Attribute"))
        {
            attributeName = attributeName[0..^9];
        }
        sb.Append(attributeName);
        // TODO: Add attribute fields/props
        sb.AppendLine("]");
    }
    int lengthBeforeDeclaration = sb.Length;
    sb.Append(FormatTypeVisibility(type));
    sb.Append(' ');
    var invokeMethod = type.IsAssignableTo(typeof(Delegate)) ? type.GetMethod("Invoke") : null;
    if (invokeMethod is not null)
    {
        AppendDelegateSignature();
    }
    else
    {
        AppendTypeSignature();
    }
    sb.AppendLine();
    sb.AppendLine("```");
    sb.AppendLine();
    AppendGenericTypeArgs();
    if (invokeMethod is not null)
    {
        AppendDelegateParameters();
    }
    else
    {
        AppendInheritance();
    }

    void AppendDelegateParameters()
    {
        WriteMethodParameters(sb, invokeMethod, ns, 3, parameterComments);
    }

    void AppendDelegateSignature()
    {
        if (invokeMethod.ReturnType == typeof(void))
        {
            sb.Append("void ");
        }
        else
        {
            sb.Append(
                FormatGenericTypeName(invokeMethod.ReturnType, fullPath: true, escaped: false, collapsePrimitives: true)
            );
            sb.Append(' ');
        }
        sb.Append(FormatGenericTypeName(type, escaped: false, includeOuterClasses: true));
        sb.Append('(');
        AppendParameterList(
            sb,
            invokeMethod.GetParameters(),
            fullPath: true,
            escaped: false,
            collapsePrimitives: true,
            includeModifiers: true,
            includeNames: true
        );
        sb.Append(");");
    }

    void AppendGenericTypeArgs()
    {
        var typeArgs = type.IsGenericType ? type.GetGenericArguments() : [];
        if (typeArgs.Length > 0)
        {
            sb.AppendLine("### Type Parameters");
            sb.AppendLine();
            foreach (var argType in typeArgs)
            {
                sb.AppendLine($"**`{argType.Name}`**  ");
                var paramComment = GetTypeComment(
                    type,
                    c => c.TypeParameters?.Where(x => x.Name == argType.Name).Select(x => x.Text).FirstOrDefault()
                );
                if (!string.IsNullOrEmpty(paramComment))
                {
                    sb.AppendLine(ReplaceTags(paramComment, e => FormatParagraphText(e, ns)));
                }
                sb.AppendLine();
            }
            sb.AppendLine();
        }
    }

    void AppendInheritance()
    {
        var inheritanceElements = new Stack<string>();
        inheritanceElements.Push(FormatGenericTypeName(type, false));
        for (var super = GetBaseTypeOrDefinition(type); super is not null; super = GetBaseTypeOrDefinition(super))
        {
            if (!super.IsVisibleForDocumentation())
            {
                continue;
            }
            inheritanceElements.Push(" ⇦ ");
            inheritanceElements.Push(FormatTypeLink(super, ns));
        }
        if (inheritanceElements.Count > 1)
        {
            sb.AppendLine("**Inheritance**  ");
            foreach (var element in inheritanceElements)
            {
                sb.Append(element);
            }
            sb.AppendLine();
            sb.AppendLine();
        }
        if (declaredInterfaces.Length > 0)
        {
            sb.AppendLine("**Implements**  ");
            sb.AppendLine(string.Join(", ", declaredInterfaces.Select(t => FormatTypeLink(t, ns))));
            sb.AppendLine();
        }
    }

    void AppendTypeSignature()
    {
        if (type.GetCustomAttribute(typeof(IsReadOnlyAttribute)) is not null)
        {
            sb.Append("readonly ");
        }
        if (type.IsAbstract && type.IsSealed)
        {
            sb.Append("static ");
        }
        if (type.IsByRefLike)
        {
            sb.Append("ref ");
        }
        sb.Append(FormatTypeKind(type, true).ToLowerInvariant());
        sb.Append(' ');
        sb.Append(FormatGenericTypeName(type, escaped: false, includeOuterClasses: true));
        var hasNonDefaultBase =
            baseType is not null
            && baseType != typeof(object)
            && baseType != typeof(Enum)
            && baseType != typeof(ValueType);
        if (declaredInterfaces.Length > 0 || hasNonDefaultBase)
        {
            sb.Append(" : ");
            int lineLength = sb.Length - lengthBeforeDeclaration;
            if (hasNonDefaultBase)
            {
                AppendWrapped(
                    sb,
                    FormatGenericTypeName(baseType!, fullPath: true, escaped: false),
                    ref lineLength,
                    80,
                    4
                );
                if (declaredInterfaces.Length > 0)
                {
                    sb.Append(", ");
                    lineLength += 2;
                }
            }
            for (int i = 0; i < declaredInterfaces.Length; i++)
            {
                if (i > 0)
                {
                    sb.Append(", ");
                    lineLength += 2;
                }
                AppendWrapped(
                    sb,
                    FormatGenericTypeName(declaredInterfaces[i], fullPath: true, escaped: false),
                    ref lineLength,
                    80,
                    4
                );
            }
        }
    }
}

void WriteEventDetail(StringBuilder sb, EventInfo evt)
{
    string ns = evt.DeclaringType!.Namespace!;
    var comments = reader.GetMemberComments(evt);
    sb.Append("#### ");
    sb.AppendLine(evt.Name);
    sb.AppendLine();
    sb.AppendLine(GetFormattedSummary(evt));
    sb.AppendLine();
    sb.AppendLine("```cs");
    if (evt.DeclaringType?.IsInterface != true)
    {
        var primaryMethod = evt.AddMethod ?? evt.RemoveMethod!;
        sb.Append(FormatMethodVisibility(primaryMethod));
        sb.Append(' ');
    }
    sb.Append("event ");
    sb.Append(FormatGenericTypeName(evt.EventHandlerType!, fullPath: true, escaped: false, collapsePrimitives: true));
    sb.Append("? ");
    sb.Append(evt.Name);
    sb.AppendLine(";");
    sb.AppendLine("```");
    sb.AppendLine();
    sb.AppendLine("##### Event Type");
    sb.AppendLine();
    sb.AppendLine(FormatTypeLink(evt.EventHandlerType!, ns));
    sb.AppendLine();
    AppendMemberRemarks(sb, evt);
}

void WriteEventDetails(StringBuilder sb, Type type)
{
    var events = type.GetEvents(visibleBindingFlags)
        .Where(e => e.ReflectedType == e.DeclaringType)
        .OrderBy(f => f.Name)
        .ToArray();
    if (events.Length == 0)
    {
        return;
    }
    sb.AppendLine("### Events");
    sb.AppendLine();
    foreach (var evt in events)
    {
        WriteEventDetail(sb, evt);
        sb.AppendLine("-----");
        sb.AppendLine();
    }
}

void WriteEventRow(StringBuilder sb, EventInfo @event)
{
    WriteMemberRow(sb, @event);
}

void WriteEventTable(StringBuilder sb, Type type)
{
    var events = type.GetEvents(visibleBindingFlags).OrderBy(f => f.Name).ToArray();
    if (events.Length == 0)
    {
        return;
    }
    sb.AppendLine("### Events");
    sb.AppendLine();
    WriteMemberTableHeader(sb);
    foreach (var @event in events)
    {
        WriteEventRow(sb, @event);
    }
    sb.AppendLine();
}

void WriteFieldDetail(StringBuilder sb, FieldInfo field)
{
    string ns = field.DeclaringType!.Namespace!;
    var comments = reader.GetMemberComments(field);
    sb.Append("#### ");
    sb.AppendLine(field.Name);
    sb.AppendLine();
    sb.AppendLine(GetFormattedSummary(field));
    sb.AppendLine();
    sb.AppendLine("```cs");
    if (field.DeclaringType?.IsInterface != true)
    {
        sb.Append(FormatFieldVisibility(field));
        sb.Append(' ');
    }
    if (field.IsStatic)
    {
        sb.Append("static ");
    }
    if (field.IsLiteral)
    {
        sb.Append("const ");
    }
    if (field.IsInitOnly)
    {
        sb.Append("readonly ");
    }
    sb.Append(FormatGenericTypeName(field.FieldType, fullPath: true, escaped: false, collapsePrimitives: true));
    sb.Append(' ');
    sb.Append(field.Name);
    sb.AppendLine(";");
    sb.AppendLine("```");
    sb.AppendLine();
    sb.AppendLine("##### Field Value");
    sb.AppendLine();
    sb.AppendLine(FormatTypeLink(field.FieldType, ns));
    sb.AppendLine();
    AppendMemberRemarks(sb, field);
}

void WriteFieldDetails(StringBuilder sb, Type type)
{
    var fields = type.GetFields(visibleBindingFlags)
        .Where(f => f.ReflectedType == f.DeclaringType && f.IsVisibleForDocumentation())
        .OrderBy(f => f.Name)
        .ToArray();
    if (fields.Length == 0)
    {
        return;
    }
    sb.AppendLine("### Fields");
    sb.AppendLine();
    foreach (var field in fields)
    {
        WriteFieldDetail(sb, field);
        sb.AppendLine("-----");
        sb.AppendLine();
    }
}

void WriteFieldRow(StringBuilder sb, FieldInfo field)
{
    if (field.DeclaringType?.IsEnum == true)
    {
        if (field.IsSpecialName || !field.IsStatic)
        {
            return;
        }
        var summary = GetFormattedSummary(field);
        sb.Append("| ");
        sb.Append(@$"<a id=""{field.Name.ToLowerInvariant()}"">{field.Name}</a>");
        sb.Append(" | ");
        sb.Append(field.GetRawConstantValue());
        sb.Append(" | ");
        sb.Append(summary);
        sb.AppendLine(" | ");
    }
    else
    {
        WriteMemberRow(sb, field);
    }
}

void WriteFieldTable(StringBuilder sb, Type type, EnumComments? enumComments = null)
{
    var fields = type.GetFields(visibleBindingFlags)
        .Where(f => f.IsVisibleForDocumentation())
        .OrderBy(f => type.IsEnum ? (f.IsStatic && !f.IsSpecialName ? f.GetRawConstantValue() : -1) : f.Name)
        .ToArray();
    if (fields.Length == 0)
    {
        return;
    }
    sb.AppendLine(enumComments is not null ? "## Fields" : "### Fields");
    sb.AppendLine();
    WriteMemberTableHeader(sb, enumComments is not null);
    foreach (var field in fields)
    {
        WriteFieldRow(sb, field);
    }
    sb.AppendLine();
}

static void WriteFrontMatter(StringBuilder sb, string title, string? description)
{
    sb.AppendLine("---");
    sb.AppendLine($"title: {EscapeYaml(title)}");
    description = EscapeYaml(description ?? "");
    if (!string.IsNullOrWhiteSpace(description))
    {
        sb.AppendLine($"description: {description}");
    }
    sb.AppendLine("---");
    sb.AppendLine();
}

void WriteMemberRow(StringBuilder sb, MemberInfo member, string? name = null, Func<string>? getAnchor = null)
{
    var summary = GetFormattedSummary(member);
    var link =
        member.ReflectedType == member.DeclaringType
            ? FormatLink(name ?? member.Name, "#" + (getAnchor?.Invoke() ?? member.Name.ToLowerInvariant()))
        : member.DeclaringTypeOrDefinition() is Type declaringType
            ? FormatMemberLink(
                declaringType.GetMemberWithSameMetadataDefinitionAs(member),
                member.ReflectedType!.Namespace!
            )
        : FormatMemberLink(member, member.ReflectedType!.Namespace!);
    sb.Append("| ");
    sb.Append(link);
    sb.Append(" | ");
    AppendWithInheritance(
        sb,
        summary,
        member.DeclaringTypeOrDefinition(),
        member.OriginalDefinition(),
        member.ReflectedType!
    );
    sb.AppendLine(" | ");
}

static void WriteMemberTableHeader(StringBuilder sb, bool includeValueColumn = false)
{
    sb.Append(" | Name | ");
    if (includeValueColumn)
    {
        sb.Append("Value | ");
    }
    sb.AppendLine("Description |");
    if (includeValueColumn)
    {
        sb.Append("| --- ");
    }
    sb.AppendLine("| --- | --- |");
}

void WriteMethodDetail(StringBuilder sb, MethodBase method)
{
    string ns = method.DeclaringType!.Namespace!;
    var (returnType, baseDefinition) = method is MethodInfo methodInfo
        ? (methodInfo.ReturnType, methodInfo.GetBaseDefinition() as MethodBase)
        : (null, method);
    var comments = reader.GetMethodComments(method);
    var isInterfaceMember = method.DeclaringType?.IsInterface == true;
    sb.Append("#### ");
    sb.AppendLine(FormatName(false));
    sb.AppendLine();
    sb.AppendLine(GetFormattedSummary(method));
    sb.AppendLine();
    sb.AppendLine("```cs");
    if (!isInterfaceMember)
    {
        sb.Append(FormatMethodVisibility(method));
        sb.Append(' ');
    }
    if (method.IsStatic)
    {
        sb.Append("static ");
    }
    if (method.IsVirtual && !isInterfaceMember)
    {
        if (method == baseDefinition)
        {
            if (!method.IsFinal)
            {
                sb.Append("virtual ");
            }
        }
        else
        {
            if (method.IsFinal)
            {
                sb.Append("sealed ");
            }
            sb.Append("override ");
        }
    }
    if (returnType == typeof(void))
    {
        sb.Append("void ");
    }
    else if (returnType is not null)
    {
        sb.Append(FormatGenericTypeName(returnType, fullPath: true, escaped: false, collapsePrimitives: true));
        sb.Append(' ');
    }
    sb.Append(FormatName(true));
    sb.AppendLine(";");
    sb.AppendLine("```");
    sb.AppendLine();
    WriteMethodParameters(sb, method, ns, 5);
    if (returnType is not null && returnType != typeof(void))
    {
        sb.AppendLine("##### Returns");
        sb.AppendLine();
        sb.AppendLine(returnType.IsGenericParameter ? QuoteCode(returnType.Name) : FormatTypeLink(returnType, ns));
        sb.AppendLine();
        if (!string.IsNullOrEmpty(comments?.Returns))
        {
            sb.Append("  ");
            sb.AppendLine(ReplaceTags(comments.Returns, e => FormatParagraphText(e, ns)).Trim());
            sb.AppendLine();
        }
    }
    AppendMemberRemarks(sb, method);

    string FormatName(bool asCode)
    {
        return method is ConstructorInfo ctor
            ? FormatConstructorName(
                ctor,
                fullPath: asCode,
                escaped: !asCode,
                collapsePrimitives: true,
                includeParameterNames: asCode
            )
            : FormatGenericMethodName(
                method,
                fullPath: asCode,
                escaped: !asCode,
                collapsePrimitives: true,
                includeParameterModifiers: asCode,
                includeParameterNames: asCode
            );
    }
}

void WriteMethodDetails(StringBuilder sb, Type type)
{
    var methods = GetDocumentableMethods(type, m => m.ReflectedType == m.DeclaringType);
    if (methods.Length == 0)
    {
        return;
    }
    sb.AppendLine("### Methods");
    sb.AppendLine();
    foreach (var method in methods)
    {
        WriteMethodDetail(sb, method);
        sb.AppendLine("-----");
        sb.AppendLine();
    }
}

void WriteMethodParameters(
    StringBuilder sb,
    MethodBase method,
    string ns,
    int level,
    IReadOnlyList<(string Name, string Text)>? parameterComments = null
)
{
    var parameters = method.GetParameters();
    if (parameters.Length > 0)
    {
        sb.Append(new string('#', level));
        sb.Append(' ');
        sb.AppendLine("Parameters");
        sb.AppendLine();
        foreach (var param in parameters)
        {
            sb.Append("**`");
            sb.Append(param.Name);
            sb.Append("`** &nbsp; ");
            sb.Append(
                param.ParameterType.IsGenericParameter
                    ? param.ParameterType.Name
                    : FormatTypeLink(param.ParameterType, ns)
            );
            var description = parameterComments is not null
                ? GetParameterDescription(parameterComments, param.Name)
                : GetMaybeInherited(
                    method,
                    m => reader.GetMethodComments(m),
                    m => GetBaseMembers(m).OfType<MethodBase>(),
                    (MethodComments c) => GetParameterDescription(c.Parameters, param.Name)
                );
            if (!string.IsNullOrWhiteSpace(description))
            {
                sb.AppendLine("  ");
                sb.AppendLine(ReplaceTags(description, e => FormatParagraphText(e, ns)));
            }
            else
            {
                sb.AppendLine();
            }
            sb.AppendLine();
        }
    }

    static string? GetParameterDescription(
        IReadOnlyList<(string Name, string Text)> parameterComments,
        string? paramName
    )
    {
        return parameterComments
            .Where(p => p.Name == paramName)
            .Select(p => p.Text)
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .FirstOrDefault();
    }
}

void WriteMethodRow(StringBuilder sb, MethodBase method)
{
    var formattedName = method is ConstructorInfo ctor ? FormatConstructorName(ctor) : FormatGenericMethodName(method);
    WriteMemberRow(sb, method, formattedName, () => GetMethodAnchor(method));
}

void WriteMethodTable(StringBuilder sb, Type type)
{
    var methods = GetDocumentableMethods(type);
    if (methods.Length == 0)
    {
        return;
    }
    sb.AppendLine("### Methods");
    sb.AppendLine();
    WriteMemberTableHeader(sb);
    foreach (var method in methods)
    {
        WriteMethodRow(sb, method);
    }
    sb.AppendLine();
}

async Task WriteNamespaceToFile(string ns, IEnumerable<Type> types)
{
    var typesByKind = types.ToLookup(t => FormatTypeKind(t));
    var fileName = Path.Combine(outputDirectory, GetNamespaceFilePath(ns));
    var sb = new StringBuilder();
    WriteFrontMatter(sb, ns, null);
    sb.AppendLine(@"<link rel=""stylesheet"" href=""/StardewUI/stylesheets/reference.css"" />");
    sb.AppendLine();
    sb.AppendLine("/// html | div.api-reference");
    sb.AppendLine();
    sb.AppendLine($"# {ns} Namespace");
    sb.AppendLine();
    foreach (var (kind, title) in namespaceSections)
    {
        var typesInSection = typesByKind[kind].OrderBy(t => t.FullName).ToList();
        if (typesInSection.Count == 0)
        {
            continue;
        }
        sb.Append("## ");
        sb.AppendLine(title);
        sb.AppendLine();
        sb.AppendLine("| Name | Description |");
        sb.AppendLine("| --- | --- |");
        foreach (var type in typesInSection)
        {
            var summary = GetTypeComment(type, c => !string.IsNullOrWhiteSpace(c.Summary) ? c.Summary : null);
            sb.Append("| ");
            sb.Append(FormatTypeLink(type, ns, includeOuterClasses: true));
            sb.Append(" | ");
            sb.Append(ReplaceTags(summary ?? "", e => FormatParagraphText(e, ns)));
            sb.AppendLine(" |");
        }
        sb.AppendLine();
    }
    await File.WriteAllTextAsync(fileName, sb.ToString());
}

void WritePropertyDetail(StringBuilder sb, PropertyInfo property)
{
    string ns = property.DeclaringType!.Namespace!;
    var comments = reader.GetMemberComments(property);
    sb.Append("#### ");
    sb.AppendLine(FormatPropertyName(property, collapsePrimitives: true));
    sb.AppendLine();
    sb.AppendLine(GetFormattedSummary(property));
    sb.AppendLine();
    sb.AppendLine("```cs");
    var getMethod = property.GetGetMethod(true);
    var setMethod = property.GetSetMethod(true);
    var primaryMethod = getMethod ?? setMethod!;
    string primaryMethodVisibility = FormatMethodVisibility(primaryMethod);
    if (property.DeclaringType?.IsInterface != true)
    {
        sb.Append(primaryMethodVisibility);
        sb.Append(' ');
    }
    if (primaryMethod.IsStatic)
    {
        sb.Append("static ");
    }
    sb.Append(FormatGenericTypeName(property.PropertyType, fullPath: true, escaped: false, collapsePrimitives: true));
    sb.Append(' ');
    sb.Append(FormatPropertyName(property, fullPath: true, escaped: false, collapsePrimitives: true));
    sb.Append(" { ");
    if (getMethod is not null)
    {
        sb.Append("get; ");
    }
    if (setMethod is not null)
    {
        string setMethodVisibility = FormatMethodVisibility(setMethod);
        if (setMethodVisibility != primaryMethodVisibility)
        {
            sb.Append(setMethodVisibility);
            sb.Append(' ');
        }
        sb.Append("set; ");
    }
    sb.AppendLine("}");
    sb.AppendLine("```");
    sb.AppendLine();
    sb.AppendLine("##### Property Value");
    sb.AppendLine();
    sb.AppendLine(FormatTypeLink(property.PropertyType, ns));
    sb.AppendLine();
    if (
        getMethod is not null
        && reader.GetMethodComments(getMethod) is MethodComments getterComments
        && !string.IsNullOrWhiteSpace(getterComments.Returns)
    )
    {
        sb.AppendLine(ReplaceTags(getterComments.Returns, e => FormatParagraphText(e, ns)));
        sb.AppendLine();
    }
    AppendMemberRemarks(sb, property);
}

void WritePropertyDetails(StringBuilder sb, Type type)
{
    var properties = type.GetProperties(visibleBindingFlags)
        .Where(p => p.ReflectedType == p.DeclaringType && p.IsVisibleForDocumentation())
        .OrderBy(p => p.Name)
        .ToArray();
    if (properties.Length == 0)
    {
        return;
    }
    sb.AppendLine("### Properties");
    sb.AppendLine();
    foreach (var property in properties)
    {
        WritePropertyDetail(sb, property);
        sb.AppendLine("-----");
        sb.AppendLine();
    }
}

void WritePropertyRow(StringBuilder sb, PropertyInfo property)
{
    string name = FormatPropertyName(property, false, true, true);
    WriteMemberRow(sb, property, name, () => GetPropertyAnchor(property));
}

void WritePropertyTable(StringBuilder sb, Type type)
{
    var properties = type.GetProperties(visibleBindingFlags)
        .Where(p => p.IsVisibleForDocumentation())
        .OrderBy(p => p.Name)
        .ToArray();
    if (properties.Length == 0)
    {
        return;
    }
    sb.AppendLine("### Properties");
    sb.AppendLine();
    WriteMemberTableHeader(sb);
    foreach (var property in properties)
    {
        WritePropertyRow(sb, property);
    }
    sb.AppendLine();
}

void WriteRemarks(StringBuilder sb, Type type)
{
    var remarks = GetTypeComment(type, c => !string.IsNullOrWhiteSpace(c.Remarks) ? c.Remarks : null);
    if (string.IsNullOrEmpty(remarks))
    {
        return;
    }
    sb.AppendLine("## Remarks");
    sb.AppendLine();
    sb.AppendLine(ReplaceTags(remarks, e => FormatParagraphText(e, type.Namespace!)).Trim());
    sb.AppendLine();
}

static void WriteTitle(StringBuilder sb, Type type)
{
    sb.Append("# ");
    sb.Append(FormatTypeKind(type));
    sb.Append(' ');
    sb.AppendLine(FormatGenericTypeName(type, includeOuterClasses: true));
    sb.AppendLine();
}

async Task WriteTypeFile(Type type)
{
    var typeComments = reader.GetTypeComments(type);
    var fileName = Path.Combine(outputDirectory, GetTypeFilePath(type, true));
    var sb = new StringBuilder();
    WriteTypeFrontMatter(sb, type, typeComments);
    sb.AppendLine(@"<link rel=""stylesheet"" href=""/StardewUI/stylesheets/reference.css"" />");
    sb.AppendLine();
    sb.AppendLine("/// html | div.api-reference");
    sb.AppendLine();
    WriteTitle(sb, type);
    WriteDefinition(sb, type, typeComments);
    WriteRemarks(sb, type);
    if (type.IsEnum)
    {
        var enumComments = reader.GetEnumComments(type);
        WriteFieldTable(sb, type, enumComments);
    }
    else if (!type.IsAssignableTo(typeof(Delegate)))
    {
        sb.AppendLine("## Members");
        sb.AppendLine();
        WriteConstructorTable(sb, type);
        WriteFieldTable(sb, type);
        WritePropertyTable(sb, type);
        WriteMethodTable(sb, type);
        WriteEventTable(sb, type);
        sb.AppendLine("## Details");
        sb.AppendLine();
        WriteConstructorDetails(sb, type);
        WriteFieldDetails(sb, type);
        WritePropertyDetails(sb, type);
        WriteMethodDetails(sb, type);
        WriteEventDetails(sb, type);
    }
    Directory.CreateDirectory(Path.GetDirectoryName(fileName)!);
    await File.WriteAllTextAsync(fileName, sb.ToString());
}

static void WriteTypeFrontMatter(StringBuilder sb, Type type, TypeComments? typeComments)
{
    var title = FormatGenericTypeName(type, includeOuterClasses: true);
    var description = typeComments is not null ? ReplaceTags(typeComments.Summary, FormatSimpleText) : null;
    WriteFrontMatter(sb, title, description);
}

class ListNumberer
{
    public int ItemNumber { get; set; } = 1;
}

class FixedUnindentXmlReader
{
    private static readonly Regex genericArgRegex = new(@"`\d+", RegexOptions.Compiled);
    private static readonly XPathNavigator nullNavigator = new XPathDocument(
        new StringReader(@"<?xml version=""1.0""?><invalid />")
    ).CreateNavigator();

    private readonly DocXmlReader reader = new(unindentText: false);

    public MethodComments GetComments(MethodBase methodInfo, MethodComments comments, XPathNavigator xmlMemberNode)
    {
        FixAssemblyNavigators();
        lock (reader)
        {
            comments = reader.GetComments(methodInfo, comments, xmlMemberNode);
        }
        FixMethodComments(comments);
        return comments;
    }

    public EnumComments GetEnumComments(Type type)
    {
        EnumComments comments;
        lock (reader)
        {
            comments = reader.GetEnumComments(type);
        }
        FixEnumComments(comments);
        return comments;
    }

    public CommonComments GetMemberComments(MemberInfo member)
    {
        if (member is MethodBase method)
        {
            return GetMethodComments(method);
        }
        CommonComments comments;
        lock (reader)
        {
            comments = reader.GetMemberComments(member);
        }
        if (comments is MethodComments methodComments)
        {
            FixMethodComments(methodComments);
        }
        else
        {
            FixCommonComments(comments);
        }
        return comments;
    }

    public MethodComments GetMethodComments(MethodBase method)
    {
        method = method.RevertGenerics();
        var methodId = method.MethodId();
        XPathNavigator xmlMemberNode;
        lock (reader)
        {
            xmlMemberNode = GetXmlMemberNode(methodId, method.ReflectedType);
            if (xmlMemberNode is null)
            {
                methodId = FixMethodId(methodId, method);
                xmlMemberNode = GetXmlMemberNode(methodId, method.ReflectedType);
            }
        }
        var comments = new MethodComments();
        return GetComments(method, comments, xmlMemberNode);
    }

    public TypeComments GetTypeComments(Type type)
    {
        TypeComments comments;
        lock (reader)
        {
            comments = reader.GetTypeComments(type);
        }
        FixTypeComments(comments);
        return comments;
    }

    public XPathNavigator GetXmlMemberNode(string methodId, Type? type)
    {
        return reader.GetXmlMemberNode(methodId, type);
    }

    // Docxml puts null navigators into the dictionary, and does so deliberately, yet has methods that iterate through
    // the entire list and do not handle null.
    private void FixAssemblyNavigators()
    {
        foreach (var (key, navigator) in reader.assemblyNavigators)
        {
            if (navigator is null)
            {
                reader.assemblyNavigators[key] = nullNavigator;
            }
        }
    }

    // Unindenting is broken in the library, so we do our own.
    private static string FixComments(string comments)
    {
        if (string.IsNullOrEmpty(comments))
        {
            return comments;
        }
        comments = comments.Trim().ReplaceLineEndings();
        int newLineIndex;
        while ((newLineIndex = comments.IndexOf(Environment.NewLine)) >= 0)
        {
            int indentEndIndex = -1;
            for (int i = newLineIndex + Environment.NewLine.Length; i < comments.Length; i++)
            {
                if (comments[i] != ' ')
                {
                    indentEndIndex = i;
                    break;
                }
            }
            if (indentEndIndex == -1)
            {
                break;
            }
            comments = comments.Replace(comments[newLineIndex..indentEndIndex], " ");
        }
        return comments;
    }

    private static void FixCommonComments(CommonComments comments)
    {
        comments.Summary = FixComments(comments.Summary);
        comments.Remarks = FixComments(comments.Remarks);
    }

    private static void FixEnumComments(EnumComments comments)
    {
        FixCommonComments(comments);
        foreach (var valueComment in comments.ValueComments)
        {
            valueComment.Summary = FixComments(valueComment.Summary);
            valueComment.Remarks = FixComments(valueComment.Remarks);
        }
    }

    private static void FixMethodComments(MethodComments comments)
    {
        FixCommonComments(comments);
        for (int i = 0; i < comments.TypeParameters.Count; i++)
        {
            comments.TypeParameters[i] = (
                comments.TypeParameters[i].Name,
                FixComments(comments.TypeParameters[i].Text)
            );
        }
        for (int i = 0; i < comments.Parameters.Count; i++)
        {
            comments.Parameters[i] = (comments.Parameters[i].Name, FixComments(comments.Parameters[i].Text));
        }
        comments.Returns = FixComments(comments.Returns);
        for (int i = 0; i < comments.Exceptions.Count; i++)
        {
            comments.Exceptions[i] = (comments.Exceptions[i].Cref, FixComments(comments.Exceptions[i].Text));
        }
    }

    private static string FixMethodId(string methodId, MethodBase method)
    {
        var genericArgs = method.IsGenericMethod
            ? method
                .RevertGenerics()
                .GetParameters()
                .SelectMany(p =>
                {
                    var realArgs = p.ParameterType.GetGenericArguments();
                    var definitionArgs = p.ParameterType.RevertGenerics().GetGenericArguments();
                    return realArgs.Zip(definitionArgs, (r, d) => r.IsGenericMethodParameter ? r : d);
                })
                .ToArray()
            : [];
        if (genericArgs.Length == 0)
        {
            return methodId;
        }
        var argStartPos = methodId.IndexOf('(') + 1;
        string argsString = methodId[argStartPos..];
        int startPos = 0;
        foreach (var genericArg in genericArgs)
        {
            var qualifier = genericArg.IsGenericMethodParameter ? "``" : "`";
            var index = genericArg.GenericParameterPosition;
            var genericRef = string.Concat("{", qualifier, index, "}");
            var nextMatch = genericArgRegex.Match(argsString, startPos);
            if (!nextMatch.Success)
            {
                break;
            }
            argsString = genericArgRegex.Replace(argsString, genericRef, 1, startPos);
            startPos = nextMatch.Index + 3;
        }
        return methodId[..argStartPos] + argsString;
    }

    private static void FixTypeComments(TypeComments comments)
    {
        FixCommonComments(comments);
        for (int i = 0; i < comments.TypeParameters.Count; i++)
        {
            comments.TypeParameters[i] = (
                comments.TypeParameters[i].Name,
                FixComments(comments.TypeParameters[i].Text)
            );
        }
        for (int i = 0; i < comments.Parameters.Count; i++)
        {
            comments.Parameters[i] = (comments.Parameters[i].Name, FixComments(comments.Parameters[i].Text));
        }
    }
}

static class ReflectionExtensions
{
    public static Type? DeclaringTypeOrDefinition(this MemberInfo member)
    {
        return member.DeclaringType?.RevertGenerics();
    }

    public static bool IsCompilerGenerated(this MemberInfo member)
    {
        return member.GetCustomAttribute<CompilerGeneratedAttribute>() is not null;
    }

    public static bool IsShadowed(this MethodBase method)
    {
        if (!method.IsHideBySig || method.ReflectedType is null)
        {
            return false;
        }
        var flags = method.IsPublic ? BindingFlags.Public : BindingFlags.NonPublic;
        flags |= method.IsStatic ? BindingFlags.Static : BindingFlags.Instance;
        var paramTypes = method.GetParameters().Select(p => p.ParameterType).ToArray();
        var foundMethod = method.ReflectedType.GetMethod(method.Name, flags, null, paramTypes, null);
        return foundMethod is not null
            && foundMethod != method
            && foundMethod.Attributes.HasFlag(MethodAttributes.NewSlot);
    }

    public static bool IsVisibleForDocumentation(this EventInfo evt)
    {
        return evt.AddMethod?.IsVisibleForDocumentation(true) == true
            || evt.RemoveMethod?.IsVisibleForDocumentation(true) == true;
    }

    public static bool IsVisibleForDocumentation(this FieldInfo field)
    {
        return (field.IsPublic || field.IsFamily || field.IsFamilyOrAssembly)
            && !field.IsCompilerGenerated()
            && field.ReflectedType?.RevertGenerics().IsVisibleForDocumentation() == true;
    }

    public static bool IsVisibleForDocumentation(this MemberInfo member)
    {
        return member switch
        {
            EventInfo evt => evt.IsVisibleForDocumentation(),
            FieldInfo field => field.IsVisibleForDocumentation(),
            MethodBase method => method.IsVisibleForDocumentation(),
            PropertyInfo property => property.IsVisibleForDocumentation(),
            TypeInfo type => type.IsVisibleForDocumentation(),
            _ => member.ReflectedType?.IsVisibleForDocumentation() == true,
        };
    }

    public static bool IsVisibleForDocumentation(this MethodBase method, bool asAccessor = false)
    {
        return (method.IsPublic || method.IsFamily || method.IsFamilyOrAssembly)
            && (asAccessor || ((!method.IsSpecialName || method.IsConstructor) && !method.IsCompilerGenerated()))
            && (method.DeclaringType != typeof(object) && method.DeclaringType != typeof(Enum))
            && method.ReflectedType?.RevertGenerics().IsVisibleForDocumentation() == true
            && (asAccessor || !method.IsShadowed());
    }

    public static bool IsVisibleForDocumentation(this PropertyInfo property)
    {
        return property.GetMethod?.IsVisibleForDocumentation(true) == true
            || property.SetMethod?.IsVisibleForDocumentation(true) == true;
    }

    public static bool IsVisibleForDocumentation(this Type type)
    {
        if (
            type.Namespace?.StartsWith("System") == true
            || type.Namespace?.StartsWith("Microsoft.Xna.Framework") == true
        )
        {
            return true;
        }
        if (
            type.IsSpecialName
            || (!type.IsPublic && !type.IsNestedPublic && !type.IsNestedFamily && !type.IsNestedFamORAssem)
        )
        {
            return false;
        }
        if (type.DeclaringType is Type outerType)
        {
            return outerType.IsVisibleForDocumentation();
        }
        return true;
    }

    public static MemberInfo? OriginalDefinition(this MemberInfo member)
    {
        return member switch
        {
            MethodInfo method => method.GetBaseDefinition(),
            PropertyInfo property => (property.GetMethod ?? property.SetMethod)?.GetBaseDefinition(),
            EventInfo evt => (evt.AddMethod ?? evt.RemoveMethod)?.GetBaseDefinition(),
            _ => member,
        };
    }

    public static MethodBase RevertGenerics(this MethodBase method)
    {
        return method.IsGenericMethod && !method.IsGenericMethodDefinition
            ? ((MethodInfo)method).GetGenericMethodDefinition()
            : method;
    }

    public static Type RevertGenerics(this Type type)
    {
        var realType = type.IsByRef ? type.GetElementType()! : type;
        return realType.IsGenericType && !realType.IsGenericTypeDefinition ? realType.GetGenericTypeDefinition() : type;
    }
}

static class StringBuilderExtensions
{
    public static void AppendEscaped(this StringBuilder sb, string text)
    {
        text = text.Replace("<", "&lt;").Replace(">", "&gt;");
        sb.Append(text);
    }
}
