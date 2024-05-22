namespace ElectronApp.Tools
{
    /// <summary> 環境變數 Manager </summary>
    public static class WebHostEnvironmentManager
    {
        /// <summary> wwwroot 實體路徑 </summary>
        public static string WebRootPath { get; set; }
        /// <summary> 網站 實體路徑 </summary>
        public static string ContentRootPath { get; set; }
        /// <summary> 環境名稱 </summary>
        public static string EnvironmentName { get; set; }
    }
}
