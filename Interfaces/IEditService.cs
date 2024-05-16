﻿using ElectronApp.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ElectronApp.Interfaces
{
    /// <summary> 新增/編輯功能相關操作服務介面 </summary>
    public interface IEditService<U, T>
    {
        /// <summary>
        /// 儲存變更
        /// </summary>
        /// <param name="fvm"></param>
        /// <returns></returns>
        Task<StatusResultViewModel> Save(U fvm);
        /// <summary>
        /// 驗證
        /// </summary>
        /// <param name="fvm"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        ModelStateViewModel Valid(U fvm, ModelStateDictionary modelState);
    }
}
