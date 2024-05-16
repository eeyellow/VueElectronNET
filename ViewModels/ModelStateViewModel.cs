namespace ElectronApp.ViewModels
{
    /// <summary>
    /// ModelState回應訊息
    /// </summary>
    public class ModelStateViewModel
    {
        /// <summary>
        /// Http狀態碼
        /// </summary>
        public int StatusCode { get; set; } = 200;
        /// <summary>
        /// Data
        /// </summary>
        public object Data { get; set; } = new object();
        /// <summary>
        /// ModelState
        /// </summary>
        public List<KeyValuePair<string, List<string>>> ModelState { get; set; }
    }
}
