using ElectronApp.Areas.DepManage.ViewModels;
using ElectronApp.Database.Contexts;
using ElectronApp.Database.Entities;
using ElectronApp.Interfaces;
using Microsoft.EntityFrameworkCore;
using ElectronApp.Enums;
using ElectronApp.Tools;

namespace ElectronApp.Services
{
    public interface IFileUploadService
    {
        Task<bool> DeleteByID(long id);
        Task<bool> DeleteByID(long id, int refType);
        Task<UploadFile> GetByID(long id);
        Task<UploadFile> GetByID(long id, int refType);
        Task<bool> Save(IFormFile file, long refID, UploadFileRefTypeEnum refType, string sessionID = "");
        Task<bool> Save(List<IFormFile> files, long refID, UploadFileRefTypeEnum refType, string sessionID = "");
        Task<IList<FileShowViewModel>> ShowFiles(long refID, UploadFileRefTypeEnum refType);
    }

    /// <summary>
    /// 檔案上傳
    /// </summary>
    public class FileUploadService : IFileUploadService
    {
        private readonly DatabaseContext _context;
        private static readonly string dir = $"{WebHostEnvironmentManager.ContentRootPath}\\UploadFiles\\";

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="context"></param>
        public FileUploadService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<UploadFile> GetByID(long id) =>
            await _context.Set<UploadFile>().Where(a => a.IsDelete == 0).FirstOrDefaultAsync(a => a.ID == id);

        public async Task<UploadFile> GetByID(long id, int refType) =>
            await _context.Set<UploadFile>().Where(a => a.IsDelete == 0).FirstOrDefaultAsync(a => a.ID == id && a.RefType == refType);

        public async Task<bool> DeleteByID(long id)
        {
            try
            {
                var entity = await GetByID(id);
                if (entity != null)
                {
                    entity.IsDelete = 1;
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteByID(long id, int refType)
        {
            try
            {
                var entity = await GetByID(id, refType);
                if (entity != null)
                {
                    entity.IsDelete = 1;
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 儲存
        /// </summary>
        /// <param name="file"></param>
        /// <param name="refID"></param>
        /// <param name="refType"></param>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        public async Task<bool> Save(IFormFile file, long refID, UploadFileRefTypeEnum refType, string sessionID = "")
        {
            if (refID == 0)
            {
                return false;
            }

            try
            {
                var uploadPath = $"{dir}\\{refType.ToString()}\\";
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                if (string.IsNullOrWhiteSpace(sessionID))
                {
                    sessionID = Guid.NewGuid().ToString();
                }

                // 儲存資料表紀錄
                var fileName = Guid.NewGuid().ToString(); //要存到伺服器上的檔案名稱
                var entity = new UploadFile
                {
                    RefID = refID,
                    RefType = (int)refType,
                    Name = file.FileName, //原始檔案名稱
                    FileName = fileName,  //存到伺服器上後的檔案名稱
                    ContentType = file.ContentType,
                };
                await _context.Set<UploadFile>().AddAsync(entity);
                await _context.SaveChangesAsync();

                // 儲存實體檔案
                var filePath = uploadPath + fileName;
                await using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 儲存(多檔案)
        /// </summary>
        /// <param name="files"></param>
        /// <param name="refID"></param>
        /// <param name="refType"></param>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        public async Task<bool> Save(List<IFormFile> files, long refID, UploadFileRefTypeEnum refType, string sessionID = "")
        {
            var allResult = true;
            if (refID == 0)
            {
                return false;
            }

            foreach (var file in files)
            {
                var result = await Save(file, refID, refType, sessionID);
                allResult = allResult && result;
            }
            return allResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="refID"></param>
        /// <param name="refType"></param>
        /// <returns></returns>
        public async Task<IList<FileShowViewModel>> ShowFiles(long refID, UploadFileRefTypeEnum refType)
        {
            var data = await _context.Set<UploadFile>()
                .Where(a => a.IsDelete == 0)
                .Where(a => a.RefType == (int)refType)
                .Where(a => a.RefID == refID)
                .Select(a => new
                {
                    ID = a.ID,
                    Name = a.Name
                }).ToListAsync();

            var encryptData = data.Select(a => new FileShowViewModel
            {
                ID = EncryptTool.AesEncrypt(a.ID.ToString()),
                Name = a.Name,
            }).ToList();

            return encryptData;
        }
    }

    public class FileShowViewModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }
}
