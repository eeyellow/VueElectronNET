using ElectronApp.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ElectronApp.ViewModels
{
    /// <summary> 基底排序模型 </summary>
    public class BaseSortViewModel : IPageSort
    {
        /// <summary>目前頁數(default:1 不可為負)</summary>
        public int NowPage { get; set; } = 1;

        /// <summary>分頁筆數(default:10 不可為負)</summary>
        public int PageSize { get; set; } = 10;

        /// <summary>目前排序欄位</summary>
        public string SortField { get; set; } = "ID";

        /// <summary>排序方法</summary>
        public string SortAction { get; set; } = "DESC";

        /// <summary>打開進階查詢</summary>
        public bool OpenSearchMore { get; set; }
    }

    /// <summary> 基底搜尋模型 </summary>
    public class BaseSearchViewModel : BaseSortViewModel
    {
        /// <summary> 查詢欄位 </summary>
        public int? ByField { get; set; }

        /// <summary> 搜尋關鍵字 </summary>
        [Display(Name = "Txt_關鍵字查詢")]
        public string ByKeyword { get; set; } = "";
    }
}
