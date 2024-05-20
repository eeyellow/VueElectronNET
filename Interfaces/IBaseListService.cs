namespace ElectronApp.Interfaces
{
    /// <summary> 列表功能相關操作服務介面 </summary>
    public interface IBaseListService<V, T>
    {
        /// <summary>
        /// 列表搜尋篩選
        /// </summary>
        /// <param name="fvm"></param>
        /// <returns></returns>
        IQueryable<T> FilterBySearch(V fvm);
    }
}
