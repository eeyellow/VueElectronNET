using Newtonsoft.Json;

namespace ElectronApp.Tools
{
    /// <summary>
    /// ElectronManifest工具
    /// </summary>
    public static class ElectronManifestTool
    {
        /// <summary>
        /// 讀取Manifest檔案並將其反序列化為Manifest物件
        /// </summary>
        /// <returns>反序列化後的Manifest物件</returns>
        public static Manifest ReadManifest(string ManifestPath)
        {
            string json = File.ReadAllText(ManifestPath);
            return JsonConvert.DeserializeObject<Manifest>(json);
        }
    }

    /// <summary>
    /// Electron應用程式的Manifest類別
    /// </summary>
    public class Manifest
    {
        /// <summary> Executable </summary>
        public string Executable { get; set; }
        /// <summary> Splashscreen </summary>
        public Splashscreen Splashscreen { get; set; }
        /// <summary> Name </summary>
        public string Name { get; set; }
        /// <summary> Author </summary>
        public string Author { get; set; }
        /// <summary> SingleInstance </summary>
        public bool SingleInstance { get; set; }
        /// <summary> Environment </summary>
        public string Environment { get; set; }
        /// <summary> Build </summary>
        public Build Build { get; set; }
    }

    /// <summary>
    /// Splashscreen類別，用於定義啟動畫面相關資訊
    /// </summary>
    public class Splashscreen
    {
        /// <summary> ImageFile </summary>
        public string ImageFile { get; set; }
    }

    /// <summary>
    /// Build類別，用於定義應用程式建置相關資訊
    /// </summary>
    public class Build
    {
        /// <summary> AppId </summary>
        public string AppId { get; set; }
        /// <summary> ProductName </summary>
        public string ProductName { get; set; }
        /// <summary> Copyright </summary>
        public string Copyright { get; set; }
        /// <summary> BuildVersion </summary>
        public string BuildVersion { get; set; }
        /// <summary> Compression </summary>
        public string Compression { get; set; }
        /// <summary> Win </summary>
        public Win Win { get; set; }
        /// <summary> Directories </summary>
        public Directories Directories { get; set; }
        /// <summary> ExtraResources </summary>
        public ExtraResource[] ExtraResources { get; set; }
    }

    /// <summary>
    /// Win類別，用於定義Windows相關資訊
    /// </summary>
    public class Win
    {
        /// <summary> Icon </summary>
        public string Icon { get; set; }
        /// <summary> Publish </summary>
        public Publish[] Publish { get; set; }
    }

    /// <summary>
    /// Directories類別，用於定義目錄相關資訊
    /// </summary>
    public class Directories
    {
        /// <summary> Output </summary>
        public string Output { get; set; }
    }

    /// <summary>
    /// ExtraResource類別，用於定義額外資源相關資訊
    /// </summary>
    public class ExtraResource
    {
        /// <summary> From </summary>
        public string From { get; set; }
        /// <summary> To </summary>
        public string To { get; set; }
        /// <summary> Filter </summary>
        public string[] Filter { get; set; }
    }

    /// <summary>
    /// Publish類別，用於定義發佈相關資訊
    /// </summary>
    public class Publish
    {
        /// <summary> Provider </summary>
        public string Provider { get; set; }
        /// <summary> Url </summary>
        public string Url { get; set; }
        /// <summary> Channel </summary>
        public string Channel { get; set; }
    }
}
