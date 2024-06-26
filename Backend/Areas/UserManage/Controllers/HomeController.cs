﻿using ElectronApp.Areas.UserManage.Services;
using ElectronApp.Areas.UserManage.ViewModels;
using ElectronApp.Controllers;
using ElectronApp.Database.Contexts;
using ElectronApp.Database.Entities;
using ElectronApp.Enums;
using ElectronApp.Interfaces;
using ElectronApp.Tools;
using Microsoft.AspNetCore.Mvc;

namespace ElectronApp.Areas.UserManage.Controllers
{
    /// <summary>
    /// 使用者帳號管理
    /// </summary>
    [Area("UserManage")]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DatabaseContext _context;
        private readonly IBaseListService<ListViewModel, UserProfiles> _listService;
        private readonly IBaseEditService<fvmEdit, UserProfiles> _editService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="logger">The logger</param>
        /// <param name="context">The database context</param>
        /// <param name="listService"></param>
        /// <param name="editService"></param>
        public HomeController(IConfiguration configuration,
                              ILogger<HomeController> logger,
                              DatabaseContext context,
                              IBaseListService<ListViewModel, UserProfiles> listService,
                              IBaseEditService<fvmEdit, UserProfiles> editService)
            : base(configuration)
        {
            _logger = logger;
            _context = context;
            _listService = listService;
            _editService = editService;
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
            var list = _listService.FilterBySearch(fvm);
            // 排序 & 分頁
            var pageList = await GetPagedListAsync(fvm, list);

            // 列出資料欄位
            var jsonResult = from e in pageList
                             select new
                             {
                                 ID = e.ID,
                                 Account = e.Account,
                                 Name = e.Name
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

            return HttpTool.CreateResponse(result);
        }
    }
}
