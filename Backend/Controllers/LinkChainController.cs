using ElectronApp.Tools;
using Microsoft.AspNetCore.Mvc;

namespace ElectronApp.Controllers
{
    /// <summary>
    /// LinkChain管理專用功能
    /// </summary>
    public class LinkChainController : BaseController
    {
        private readonly ILogger<LinkChainController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        /// <param name="webHostEnvironment"></param>
        public LinkChainController(
            ILogger<LinkChainController> logger,
            IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment) 
            : base(configuration)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// C#類別轉換Js
        /// </summary>
        /// <returns></returns>
        public IActionResult ClassTransfer()
        {
            var resultDictionary = TypeTransferExtension.Generate(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", "js"));
            return Content(string.Join("\r\n", resultDictionary.Select(x => $"實體路徑: {x.Key}, 相對路徑: {x.Value}")));
        }
    }
}
