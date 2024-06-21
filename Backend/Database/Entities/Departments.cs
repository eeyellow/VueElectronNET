using ElectronApp.Database.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectronApp.Database.Entities
{
    /// <summary>
    /// 部門資料
    /// </summary>
    public class Departments : ARecord
    {
        /// <summary>
        /// 名稱
        /// </summary>
        [Comment("名稱")]
        [Required]
        [DefaultValue("")]
        [Description("名稱")]
        public string Name { get; set; }

        /// <summary>
        /// 縮寫
        /// </summary>
        [Comment("縮寫")]
        [DefaultValue("")]
        [Description("縮寫")]
        public string Alias { get; set; }

        /// <summary>
        /// 上層部門的ID
        /// </summary>
        [Comment("上層部門的ID")]
        [DefaultValue("0")]
        [Description("上層部門的ID")]
        public int ParentID { get; set; } = 0;

        /// <summary>
        /// 成立日期
        /// </summary> 
        [Column(TypeName = "timestamp")]
        [Comment("成立日期")]
        [SqlDefaultValue("DateTime('now')")]
        [Description("成立日期")]
        public DateTime EstablishDate { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        [Comment("是否啟用")]
        [SqlDefaultValue("1")]
        [Description("是否啟用")]
        public int IsEnable { get; set; } = 1;
    }
}
