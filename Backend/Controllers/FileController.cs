using ElectronApp.Enums;
using ElectronApp.Services;
using ElectronApp.Tools;
using Microsoft.AspNetCore.Mvc;

namespace ElectronApp.Controllers
{
    /// <summary>
    /// 檔案相關功能
    /// </summary>
    public class FileController : BaseController
    {
        private static readonly string dir = $"{WebHostEnvironmentManager.ContentRootPath}\\UploadFiles\\";
        private readonly ILogger<LinkChainController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileUploadService _fileUploadService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        /// <param name="webHostEnvironment"></param>
        /// <param name="fileUploadService"></param>
        public FileController(
            ILogger<LinkChainController> logger,
            IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment,
            IFileUploadService fileUploadService) 
            : base(configuration)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _fileUploadService = fileUploadService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Show(int id, string key)
        {
            //id是refType
            //key是refID ，需解密
            var decryptKey = EncryptTool.AesDecrypt(key);
            if (int.TryParse(decryptKey, out var decryptID) && EnumMapTool.isVlaid<UploadFileRefTypeEnum>(id))
            {
                var data = await _fileUploadService.ShowFiles(decryptID, (UploadFileRefTypeEnum)id);

                return Json(data);
            }

            return Json("[]");
        }

        [HttpPost]
        public async Task<IActionResult> DownloadAsync(int id, string key)
        {
            var decryptIDStr = EncryptTool.AesDecrypt(key);
            if (int.TryParse(decryptIDStr, out var decryptID))
            {
                var entity = await _fileUploadService.GetByID(decryptID, id);

                if (entity != null)
                {
                    var refTypeStr = ((UploadFileRefTypeEnum)id).ToString();
                    var path = $"{dir}\\{refTypeStr}\\{entity.FileName}";
                    var contentType = entity.ContentType;
                    var originName = entity.Name;
                    byte[] bytes;
                    using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        bytes = new byte[fs.Length];
                        var numBytesToRead = (int)fs.Length;
                        var numBytesRead = 0;
                        while (numBytesToRead > 0)
                        {
                            // Read may return anything from 0 to numBytesToRead.
                            int n = fs.Read(bytes, numBytesRead, numBytesToRead);

                            // Break when the end of the file is reached.
                            if (n == 0)
                                break;

                            numBytesRead += n;
                            numBytesToRead -= n;
                        }
                    }

                    //return File(bytes, contentType, originName);
                    return new FileContentResult(bytes, contentType)
                    {
                        FileDownloadName = originName
                    };
                }
            }

            return NotFound();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id, string key)
        {
            var result = false;

            try
            {
                var decryptIDStr = EncryptTool.AesDecrypt(key);
                if (int.TryParse(decryptIDStr, out var decryptID))
                {
                    result = await _fileUploadService.DeleteByID(decryptID, id);
                    if (result)
                    {
                        return Ok();
                    }
                    throw new Exception("刪除檔案時發生錯誤");
                }
                throw new Exception("找不到檔案");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
