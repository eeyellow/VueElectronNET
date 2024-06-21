using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ElectronApp.Database.Entities
{
    /// <summary>
    /// 人員與部門的關聯表
    /// </summary>
    public class UserInDepartments : ARecord
    {
        /// <summary>
        /// 名稱
        /// </summary>
        [Comment("人員ID")]
        [Required]
        [Description("人員ID")]
        public long UserID { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        [Comment("部門ID")]
        [Required]
        [Description("部門ID")]
        public long DepartmentID { get; set; }
    }
}
