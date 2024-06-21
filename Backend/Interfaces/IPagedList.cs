namespace ElectronApp.Interfaces
{
    /// <summary> 分頁屬性 </summary>
    public interface IPager
    {
        /// <summary> 總頁數 </summary>
        int PageCount { get; }

        /// <summary> 資料總數 </summary>
        int TotalItemCount { get; }

        /// <summary> 頁面索引(從0開始) </summary>
        int PageIndex { get; }

        /// <summary> 分頁索引(從1開始) </summary>
        int PageNumber { get; }

        /// <summary> 每頁資料行數量上限 </summary>
        int PageSize { get; }

        /// <summary> 是否為第一個 </summary>
        bool IsFirstPage { get; }

        /// <summary> 是否為最後一頁 </summary>
        bool IsLastPage { get; }

        /// <summary> 當前頁面資料總數 </summary>
        int ItemCount { get; }
    }


    /// <summary> 分頁清單介面(未指定泛型) </summary>
    public interface IPagedList : System.Collections.IEnumerable, IPager
    {
    }

    /// <summary> 分頁清單介面 </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPagedList<T> : IPagedList, IList<T>, IEnumerable<T>
    {
    }
}
