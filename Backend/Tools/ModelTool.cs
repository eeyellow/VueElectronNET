using Newtonsoft.Json;
using System.ComponentModel;
using System.Reflection;

namespace ElectronApp.Tools
{
    /// <summary>
    /// 提供模型之間的映射和轉換功能的工具類別。
    /// </summary>
    public static class ModelTool
    {
        /// <summary> 將兩個模型間名稱相同的欄位做值的映射 </summary>
        /// <typeparam name="S"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">來源模型</param>
        /// <param name="target">目標模型</param>
        /// <param name="filterFieldNames">不更新的欄位名稱</param>
        /// <param name="targetFieldNames">要更新的欄位名稱</param>
        public static void Mapping<S, T>(S source,
                                         T target,
                                         List<string> filterFieldNames = null,
                                         List<string> targetFieldNames = null)
            where S : class
            where T : class
        {
            var hasFilterFieldNames = filterFieldNames?.Any() ?? false;
            var hasTargetFieldNames = targetFieldNames?.Any() ?? false;

            var sourceType = source.GetType();
            var targetType = target.GetType();

            foreach (var sourceProperty in sourceType.GetProperties())
            {
                if (sourceProperty.CanRead)
                {
                    var propertyName = sourceProperty.Name;

                    // 指定更新的欄位
                    if (hasTargetFieldNames && targetFieldNames != null && !targetFieldNames.Contains(propertyName))
                    {
                        continue;
                    }

                    // 跳過不更新的欄位
                    if (hasFilterFieldNames && filterFieldNames != null && filterFieldNames.Contains(propertyName))
                    {
                        continue;
                    }

                    // 跳過Target不存在欄位，或無法寫入Target欄位
                    var targetProperty = targetType.GetProperty(propertyName);
                    if (targetProperty == null || !targetProperty.CanWrite)
                    {
                        continue;
                    }

                    // 跳過不相同的型態
                    if (sourceProperty.PropertyType != targetProperty.PropertyType)
                    {

                    }

                    var sourceValue = sourceProperty.GetValue(source, null);
                    targetProperty.SetValue(target, sourceValue, null);
                }
            }
        }

        /// <summary> 將兩個模型間名稱相同的欄位做值的映射 </summary>
        /// <typeparam name="S"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">來源模型</param>
        /// <param name="target">目標模型</param>
        /// <param name="ofilterFieldNames">不更新的欄位名稱</param>
        /// <param name="otargetFieldNames">要更新的欄位名稱</param>
        public static void MappingIEntity<S, T>(S source, T target,
                                         List<string> ofilterFieldNames = null,
                                         List<string> otargetFieldNames = null)
            where S : class
            where T : class
        {
            var filterFieldNames = new List<string>() { "ID", "CreateDateTime", "CreateUserID", "LastMaintainDate", "LastMaintainUserID" };
            var targetFieldNames = new List<string>();
            if (ofilterFieldNames != null)
            { filterFieldNames.AddRange(ofilterFieldNames); }
            if (otargetFieldNames != null)
            { targetFieldNames.AddRange(otargetFieldNames); }

            // 以下與Mapping相同
            var hasFilterFieldNames = filterFieldNames != null && filterFieldNames.Any();
            var hasTargetFieldNames = targetFieldNames != null && targetFieldNames.Any();

            var sourceType = source.GetType();
            var targetType = target.GetType();

            foreach (var sourceProperty in sourceType.GetProperties())
            {
                if (sourceProperty.CanRead)
                {
                    var propertyName = sourceProperty.Name;

                    // 指定更新的欄位
                    if (hasTargetFieldNames && targetFieldNames != null && !targetFieldNames.Contains(propertyName))
                    {
                        continue;
                    }

                    // 跳過不更新的欄位
                    if (hasFilterFieldNames && filterFieldNames != null && filterFieldNames.Contains(propertyName))
                    {
                        continue;
                    }

                    // 跳過Target不存在欄位，或無法寫入Target欄位
                    var targetProperty = targetType.GetProperty(propertyName);
                    if (targetProperty == null || !targetProperty.CanWrite)
                    {
                        continue;
                    }

                    // 跳過不相同的型態
                    if (sourceProperty.PropertyType != targetProperty.PropertyType)
                    {

                    }

                    var sourceValue = sourceProperty.GetValue(source, null);
                    targetProperty.SetValue(target, sourceValue, null);
                }
            }
        }

        /// <summary> 將兩個模型間名稱相同的欄位做值的映射 </summary>
        /// <typeparam name="S"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">來源模型</param>
        /// <param name="target">目標模型</param>
        /// <param name="filterFieldNames">不更新的欄位名稱</param>
        /// <param name="targetFieldNames">要更新的欄位名稱</param>
        public static void MappingWithoutNull<S, T>(S source,
                                                    T target,
                                                    List<string> filterFieldNames = null,
                                                    List<string> targetFieldNames = null)
            where S : class
            where T : class
        {

            var sourceType = source.GetType();
            var targetType = target.GetType();

            if (sourceType.GetInterface("IEntity") != null)
            {
                if (filterFieldNames == null)
                {
                    filterFieldNames = new List<string>();
                }
                filterFieldNames.AddRange(new List<string> { "CreateDateTime", "CreateUserID", "LastMaintainDate", "LastMaintainUserID" });
            }

            var hasFilterFieldNames = filterFieldNames != null && filterFieldNames.Any();
            var hasTargetFieldNames = targetFieldNames != null && targetFieldNames.Any();

            foreach (var sourceProperty in sourceType.GetProperties())
            {
                if (sourceProperty.CanRead)
                {
                    var propertyName = sourceProperty.Name;

                    // 指定更新的欄位
                    if (hasTargetFieldNames && targetFieldNames != null && !targetFieldNames.Contains(propertyName))
                    {
                        continue;
                    }

                    // 跳過不更新的欄位
                    if (hasFilterFieldNames && filterFieldNames != null && filterFieldNames.Contains(propertyName))
                    {
                        continue;
                    }

                    // 跳過Target不存在欄位，或無法寫入Target欄位
                    var targetProperty = targetType.GetProperty(propertyName);
                    if (targetProperty == null || !targetProperty.CanWrite)
                    {
                        continue;
                    }

                    // 跳過不相同的型態
                    if (sourceProperty.PropertyType != targetProperty.PropertyType)
                    {

                    }

                    var sourceValue = sourceProperty.GetValue(source, null);
                    //Null就跳過
                    if (sourceValue == null)
                    {
                        continue;
                    }
                    targetProperty.SetValue(target, sourceValue, null);
                }
            }
        }

        /// <summary> 將兩個模型間名稱相同的欄位做值的映射 </summary>
        /// <typeparam name="S"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">來源模型</param>
        /// <param name="target">目標模型</param>
        /// <param name="filterFieldNames">不更新的欄位名稱</param>
        /// <param name="targetFieldNames">要更新的欄位名稱</param>
        public static T MappingAndReturn<S, T>(S source,
                                               T target,
                                               List<string> filterFieldNames = null,
                                               List<string> targetFieldNames = null)
            where S : class
            where T : class
        {
            var hasFilterFieldNames = filterFieldNames != null && filterFieldNames.Any();
            var hasTargetFieldNames = targetFieldNames != null && targetFieldNames.Any();

            var sourceType = source.GetType();
            var targetType = target.GetType();

            foreach (var sourceProperty in sourceType.GetProperties())
            {
                if (sourceProperty.CanRead)
                {
                    var propertyName = sourceProperty.Name;

                    // 指定更新的欄位
                    if (hasTargetFieldNames && targetFieldNames != null && !targetFieldNames.Contains(propertyName))
                    {
                        continue;
                    }

                    // 跳過不更新的欄位
                    if (hasFilterFieldNames && filterFieldNames != null && filterFieldNames.Contains(propertyName))
                    {
                        continue;
                    }

                    // 跳過Target不存在欄位，或無法寫入Target欄位
                    var targetProperty = targetType.GetProperty(propertyName);
                    if (targetProperty == null || !targetProperty.CanWrite)
                    {
                        continue;
                    }

                    // 跳過不相同的型態
                    if (sourceProperty.PropertyType != targetProperty.PropertyType)
                    {

                    }

                    var sourceValue = sourceProperty.GetValue(source, null);
                    targetProperty.SetValue(target, sourceValue, null);
                }
            }

            return target;
        }

        /// <summary>
        /// 檢查物件是否每個屬性都有值
        /// </summary>
        /// <param name="myObject"></param>
        /// <returns></returns>
        public static bool IsAnyNullOrEmpty(object myObject)
        {
            foreach (PropertyInfo pi in myObject.GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(string))
                {
                    string value = pi.GetValue(myObject)?.ToString() ?? string.Empty;
                    if (string.IsNullOrEmpty(value))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// json 轉物件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T ToObject<T>(this string source) => JsonConvert.DeserializeObject<T>(source);
        /// <summary>
        /// json 轉物件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToJson<T>(this T source)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.None,
                NullValueHandling = NullValueHandling.Ignore
            };
            return JsonConvert.SerializeObject(source, settings).ToString();
        }

        /// <summary> 取得自訂 Enum 描述 (多語系要出去後再處理) </summary>
        /// <param name="enumValue">Enum物件</param>
        /// <returns> Enum描述 </returns>
        public static string GetDescription(object enumValue)
        {
            var fi = enumValue.GetType().GetField(enumValue.ToString() ?? string.Empty);
            if (fi != null)
            {
                var attrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs.Length > 0)
                {
                    var description = ((DescriptionAttribute)attrs[0]).Description;
                    return description;
                }
            }

            return string.Empty;
        }

        /// <summary> Enum 型別解析成 資料字典 </summary>
        /// <param name="type">Enum型態</param>
        /// <returns>Enum資料字典(value => item1: enum text; item2: enum description)</returns>
        public static Dictionary<int, Tuple<string, string>> ParseToDictionary(Type type)
        {
            var result = new Dictionary<int, Tuple<string, string>>();

            foreach (var foo in Enum.GetValues(type))
            {
                var key = (int)foo;
                var item1 = foo.ToString();
                var item2 = GetDescription(foo);

                result.Add(key, new Tuple<string, string>(item1 ?? string.Empty, item2));
            }
            return result;
        }
    }
}
