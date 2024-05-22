using System.Globalization;
using System.Reflection;

namespace ElectronApp.Tools
{
    /// <summary>
    /// Enum 相關工具
    /// </summary>
    public static class EnumMapTool
    {
        /// <summary>
        ///  取得自訂 Enum 描述
        /// </summary>
        /// <param name="enumValue">目標物件</param>
        /// <returns>指定Enum物件描述值</returns>
        public static string GetDescription(object enumValue)
        {
            FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());

            if (null != fi)
            {
                var localizedDescription = GetLocalizedDescription(fi, null);
                if (localizedDescription != null) return localizedDescription;

                var description = GetDescription(fi);
                if (description != null) return description;
            }

            return string.Empty;
        }

        /// <summary>
        ///  取得 (Enum名稱) + 自訂 Enum 描述
        /// </summary>
        /// <param name="enumValue">目標物件</param>
        /// <returns>指定Enum物件描述值</returns>
        public static string GetDescriptionWithEnumName(object enumValue)
        {
            FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());

            if (null != fi)
            {
                var localizedDescription = GetLocalizedDescription(fi, null);
                if (localizedDescription != null) return $"{fi.Name} {localizedDescription}";

                var description = GetDescription(fi);
                if (description != null) return $"{fi.Name} {description}";
            }

            return string.Empty;
        }

        /// <summary>
        /// 取得自訂 Enum 多語系描述
        /// </summary>
        /// <param name="enumValue">目標物件</param>
        /// <param name="culture">目標語言</param>
        /// <returns>指定Enum物件描述值</returns>
        public static string GetLocalizedDescription(object enumValue, CultureInfo culture)
        {
            FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());

            if (null != fi)
            {
                var localizedDescription = GetLocalizedDescription(fi, culture);
                if (localizedDescription != null) return localizedDescription;
            }

            return string.Empty;
        }

        /// <summary>
        /// 列舉轉為List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>列舉清單</returns>
        public static List<T> EnumToList<T>()
        {
            var result = new List<T>();
            var items = typeof(T).GetFields()
                .Where(field => field.IsStatic)
                .Select(field => field)
                .Select(fieldInfo => fieldInfo.Name)
                .Select(fieldName => (T)Enum.Parse(typeof(T), fieldName, true));

            foreach (T item in items)
            {
                result.Add(item);
            }
            return result;
        }

        /// <summary>
        /// 將二進位的數值轉為列舉清單
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binaryValue">二進位的數值</param>
        /// <returns>列舉清單</returns>
        public static List<T> EnumToListWithBinaryValue<T>(int binaryValue)
        {
            var list = EnumToList<T>();

            var selectIntList = Convert.ToString(binaryValue, 2).ToCharArray().Select(x => int.Parse(x.ToString())).ToList();
            var power = selectIntList.Count() - 1;
            var selectList = new List<int>();
            selectIntList.ForEach(x =>
            {
                selectList.Add(x * (int)(Math.Pow(2, power)));
                power--;
            });

            list = list.Where(x => selectList.Contains((int)Enum.Parse(typeof(T), x.ToString()))).ToList();
            return list;

        }

        /// <summary>
        /// 將型別轉換為指定Enum
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="strEnumValue"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TEnum ToEnum<TEnum>(this int strEnumValue, TEnum defaultValue)
        {
            if (!strEnumValue.isVlaid<TEnum>())
            {
                return defaultValue;
            }
            return (TEnum)Enum.Parse(typeof(TEnum), strEnumValue.ToString());
        }


        /// <summary>
        /// 判斷是否為合法EnumValue
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="strEnumValue"></param>
        /// <returns></returns>
        public static bool isVlaid<TEnum>(this int strEnumValue)
        {
            if (!Enum.IsDefined(typeof(TEnum), strEnumValue))
            {
                return false;
            }
            return true;
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

                result.Add(key, new Tuple<string, string>(item1, item2));
            }
            return result;
        }
    }
}
