namespace ElectronApp.Tools
{
    /// <summary> 字串 擴充方法 </summary>
    public static class StringExtension
    {
        /// <summary> 字串轉數字清單 </summary>
        public static List<int> SplitToInt(this string x, string splitChar = ",") => (x ?? string.Empty).Split(splitChar).Where(y => !string.IsNullOrWhiteSpace(y)).Select(int.Parse).ToList();

        /// <summary> 去掉 Controller 結尾 </summary>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">不可為空</exception>
        /// <exception cref="ArgumentException">請使用Controller類別</exception>
        public static string GetControllerName(this string controllerName)
        {
            const string controllerText = "Controller";

            if (string.IsNullOrWhiteSpace(controllerName))
            {
                throw new ArgumentNullException(nameof(controllerName), "不可為空");
            }

            if (controllerName.EndsWith(controllerText))
            {
                return controllerName.Replace(controllerText, string.Empty);
            }

            throw new ArgumentException($"請使用{controllerText}類別");
        }
        /// <summary>
        /// 移除字串尾端Controller
        /// </summary>
        /// <param name="controllerString"></param>
        /// <returns></returns>
        public static string ReplaceControllerEmpty(this string controllerString) => controllerString.Replace("Controller", string.Empty);
        /// <summary>
        /// 移除字串尾端Async
        /// </summary>
        /// <param name="AsyncString"></param>
        /// <returns></returns>
        public static string ReplaceAsyncEmpty(this string AsyncString) => AsyncString.Replace("Async", string.Empty);
    }
}