using ElectronApp.Controllers;
using ElectronNET.API;
using Microsoft.AspNetCore.Mvc;

namespace ElectronApp.Areas.Updater.Controllers
{
    /// <summary>
    /// 應用程式更新
    /// </summary>
    [Area("Updater")]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="logger">The logger</param>
        public HomeController(IConfiguration configuration,
                                 ILogger<HomeController> logger)
            : base(configuration)
        {
            _logger = logger;
        }

        /// <summary>
        /// 更新功能首頁
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            if (HybridSupport.IsElectronActive)
            {

                Electron.AutoUpdater.OnError += (message) =>
                {
                    var mainWindow = Electron.WindowManager.BrowserWindows.First();
                    Electron.IpcMain.Send(mainWindow, "auto-update-reply", "檢查更新時發生錯誤：" + message);
                };

                Electron.AutoUpdater.OnCheckingForUpdate += () =>
                {
                    var mainWindow = Electron.WindowManager.BrowserWindows.First();
                    Electron.IpcMain.Send(mainWindow, "auto-update-reply", "查看是否有更新...");
                };

                Electron.AutoUpdater.OnUpdateNotAvailable += (info) =>
                {
                    var mainWindow = Electron.WindowManager.BrowserWindows.First();
                    Electron.IpcMain.Send(mainWindow, "auto-update-reply", "目前沒有更新");
                };

                Electron.AutoUpdater.OnUpdateAvailable += (info) =>
                {
                    var mainWindow = Electron.WindowManager.BrowserWindows.First();
                    Electron.IpcMain.Send(mainWindow, "auto-update-reply", "目前最新版本為：" + info.Version);
                };

                Electron.AutoUpdater.OnDownloadProgress += (info) =>
                {
                    var message1 = "Download speed: " + info.BytesPerSecond + "\n<br/>";
                    var message2 = "Downloaded " + info.Percent + "%" + "\n<br/>";
                    var message3 = $"({info.Transferred}/{info.Total})" + "\n<br/>";
                    var message4 = "Progress: " + info.Progress + "\n<br/>";
                    var information = message1 + message2 + message3 + message4;

                    var mainWindow = Electron.WindowManager.BrowserWindows.First();
                    Electron.IpcMain.Send(mainWindow, "auto-update-reply", information);
                };
                Electron.AutoUpdater.OnUpdateDownloaded += (info) =>
                {
                    var mainWindow = Electron.WindowManager.BrowserWindows.First();
                    Electron.IpcMain.Send(mainWindow, "auto-update-reply", info.Version + "更新完成");
                };

                Electron.IpcMain.On("auto-update", async (args) =>
                {
                    var mainWindow = Electron.WindowManager.BrowserWindows.First();

                    try
                    {
                        var currentVersion = await Electron.App.GetVersionAsync();
                        var updateCheckResult = await Electron.AutoUpdater.CheckForUpdatesAndNotifyAsync();
                        var availableVersion = updateCheckResult.UpdateInfo.Version;
                        string information = $"目前版本：{currentVersion} --> 最新版本：{availableVersion}";

                        Electron.IpcMain.Send(mainWindow, "auto-update-reply", information);
                    }
                    catch (Exception ex)
                    {
                        Electron.IpcMain.Send(mainWindow, "auto-update-reply", "更新發生錯誤：" + ex.Message);
                    }
                });
            }

            return View();
        }
    }
}
