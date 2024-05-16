using ElectronApp.Interfaces;
using ElectronApp.ViewModels;

namespace ElectronApp.Tools
{
    /// <summary> 擴充函式庫-分頁 </summary>
    public static class PagedListExtension
    {
        #region IQueryable<T> extensions

        /// <summary> 取得目前頁數 </summary>
        private static int GetCurrentPageIndex(object pageIndex) => pageIndex switch
        {
            null => 0,
            int index => index != 0 ? index - 1 : 0,
            _ => throw new Exception("pageIndex 必須是整數!!")
        };

        /// <summary> 依SortModel裡的資訊做分頁 </summary>
        public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, IPageSort sortModel)
            => await new PagedList<T>().InitializeAsync(source, GetCurrentPageIndex(sortModel.NowPage), sortModel.PageSize);

        #endregion
    }
}
