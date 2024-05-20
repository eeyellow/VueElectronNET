using ElectronApp.Areas.DepManage.ViewModels;
using ElectronApp.Database.Contexts;
using ElectronApp.Database.Entities;
using ElectronApp.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ElectronApp.Areas.DepManage.Services
{
    /// <summary>
    /// Departments相關操作介面
    /// </summary>
    public interface IListService<V, T> : IBaseListService<V, T>
    {

    }

    /// <summary>
    /// Departments相關操作
    /// </summary>
    public class ListService<V, T> : IListService<V, T>
        where V : class
        where T : ARecord
    {
        private readonly DatabaseContext _context;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="context"></param>
        public ListService(DatabaseContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public IQueryable<T> FilterBySearch(V fvm)
        {
            return _context.Set<T>();
        }

        

        
    }
}
