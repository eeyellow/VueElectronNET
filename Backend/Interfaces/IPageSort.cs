namespace ElectronApp.Interfaces
{
    /// <summary> 基底排序模型介面 </summary>
    public interface IPageSort
    {
        /// <summary> 目前頁數 </summary>
        int NowPage { get; set; }

        /// <summary> 分頁筆數 </summary>
        int PageSize { get; set; }

        /// <summary> 目前排序欄位 </summary>
        string SortField { get; set; }

        /// <summary> 排序方法 </summary>
        string SortAction { get; set; }
    }
}
