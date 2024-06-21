using Microsoft.AspNetCore.Mvc;

namespace ElectronApp.Controllers
{
    /// <summary>
    /// 單頁式應用控制器
    /// </summary>
    public class AppController : BaseController
    {
        private readonly ILogger<AppController> _logger;
        private readonly IWebHostEnvironment _env;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public AppController(ILogger<AppController> logger,
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
            // 讀取 VueApp/dist/index.html 的內容到stringbuilder
            var indexHtml = System.IO.File.ReadAllText(
                Path.Combine(_env.WebRootPath, "VueApp/index.html")
            );

            return Content(indexHtml, "text/html");
        }
    }
}
