using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ElectronApp.Database.Entities
{
    /// <summary>
    /// 帳號資料
    /// </summary>
    public class UserProfiles : ARecord
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
        /// 帳號
        /// </summary>                
        [Comment("帳號")]
        [Required]
        [DefaultValue("")]
        [Description("帳號")]
        public string Account { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>                
        [Comment("密碼")]
        [Required]
        [DefaultValue("")]
        [Description("密碼")]
        public string Mima { get; set; }
    }
}
