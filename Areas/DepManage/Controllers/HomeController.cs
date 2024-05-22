using ElectronApp.Areas.DepManage.Services;
using ElectronApp.Areas.DepManage.ViewModels;
using ElectronApp.Controllers;
using ElectronApp.Database.Contexts;
using ElectronApp.Database.Entities;
using ElectronApp.Enums;
using ElectronApp.Interfaces;

using ElectronApp.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectronApp.Areas.DepManage.Controllers
{
    /// <summary>
    /// 部門管理
    /// </summary>
    [Area("DepManage")]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DatabaseContext _context;
        private readonly IListService<ListViewModel, Departments> _listService;
        private readonly IEditService<fvmEdit, Departments> _editService;
        private readonly IEditUserService<fvmEditUsers, UserInDepartments> _editUserService;
        private readonly IEditEarthlyBranchService<fvmEarthlyBranch, EarthlyBranchInDepartments> _editEarthlyBranchService;
        private readonly IQueryService<Departments> _queryService;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="logger">The logger</param>
        /// <param name="context">The database context</param>
        /// <param name="listService">列表相關服務</param>
        /// <param name="editService">新增編輯相關服務</param>
        /// <param name="editUserService">新增編輯User關聯相關服務</param>
        /// <param name="editEarthlyBranchService">新增編輯EarthlyBranch關聯相關服務</param>
        /// <param name="queryService">查詢資料相關服務</param>
        public HomeController(IConfiguration configuration,
                              ILogger<HomeController> logger,
                              DatabaseContext context,
                              IListService<ListViewModel, Departments> listService,
                              IEditService<fvmEdit, Departments> editService,
                              IEditUserService<fvmEditUsers, UserInDepartments> editUserService,
                              IEditEarthlyBranchService<fvmEarthlyBranch, EarthlyBranchInDepartments> editEarthlyBranchService,
                              IQueryService<Departments> queryService)
            : base(configuration)
        {
            _logger = logger;
            _context = context;
            _listService = listService;
            _editService = editService;
            _editUserService = editUserService;
            _editEarthlyBranchService = editEarthlyBranchService;
            _queryService = queryService;
        }

        /// <summary>
        /// 列表頁
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index() => View();

        /// <summary>
        /// 列表頁
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index(ListViewModel fvm)
        {
            // 一般頁面載入時，直接回傳ViewModel
            if (!Request.IsAjaxRequest())
            {
                return View(fvm);
            }

            // 搜尋
            var query = _listService.FilterBySearch(fvm);
            var list = _listService.GetListModel(query);

            // 排序 & 分頁
            var pageList = await GetPagedListAsync(fvm, list);

            // 列出資料欄位
            var jsonResult = from e in pageList
                             select new
                             {
                                 ID = e.ID,
                                 Name = e.Name,
                                 Alias = e.Alias,
                                 ParentDepName = e.ParentDepName,
                                 EstablishDate = e.EstablishDate.ToString("yyyy/MM/dd")
                             };

            return LcGridJson(pageList.TotalItemCount, jsonResult);
        }

        /// <summary>
        /// 新增頁
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Create()
        {
            var model = new fvmEdit
            {
                ID = 0,
                VueMode = (int)VueModeEnum.Edit
            };

            return View("Edit", model);
        }

        /// <summary>
        /// 編輯頁
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = new fvmEdit
            {
                ID = id,
                VueMode = (int)VueModeEnum.Edit
            };

            return View(model);
        }

        /// <summary>
        /// 檢視頁
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Detail(int id)
        {
            var model = new fvmEdit
            {
                ID = id,
                VueMode = (int)VueModeEnum.Detail
            };

            return View("Edit", model);
        }

        /// <summary>
        /// 編輯頁 - 取得資料
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetEditDataAsync(int id)
        {
            var entity = await _editService.FindByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            var model = ModelTool.MappingAndReturn(entity, new fvmEdit(), []);

            model.Users = await _editService.FindUsersByIdAsync(id);
            model.EarthlyBranch = await _editService.FindEarthlyBranchByIdAsync(id);

            model.VueMode = (int)VueModeEnum.Edit;

            return Json(model);
        }

        /// <summary>
        /// 編輯頁 - 接收資料
        /// </summary>
        /// <param name="fvm"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostEditDataAsync(fvmEdit fvm)
        {
            var vallidResult = _editService.Valid(fvm, ModelState);

            if (vallidResult.ModelState != null && vallidResult.ModelState.Any())
            {
                // 驗證不通過
                return HttpTool.CreateResponse(vallidResult);
            }

            var result = await _editService.Save(fvm);

            
            foreach (var item in fvm.Users ?? new List<fvmEditUsers>())
            {
                item.DepartmentID = ((Departments)result.Data).ID;
                await _editUserService.Save(item);
            }

            foreach (var item in fvm.EarthlyBranch ?? new List<fvmEarthlyBranch>())
            {
                item.DepartmentID = ((Departments)result.Data).ID;
                await _editEarthlyBranchService.Save(item);
            }

            return HttpTool.CreateResponse(result);
        }
    
        /// <summary>
        /// 取得所有的部門資料
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetAllDepData()
        {
            var result = await _queryService.FindAll().ToListAsync();

            return Json(result);
        }

        /// <summary>
        /// 取得所有的使用者資料
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetAllUserData()
        {
            var result = await _queryService.FindUsersAsync();

            return Json(result);
        }
    }
}
