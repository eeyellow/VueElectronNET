using ElectronApp.Areas.DepManage.ViewModels;
using ElectronApp.Database.Contexts;
using ElectronApp.Database.Entities;
using ElectronApp.Interfaces;
using ElectronApp.Tools;
using ElectronApp.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace ElectronApp.Areas.DepManage.Services
{
    /// <summary>
    /// Departments相關操作介面
    /// </summary>
    public interface IEditUserService<U, T> : IBaseEditService<U, T>
    {
        
    }

    /// <summary>
    /// Departments相關操作
    /// </summary>
    public class EditUserService<U, T> : IEditUserService<U, T>
        where U : fvmEditUsers
        where T : ARecord
    {
        private readonly DatabaseContext _context;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="context"></param>
        public EditUserService(DatabaseContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public ModelStateViewModel Valid(U fvm, ModelStateDictionary modelState)
        {
            var result = new ModelStateViewModel
            {
                StatusCode = modelState.IsValid ? 200 : 400,
                Data = fvm,
                ModelState = modelState.ToSerializedList()
            };

            return result;
        }

        /// <inheritdoc/>
        public async Task<StatusResultViewModel> Save(U fvm)
        {
            var result = new StatusResultViewModel();

            if (fvm.ID == 0) //新增
            {
                var entity = Activator.CreateInstance<T>();
                ModelTool.Mapping(fvm, entity);

                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    await _context.Set<T>().AddAsync(entity);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    result.Data = entity;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    result.StatusCode = StatusCodes.Status500InternalServerError;
                    result.Message.Add(ex.Message);
                }
            }
            else //編輯
            {
                var entity = await _context.Set<T>().FirstOrDefaultAsync(a => a.ID == fvm.ID);
                if (entity == null)
                {
                    result.StatusCode = StatusCodes.Status500InternalServerError;
                    result.Message.Add("找不到此Entity");
                    return result;
                }

                ModelTool.Mapping(fvm, entity);

                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    result.Data = entity;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    result.StatusCode = StatusCodes.Status500InternalServerError;
                    result.Message.Add(ex.Message);
                }
            }

            return result;
        }

        /// <inheritdoc/>
        public Task<T> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
