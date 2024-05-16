using ElectronApp.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace ElectronApp.ViewModels
{
    /// <summary> 分頁清單 </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedList<T> : List<T>, IPagedList<T>
    {
        #region IPagedList Members

        /// <summary>頁面索引(從0開始)</summary>
        public int PageIndex { get; private set; }

        /// <summary>每頁資料行數量上限</summary>
        public int PageSize { get; private set; }

        /// <summary>總頁數</summary>
        public int PageCount { get; private set; }

        /// <summary>資料總數</summary>
        public int TotalItemCount { get; private set; }

        /// <summary>當前頁面資料總數</summary>
        public int ItemCount { get; private set; }

        /// <summary>分頁索引(從1開始)</summary>
        public int PageNumber => PageIndex + 1;

        /// <summary>是否為第一個</summary>
        public bool IsFirstPage => PageIndex <= 0;

        /// <summary>是否為最後一頁</summary>
        public bool IsLastPage => PageIndex >= PageCount - 1;

        #endregion

        #region 建構函式

        /// <summary> 預設建構子(供子類別使用) </summary>
        public PagedList() { }

        #endregion



        /// <summary> 初始化分頁資訊與資料列 </summary>
        /// <param name="source"></param>
        /// <param name="index"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        public async Task<IPagedList<T>> InitializeAsync(IQueryable<T> source, int index, int pageSize, int? totalCount = null)
        {
            #region 參數檢查與修正

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "PageIndex cannot be below 0.");
            }
            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), "PageSize cannot be less than 1.");
            }

            source ??= new List<T>().AsQueryable();

            #endregion

            #region 分頁屬性設定

            TotalItemCount = totalCount ?? (source.Provider is IAsyncQueryProvider ? await source.CountAsync() : await Task.FromResult(source.Count()));

            PageSize = pageSize;
            PageIndex = index;
            if (TotalItemCount > 0)
            {
                PageCount = (int)Math.Ceiling(TotalItemCount / (double)PageSize);
            }
            else
            {
                PageCount = 0;
            }

            // 若指定頁數大於總頁數, 則頁數歸零
            if (PageIndex >= PageCount)
            {
                PageIndex = 0;
                index = 0;
            }

            #endregion

            #region 資料列相關屬性設定

            if (PageIndex < PageCount - 1)
            {
                ItemCount = PageSize;
            }
            else
            {
                ItemCount = TotalItemCount % PageSize;
            }

            //### add items to internal list

            if (totalCount.HasValue)
            {
                //有傳此值，表示source已經是分頁後目前頁數的資料了，所以直接顯示出來即可。
                AddRange(source);
            }
            else if (TotalItemCount > 0)
            {
                if (TotalItemCount <= pageSize)
                {
                    AddRange(source.Provider is IAsyncQueryProvider ?
                             await source.ToListAsync() :
                             await Task.FromResult(source.ToList()));
                }
                else if (index == 0)
                {
                    AddRange(source.Take(pageSize));
                }
                else
                {
                    var skipNumber = index * pageSize;
                    AddRange(source.Skip(skipNumber).Take(pageSize));
                }
            }

            #endregion

            return this;
        }
    }
}
