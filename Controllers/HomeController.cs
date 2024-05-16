using ElectronApp.Tools;
using ElectronApp.ViewModels;
using ElectronNET.API;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ElectronApp.Controllers
{
    /// <summary>
    /// 首頁控制器
    /// </summary>
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public HomeController(ILogger<HomeController> logger,
                              IConfiguration configuration,
                              IWebHostEnvironment env) 
            : base(configuration)
        {
            _logger = logger;
            _env = env;
        }

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            if (HybridSupport.IsElectronActive)
            {
                ViewData["currentVersion"] = ElectronManifestTool.ReadManifest("electron.manifest.json").Build.BuildVersion;
                ViewData["areaList"] = string.Join('，', RouterTool.GetAreas());
                ViewData["motherboardSN"] = HardwareInfoTool.GetMotherboardSN();
            }

            return View();
        }

        /// <summary>
        /// Privacy
        /// </summary>
        /// <returns></returns>
        public IActionResult Privacy() => View();

        /// <summary>
        /// Error
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
