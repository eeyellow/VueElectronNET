using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Xml;


namespace ElectronApp.Tools
{
    /// <summary> C# Type 轉 Javascript Class </summary>
    public static class TypeTransferExtension
    {
        private static readonly Type[] _nonPrimitivesExcludeList =
        {
            typeof(object), typeof(string), typeof(decimal), typeof(void),
        };

        private static readonly IDictionary<Type, string> _convertedTypes = new Dictionary<Type, string>
        {
            [typeof(string)] = "string",
            [typeof(char)] = "string",
            [typeof(byte)] = "number",
            [typeof(sbyte)] = "number",
            [typeof(short)] = "number",
            [typeof(ushort)] = "number",
            [typeof(int)] = "number",
            [typeof(uint)] = "number",
            [typeof(long)] = "number",
            [typeof(ulong)] = "number",
            [typeof(float)] = "number",
            [typeof(double)] = "number",
            [typeof(decimal)] = "number",
            [typeof(bool)] = "boolean",
            [typeof(object)] = "any",
            [typeof(void)] = "void",
            [typeof(StringValues)] = "string",
            [typeof(DateTime)] = "datetime",
            [typeof(DateTime?)] = "datetime",
        };

        private static readonly IDictionary<string, string> _typeInitVal = new Dictionary<string, string>
        {
            ["string"] = "\"\"",
            ["number"] = "0",
            ["boolean"] = "false",
            ["void"] = "null",
            ["datetime"] = "\"\"",
            ["any"] = "new Object()",
        };

        /// <summary> 產生Script類型 </summary>
        public enum GenerateScriptType
        {
            /// <summary> All </summary>
            [Description("All")]
            All = 0,
            /// <summary> Javascript </summary>
            [Description("Javascript")]
            Javascript = 1,
            /// <summary> Typescript </summary>
            [Description("Typescript")]
            Typescript = 2,
        }

        /// <summary> 產生Script </summary>
        /// <param name="rootPath"></param>
        /// <param name="scriptType"></param>
        public static Dictionary<string, string> Generate(string rootPath, GenerateScriptType scriptType = GenerateScriptType.Javascript)
        {
            var resultDictionary = new Dictionary<string, string>();

            // 清除舊檔案
            var folderPath = Path.Combine(rootPath, "TypeModules");
            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, true);
            }

            var assemblies = Assembly.GetExecutingAssembly();
            var viewModelsToConvert = GetViewModelsToConvert(assemblies).ToList();
            var allTypesWithNestedList = new List<Type>();
            viewModelsToConvert.ForEach(x =>
            {
                allTypesWithNestedList.AddRange(GetAllNestedTypes(x, []));
            });

            var csTypeList = allTypesWithNestedList.Union(viewModelsToConvert).Distinct().ToList();
            var enumTypeList = GetEnumsToConvert(assemblies).ToList();

            #region TypeScript未完成

            //if (scriptType == GenerateScriptTypeEnum.All || scriptType == GenerateScriptTypeEnum.Typescript)
            //{
            //    #region 產生TS
            //    foreach (Type type in typesToConvert)
            //    {
            //        var tsType = ConvertCs2Ts(type);
            //        var fullPath = Path.Combine(path, tsType.Name);

            //        var directory = Path.GetDirectoryName(fullPath);
            //        if (!Directory.Exists(directory))
            //        {
            //            Directory.CreateDirectory(directory);
            //        }

            //        File.WriteAllLines(fullPath, tsType.Lines);
            //    }
            //    #endregion 產生TS
            //}

            #endregion

            if (scriptType is GenerateScriptType.All or GenerateScriptType.Javascript)
            {
                #region 產生 ViewModels Javascript

                foreach (var type in csTypeList)
                {
                    var jsType = ConvertCsToJs(type);
                    var filePath = Path.Combine(folderPath, jsType.FilePath.Replace("ElectronApp\\", ""));
                    var directory = Path.GetDirectoryName(filePath) ?? string.Empty;
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    if (!resultDictionary.ContainsKey(filePath))
                    {
                        File.WriteAllLines(filePath, jsType.Lines);
                        resultDictionary.TryAdd(filePath, jsType.FilePath);
                    }
                }

                #endregion

                #region 產生 Enums Javascript

                var enumPath = Path.Combine(folderPath, "Enums");
                var enumFullPath = Path.Combine(enumPath, "EnumType.js");
                var enumDirectory = Path.GetDirectoryName(enumFullPath) ?? string.Empty;
                if (!Directory.Exists(enumDirectory))
                {
                    Directory.CreateDirectory(enumDirectory);
                }

                var enumTextList = new List<string>
                {
                    "/* ===== 此檔案是自動產生 ===== */", "/* ===== 請勿手動變更修改 ===== */", ""
                };
                foreach (var type in enumTypeList)
                {
                    var enumLines = ConvertEnum2Js(type);
                    enumTextList.AddRange(enumLines);
                }

                File.WriteAllLines(enumFullPath, enumTextList);
                resultDictionary.TryAdd(enumFullPath, enumFullPath);

                #endregion
            }

            return resultDictionary;
        }

        private static (bool, Type) ReplaceByGenericArgument(Type type)
        {
            if (type.IsArray)
            {
                return (true, type.GetElementType() ?? typeof(Type));
            }

            if (!type.IsConstructedGenericType)
            {
                return (true, type);
            }

            var genericArgument = type.GenericTypeArguments.First();

            var isTask = type.GetGenericTypeDefinition() == typeof(Task<>);
            var isActionResult = type.GetGenericTypeDefinition() == typeof(ActionResult<>);
            var isEnumerable = typeof(IEnumerable<>).MakeGenericType(genericArgument).IsAssignableFrom(type);

            if (!isTask && !isActionResult && !isEnumerable)
            {
                return (false, type);
            }

            if (genericArgument.IsConstructedGenericType)
            {
                return ReplaceByGenericArgument(genericArgument);
            }

            return (true, genericArgument);
        }
        private static IEnumerable<Type> GetViewModelsToConvert(Assembly assembly)
        // !x.IsGenericType: 排除泛型
        // x.IsNestedPublic: 把FvmLogin底下的也納進來
        // x.GetProperties().Length > 0 || x.GetFields().Length > 0: 排除沒有欄位的(FvmLogin)
            => assembly.GetTypes().Where(x => !x.IsAbstract && (x.IsPublic || x.IsNestedPublic) && !x.IsGenericType &&
                                              (x.GetProperties().Length > 0 || x.GetFields().Length > 0) && (x.Namespace ?? string.Empty).Contains(".ViewModels"))
                       .ToList()
                       .Select(ReplaceByGenericArgument)
                       .Where(t => t.Item1)
                       .Select(t => t.Item2)
                       .Where(t => !t.IsPrimitive && !_nonPrimitivesExcludeList.Contains(t))
                       .Distinct()
                       .ToList();
        private static IEnumerable<Type> GetEnumsToConvert(Assembly assembly)
            => assembly.GetTypes().Where(x => !x.IsAbstract && (x.IsPublic || x.IsNestedPublic) && !x.IsGenericType &&
                                              (x.GetProperties().Length > 0 || x.GetFields().Length > 0) && (x.Namespace ?? string.Empty).Contains(".Enums"))
                       .ToList()
                       .Select(ReplaceByGenericArgument)
                       .Where(t => t.Item1)
                       .Select(t => t.Item2)
                       .Where(t => !t.IsPrimitive && !_nonPrimitivesExcludeList.Contains(t))
                       .Distinct()
                       .ToList();
        private static (string FileName, string FilePath) TransferTypeToFileName(Type type, string extension = "js")
        {
            var fileName = (type.FullName ?? string.Empty).Split(".").LastOrDefault() ?? string.Empty;
            fileName = fileName.Contains('+') ? fileName.Replace('+', '_') : fileName;
            return (fileName, $@"{(type.Namespace?.Replace(".", @"\") ?? "EmptyNameSpace")}\{fileName}.{extension}".Replace("Template\\", string.Empty));
        }
        private static Type[] GetAllNestedTypes(Type type, List<Type> allNestedTypesForProperty)
        {
            allNestedTypesForProperty ??= new List<Type>();
            if (!allNestedTypesForProperty.Contains(type))
            {
                allNestedTypesForProperty.Add(type);
            }

            foreach (var propertyInfo in type.GetProperties())
            {
                if (!_convertedTypes.ContainsKey(propertyInfo.PropertyType))
                {
                    if (propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GenericTypeArguments.Any())
                    {
                        foreach (var propertyType in propertyInfo.PropertyType.GenericTypeArguments)
                        {
                            if (!allNestedTypesForProperty.Contains(propertyType))
                            {
                                allNestedTypesForProperty.AddRange(GetAllNestedTypes(propertyType, allNestedTypesForProperty));
                            }
                        }
                    }
                    else if (!allNestedTypesForProperty.Contains(propertyInfo.PropertyType))
                    {
                        allNestedTypesForProperty.AddRange(GetAllNestedTypes(propertyInfo.PropertyType, allNestedTypesForProperty));
                    }
                }
            }
            var result = new[]
                         {
                             type
                         }
                         .Concat(allNestedTypesForProperty)
                         .Where(a => !_convertedTypes.ContainsKey(a) && a.IsClass)
                         .Distinct()
                         .ToArray();

            return result;
        }
        private static Type GetArrayOrEnumerableType(Type type)
        {
            if (type.IsArray)
            {
                return type.GetElementType() ?? typeof(Type);
            }
            if (type.IsConstructedGenericType)
            {
                var typeArgument = type.GenericTypeArguments.First();

                if (typeof(IEnumerable<>).MakeGenericType(typeArgument).IsAssignableFrom(type))
                {
                    return typeArgument;
                }
            }
            return null;
        }
        private static Type GetNullableType(Type type)
        {
            if (type.IsConstructedGenericType)
            {
                var typeArgument = type.GenericTypeArguments.First();

                if (typeArgument.IsValueType && typeof(Nullable<>).MakeGenericType(typeArgument).IsAssignableFrom(type))
                {
                    return typeArgument;
                }
            }
            return null;
        }
        private static string ConvertType(Type typeToUse)
        {
            if (_convertedTypes.TryGetValue(typeToUse, out var convertType))
            {
                return convertType;
            }

            if (typeToUse.IsConstructedGenericType && typeToUse.GetGenericTypeDefinition() == typeof(IDictionary<,>))
            {
                var keyType = typeToUse.GenericTypeArguments[0];
                var valueType = typeToUse.GenericTypeArguments[1];
                return $"{{ [key: {ConvertType(keyType)}]: {ConvertType(valueType)} }}";
            }
            return typeToUse.Name;
        }
        private static void OutputJsClass(ICollection<string> lines, Type type, string fileName)
        {
            var constructorTemp = new List<string>();

            lines.Add("/**");

            var classSummary = type.GetSummary().Trim();
            if (!string.IsNullOrWhiteSpace(classSummary))
            {
                lines.Add($" * {classSummary}");
            }

            lines.Add(" * @class");
            lines.Add(" */");
            lines.Add($"class {fileName} {{");

            var publicProperties = type.GetProperties().Where(p => p.GetMethod?.IsPublic ?? false);
            var propertiesDictionary = new Dictionary<string, string>();
            foreach (var property in publicProperties)
            {
                var propType = property.PropertyType;
                var arrayType = GetArrayOrEnumerableType(propType);
                var nullableType = GetNullableType(propType);
                var typeToUse = nullableType ?? arrayType ?? propType;

                var convertedType = ConvertType(typeToUse);

                // 遇到 Func 會出錯， 先做排除字元
                if (convertedType.Contains("Func"))
                {
                    convertedType = convertedType.Replace("`", string.Empty);
                }

                var suffix = "";
                suffix = arrayType != null ? "[]" : suffix;
                suffix = nullableType != null ? "|null" : suffix;

                if (suffix == "[]")
                {
                    constructorTemp.Add($"        this.{property.Name} = {suffix};");
                    constructorTemp.Add($"        this.ModelError__{property.Name} = '';");
                }
                else
                {
                    if (_typeInitVal.ContainsKey(convertedType))
                    {
                        constructorTemp.Add(_typeInitVal.TryGetValue(convertedType, out var convertedTypeValue) ? $"        this.{property.Name} = {convertedTypeValue};" : $"        this.{property.Name};");
                        constructorTemp.Add($"        this.ModelError__{property.Name} = '';");
                    }
                    else if (convertedType.Contains("{ [key:"))
                    {
                        constructorTemp.Add($"        this.{property.Name} = new Object();");
                        constructorTemp.Add($"        this.ModelError__{property.Name} = '';");
                    }
                    else
                    {
                        constructorTemp.Add($"        this.{property.Name} = new {convertedType}();");
                        constructorTemp.Add($"        this.ModelError__{property.Name} = '';");
                    }
                }

                lines.Add("    /**");

                var propertySummary = property.GetSummary().Trim();
                if (!string.IsNullOrWhiteSpace(propertySummary))
                {
                    lines.Add($"     * {propertySummary}");
                }

                lines.Add($"     * @type {{{convertedType}{suffix}}}");
                lines.Add("     */");
                lines.Add($"    {property.Name};");

                propertiesDictionary.Add(property.Name, $"{convertedType}{suffix}");
            }

            lines.Add("");
            lines.Add("    /** 建構式 */");
            lines.Add("    constructor () {");
            lines.AddRange(constructorTemp);
            lines.Add("    }");

            lines.Add("");
            lines.Add("    /** ");
            lines.Add("     * 取得屬性的型別");
            lines.Add("     * @param {string} prop 屬性名稱");
            lines.Add("     * @returns {string|Map<string, string>} 屬性的型別");
            lines.Add("     */");
            lines.Add("    propertyDictionary (prop = undefined) {");
            lines.Add("        const propMap = new Map([");
            var maxKeyLength = propertiesDictionary.Keys.Max(x => x.Length);
            foreach (var dict in propertiesDictionary)
            {
                lines.Add($"            ['{dict.Key}',{alignByLength(maxKeyLength, dict.Key)}'{dict.Value}'],");
            }
            lines.Add("        ]);");
            lines.Add("        if (prop == undefined) {");
            lines.Add("            return propMap;");
            lines.Add("        }");
            lines.Add("        return propMap.get(prop);");
            lines.Add("    }");

            lines.Add("}");
        }

        private static string alignByLength(int maxKeyLength, string key)
        {
            var padSpaceNum = maxKeyLength - key.Length;
            return new string(' ', padSpaceNum + 1);
        }
        
        private static void OutputJsEnum(ICollection<string> lines, Type type, string fileName)
        {
            var enumDictionary = ModelTool.ParseToDictionary(type);

            lines.Add("/**");

            var classSummary = type.GetSummary().Trim();
            if (!string.IsNullOrWhiteSpace(classSummary))
            {
                lines.Add($" * {classSummary}");
            }

            lines.Add(" * @enum");
            lines.Add(" */");

            lines.Add($"const {fileName} = Object.freeze({{");
            foreach (var key in enumDictionary.Keys)
            {
                lines.Add($"    {enumDictionary[key].Item1}: Object.freeze({{");
                var desc = enumDictionary[key].Item2;
                // 如果沒有註解 就抓 displayName、display(name)、summary
                if (string.IsNullOrWhiteSpace(desc))
                {
                    desc = type.GetField(enumDictionary[key].Item1)?.GetCustomAttributes<DisplayNameAttribute>().FirstOrDefault()?.DisplayName.Trim() ?? string.Empty;
                    if (string.IsNullOrWhiteSpace(desc))
                    {
                        desc = type.GetField(enumDictionary[key].Item1)?.GetCustomAttributes<DisplayAttribute>().FirstOrDefault()?.Name?.Trim() ?? string.Empty;
                        if (string.IsNullOrWhiteSpace(desc))
                        {
                            desc = type.GetField(enumDictionary[key].Item1)?.GetSummary().Trim() ?? string.Empty;
                        }
                    }
                }
                lines.Add($"        Name: `{desc}`,");
                lines.Add($"        Value: {key},");
                lines.Add("    }),");
            }

            lines.Add("})");
        }
        private static (string FilePath, string[] Lines) ConvertCsToJs(Type type)
        {
            var fileNameInfo = TransferTypeToFileName(type);
            var lines = new List<string>
            {
                "/* ===== 此檔案是自動產生 ===== */",
                "/* ===== 請勿手動變更修改 ===== */",
                ""
            };

            if (type.IsClass || type.IsInterface)
            {
                OutputJsClass(lines, type, fileNameInfo.FileName);
            }
            else if (type.IsEnum)
            {
                OutputJsEnum(lines, type, fileNameInfo.FileName);
            }
            else
            {
                throw new InvalidOperationException();
            }

            lines.Add("export {");
            lines.Add($"    {fileNameInfo.FileName}");
            lines.Add("}");

            return (fileNameInfo.FilePath, lines.ToArray());
        }
        private static IEnumerable<string> ConvertEnum2Js(Type type)
        {
            var lines = new List<string>();
            if (type.IsEnum)
            {
                OutputJsEnum(lines, type, type.Name);
            }
            else
            {
                throw new InvalidOperationException();
            }
            lines.Add("");

            lines.Add("export {");
            lines.Add($"    {type.Name}");
            lines.Add("}");
            lines.Add("");

            return lines;
        }





        private static (string Name, string[] Lines) ConvertCs2Ts(Type type)
        {
            var fileNameInfo = TransferTypeToFileName(type);
            var filename = $@"{(type.Namespace?.Replace(".", @"\") ?? "EmptyNameSpace")}\{fileNameInfo.FileName}.d.ts";

            var types = GetAllNestedTypes(type, []);

            var lines = new List<string>();

            foreach (var t in types)
            {
                lines.Add($"");

                if (t.IsClass || t.IsInterface)
                {
                    OutputTsClass(lines, t);
                }
                else if (t.IsEnum)
                {
                    ConvertTsEnum(lines, t);
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

            return (filename, lines.ToArray());
        }
        private static void OutputTsClass(ICollection<string> lines, Type type)
        {
            lines.Add($"/**");

            var classSummary = type.GetSummary().Trim();
            if (!string.IsNullOrWhiteSpace(classSummary))
            {
                lines.Add($" * {classSummary}");
            }

            lines.Add(" * @class");
            lines.Add(" */");
            lines.Add($"interface {type.Name} {{");

            var publicProperties = type.GetProperties().Where(p => p.GetMethod?.IsPublic ?? false);
            foreach (var property in publicProperties)
            {
                var propType = property.PropertyType;
                var arrayType = GetArrayOrEnumerableType(propType);
                var nullableType = GetNullableType(propType);

                var typeToUse = nullableType ?? arrayType ?? propType;


                var convertedType = ConvertType(typeToUse);

                var suffix = "";
                suffix = arrayType != null ? "[]" : suffix;
                suffix = nullableType != null ? "|null" : suffix;

                lines.Add($"    /**");

                var propertySummary = property.GetSummary().Trim();
                if (!string.IsNullOrWhiteSpace(propertySummary))
                {
                    lines.Add($"     * {propertySummary}");
                }

                lines.Add($"     * @type {{{convertedType}{suffix}}}");
                lines.Add("     */");
                lines.Add($"    {property.Name}: {convertedType}{suffix};");
            }

            lines.Add($"}}");
        }
        private static void ConvertTsEnum(ICollection<string> lines, Type type)
        {
            var enumValues = type.GetEnumValues().Cast<int>().ToArray();
            var enumNames = type.GetEnumNames();

            lines.Add($"export enum {type.Name} {{");

            for (var i = 0; i < enumValues.Length; i++)
            {
                lines.Add($"  {enumNames[i]} = {enumValues[i]},");
            }

            lines.Add($"}}");
        }

        /// <summary>
        /// AddRange
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="destination"></param>
        /// <param name="source"></param>
        public static void AddRange<T>(this ICollection<T> destination,
                               IEnumerable<T> source)
        {
            if (destination is List<T> list)
            {
                list.AddRange(source);
            }
            else
            {
                foreach (T item in source)
                {
                    destination.Add(item);
                }
            }
        }
    }

    /// <summary>
    /// Utility class to provide documentation for various types where available with the assembly
    /// </summary>
    public static class DocumentationExtensions
    {
        /// <summary> Provides the documentation comments for a specific method </summary>
        /// <param name="methodInfo">The MethodInfo (reflection data ) of the member to find documentation for</param>
        /// <returns>The XML fragment describing the method</returns>
        public static XmlElement GetDocumentation(this MethodInfo methodInfo)
        {
            // Calculate the parameter string as this is in the member name in the XML
            var parametersString = "";
            foreach (var parameterInfo in methodInfo.GetParameters())
            {
                if (parametersString.Length > 0)
                {
                    parametersString += ",";
                }
                parametersString += parameterInfo.ParameterType.FullName;
            }

            //AL: 15.04.2008 ==> BUG-FIX remove “()” if parametersString is empty
            return XmlFromName(methodInfo.DeclaringType ?? typeof(Type), 'M', parametersString.Length > 0 ? $"{methodInfo.Name}({parametersString})" : methodInfo.Name);
        }

        /// <summary> Provides the documentation comments for a specific member </summary>
        /// <param name="memberInfo">The MemberInfo (reflection data) or the member to find documentation for</param>
        /// <returns>The XML fragment describing the member</returns>
        private static XmlElement GetDocumentation(this MemberInfo memberInfo) =>
            // First character [0] of member type is prefix character in the name in the XML
            XmlFromName(memberInfo.DeclaringType ?? typeof(Type), memberInfo.MemberType.ToString()[0], memberInfo.Name);

        /// <summary> Returns the Xml documentation summary comment for this member </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public static string GetSummary(this MemberInfo memberInfo)
        {
            try
            {
                var element = memberInfo.GetDocumentation();
                var summaryElm = element?.SelectSingleNode("summary");
                return summaryElm == null ? "" : summaryElm.InnerText.Trim();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary> Provides the documentation comments for a specific type </summary>
        /// <param name="type">Type to find the documentation for</param>
        /// <returns>The XML fragment that describes the type</returns>
        private static XmlElement GetDocumentation(this Type type) =>
            // Prefix in type names is T
            XmlFromName(type, 'T', "");

        /// <summary> Gets the summary portion of a type's documentation or returns an empty string if not available </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetSummary(this Type type)
        {
            try
            {
                var element = type.GetDocumentation();
                var summaryElm = element?.SelectSingleNode("summary");
                return summaryElm == null ? "" : summaryElm.InnerText.Trim();
            }
            catch (Exception)
            {
                return string.Empty;
            }

        }

        /// <summary> Obtains the XML Element that describes a reflection element by searching the members for a member that has a name that describes the element. </summary>
        /// <param name="type">The type or parent type, used to fetch the assembly</param>
        /// <param name="prefix">The prefix as seen in the name attribute in the documentation XML</param>
        /// <param name="name">Where relevant, the full name qualifier for the element</param>
        /// <returns>The member that has a name that describes the specified reflection element</returns>
        private static XmlElement XmlFromName(this Type type, char prefix, string name)
        {
            var fullName = string.IsNullOrWhiteSpace(name) ? $"{prefix}:{type.FullName}" : $"{prefix}:{type.FullName}.{name}";

            var xmlDocument = XmlFromAssembly(type.Assembly);

            var matchedElement = xmlDocument["doc"]?["members"]?.SelectSingleNode("member[@name='" + fullName + "']") as XmlElement;

            if (matchedElement == null)
            {
                fullName = fullName.Replace("+", ".");
                xmlDocument = XmlFromAssembly(type.Assembly);
                matchedElement = xmlDocument["doc"]?["members"]?.SelectSingleNode("member[@name='" + fullName + "']") as XmlElement;
            }

            return matchedElement;
        }

        /// <summary> A cache used to remember Xml documentation for assemblies </summary>
        private static readonly Dictionary<Assembly, XmlDocument> _cache = new Dictionary<Assembly, XmlDocument>();

        /// <summary> A cache used to store failure exceptions for assembly lookups </summary>
        private static readonly Dictionary<Assembly, Exception> _failCache = new Dictionary<Assembly, Exception>();

        /// <summary> Obtains the documentation file for the specified assembly </summary>
        /// <param name="assembly">The assembly to find the XML document for</param>
        /// <returns>The XML document</returns>
        /// <remarks>This version uses a cache to preserve the assemblies, so that
        /// the XML file is not loaded and parsed on every single lookup</remarks>
        private static XmlDocument XmlFromAssembly(this Assembly assembly)
        {
            if (_failCache.TryGetValue(assembly, out var failCache))
            {
                throw failCache;
            }

            try
            {

                if (!_cache.ContainsKey(assembly))
                {
                    // load the document into the cache
                    _cache[assembly] = XmlFromAssemblyNonCached(assembly);
                }

                return _cache[assembly];
            }
            catch (Exception exception)
            {
                _failCache[assembly] = exception;
                throw;
            }
        }

        /// <summary> Loads and parses the documentation file for the specified assembly </summary>
        /// <param name="assembly">The assembly to find the XML document for</param>
        /// <returns>The XML document</returns>
        private static XmlDocument XmlFromAssemblyNonCached(Assembly assembly)
        {
            var assemblyFilename = assembly.Location;

            if (!string.IsNullOrWhiteSpace(assemblyFilename))
            {
                StreamReader streamReader;

                try
                {
                    streamReader = new StreamReader(Path.ChangeExtension(assemblyFilename, ".xml"));
                }
                catch (FileNotFoundException exception)
                {
                    throw new Exception("XML documentation not present (make sure it is turned on in project properties when building)", exception);
                }

                var xmlDocument = new XmlDocument();
                xmlDocument.Load(streamReader);
                return xmlDocument;
            }

            throw new Exception("Could not ascertain assembly filename", null);
        }
    }
}