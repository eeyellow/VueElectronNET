using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ElectronApp.Database.Entities
{
    /// <summary>
    /// 地支與部門的關聯表
    /// </summary>
    public class EarthlyBranchInDepartments : ARecord
    {
        /// <summary>
        /// 地支列舉值
        /// </summary>
        [Comment("列舉值")]
        [Required]
        [Description("列舉值")]
        public int EnumValue { get; set; }

        /// <summary>
        /// 部門名稱
        /// </summary>
        [Comment("部門ID")]
        [Required]
        [Description("部門ID")]
        public long DepartmentID { get; set; }
    }
}
