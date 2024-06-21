using ElectronApp.Interfaces;
using ElectronApp.Tools;
using ElectronApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ElectronApp.Controllers
{
    /// <summary>
    /// 基底控制器
    /// </summary>
    public class BaseController : Controller
    {
        #region 宣告、建構子

        /// <summary> Configuration </summary>
        protected readonly IConfiguration Configuration;

        /// <summary> Constructor </summary>
        /// <param name="configuration"></param>
        public BaseController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion

        #region 列表/搜尋條件

        /// <summary> 回傳LC-Grid接收Json </summary>
        /// <param name="total"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public JsonResult LcGridJson(int total, object result)
            => Json(new Dictionary<string, object>
            {
                { "total", total },
                { "rows", result },
            });

        #endregion 列表/搜尋條件

        /// <summary> 取得分頁模型 </summary>
        /// <returns></returns>
        protected static async Task<IPagedList<TResultModel>> GetPagedListAsync<TSearchModel, TResultModel>(TSearchModel search, IQueryable<TResultModel> query) where TSearchModel : BaseSearchViewModel
            => await query.OrderBy(search).ToPagedListAsync(search);
    }
}
