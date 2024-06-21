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
        /// <summary>
        /// 取得列表模型
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IQueryable<fvmList> GetListModel(IQueryable<Departments> query);
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
            var query = _context.Set<T>();

            // 使用反射取得ByKeyword屬性的值
            var byKeywordProperty = fvm.GetType().GetProperty("ByKeyword");
            if (byKeywordProperty != null)
            {
                var byKeywordValue = byKeywordProperty.GetValue(fvm);
                // 在這裡可以使用byKeywordValue來進行後續的處理
            }

            return query;
        }

        /// <inheritdoc/>
        public IQueryable<fvmList> GetListModel (IQueryable<Departments> query)
        {
            var result = (
                from q in query
                join parent in _context.Set<Departments>() 
                    on q.ParentID equals parent.ID
                select new fvmList
                {
                    ID = q.ID,
                    Alias = q.Alias,
                    Name = q.Name,
                    ParentDepName = parent.Name,
                    EstablishDate = q.EstablishDate,
                }
            );

            return result;
        }
    }
}
