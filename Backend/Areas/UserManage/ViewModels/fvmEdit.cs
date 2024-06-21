using System.ComponentModel;

namespace ElectronApp.Areas.UserManage.ViewModels
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

        /// <summary> 帳號 </summary>
        [DisplayName("帳號")]
        public string Account { get; set; }

        /// <summary> 密碼 </summary>
        [DisplayName("密碼")]
        public string Mima { get; set; }
    }
}
