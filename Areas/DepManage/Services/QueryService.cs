using ElectronApp.Database.Contexts;
using ElectronApp.Database.Entities;

namespace ElectronApp.Areas.DepManage.Services
{
    /// <summary> 新增/編輯功能相關操作服務介面 </summary>
    

    public interface IQueryService<T>
    {
        /// <summary>
        /// 取得資料
        /// </summary>
        /// <returns></returns>
        IQueryable<T> FindAll(int isDelete = 0);
    }

    /// <summary>
    /// Departments相關操作
    /// </summary>
    public class QueryService<T> : IQueryService<T> where T : ARecord
    {
        private readonly DatabaseContext _context;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="context"></param>
        public QueryService(DatabaseContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public IQueryable<T> FindAll(int isDelete = 0)
        {
            return _context.Set<T>().Where(a => a.IsDelete == isDelete);
        }
    }
}
