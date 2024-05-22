using ElectronApp.Database.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectronApp.Database.Entities
{
    /// <summary>
    /// 上傳檔案資料
    /// </summary>
    public class UploadFile : ARecord
    {
        /// <summary>
        /// 參照資料編號
        /// </summary>
        [Comment("參照資料編號")]
        [Required]
        [Description("參照資料編號")]
        public long RefID { get; set; }

        /// <summary>
        /// 參照功能編號
        /// </summary>
        [Comment("參照功能編號")]
        [Required]
        [Description("參照功能編號")]
        public int RefType { get; set; }

        /// <summary>
        /// 檔案名稱
        /// </summary>
        [Comment("檔案名稱")]
        [DefaultValue("")]
        [Description("檔案名稱")]
        public string Name { get; set; }

        /// <summary>
        /// 實體檔案名稱
        /// </summary>
        [Comment("實體檔案名稱")]
        [DefaultValue("")]
        [Description("實體檔案名稱")]
        public string FileName { get; set; }

        /// <summary>
        /// ContentType
        /// </summary>
        [Comment("ContentType")]
        [DefaultValue("")]
        [Description("ContentType")]
        public string ContentType { get; set; }
    }
}
