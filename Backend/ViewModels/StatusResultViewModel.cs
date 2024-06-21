namespace ElectronApp.ViewModels
{
    /// <summary>
    /// Http回應狀態碼與訊息
    /// </summary>
    public class StatusResultViewModel
    {
        /// <summary>
        /// Http狀態碼
        /// </summary>
        public int StatusCode { get; set; } = 200;
        /// <summary>
        /// 回應訊息
        /// </summary>
        public List<string> Message { get; set; } = new List<string>();
        /// <summary>
        /// 回應資料
        /// </summary>
        public object Data { get; set; } = new object();
    }
}
