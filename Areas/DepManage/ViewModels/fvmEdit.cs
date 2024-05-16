using System.ComponentModel;

namespace ElectronApp.Areas.DepManage.ViewModels
{
    /// <summary>
    /// 新增/編輯的ViewModel
    /// </summary>
    public class fvmEdit
    {
        /// <summary> 流水號 </summary>
        public long ID { get; set; }

        /// <summary> Vue模式 </summary>
        public int VueMode { get; set; }

        /// <summary> 名稱 </summary>
        [DisplayName("名稱")]
        public string Name { get; set; }

        /// <summary> 縮寫 </summary>
        [DisplayName("縮寫")]
        public string Alias { get; set; }

        /// <summary> 上層部門的ID </summary>
        [DisplayName("上層部門的ID")]
        public int ParentID { get; set; }

        /// <summary> 成立日期 </summary>
        [DisplayName("成立日期")]
        public DateTime EstablishDate { get; set; }
    }
}
