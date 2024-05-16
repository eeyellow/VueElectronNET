namespace ElectronApp.Interfaces
{
    /// <summary> 列表功能相關操作服務介面 </summary>
    public interface IListService<V, T>
    {
        /// <summary>
        /// 列表搜尋篩選
        /// </summary>
        /// <param name="fvm"></param>
        /// <returns></returns>
        IQueryable<T> FilterBySearch(V fvm);
        /// <summary>
        /// 依ID尋找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> FindByIdAsync(int id);
    }
}
