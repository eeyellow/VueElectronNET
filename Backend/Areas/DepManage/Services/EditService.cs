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
    public interface IEditService<U, T> : IBaseEditService<U, T>
    {
        /// <summary>
        /// 取得關聯的地支
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<fvmEarthlyBranch>> FindEarthlyBranchByIdAsync(int id);

        /// <summary>
        /// 取得關聯的使用者
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<fvmEditUsers>> FindUsersByIdAsync(int id);
    }

    /// <summary>
    /// Departments相關操作
    /// </summary>
    public class EditService<U, T> : IEditService<U, T>
        where U : fvmEdit
        where T : ARecord
    {
        private readonly DatabaseContext _context;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="context"></param>
        public EditService(DatabaseContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public ModelStateViewModel Valid(U fvm, ModelStateDictionary modelState)
        {
            if (string.IsNullOrWhiteSpace(fvm.Name))
            {
                modelState.AddModelError("Name", "名稱不可為空");
            }

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
        public async Task<T> FindByIdAsync(int id)
        {
            var entity = await _context.Set<T>().FirstOrDefaultAsync(a => a.ID == id);
            return entity;
        }

        /// <inheritdoc/>
        public async Task<List<fvmEditUsers>> FindUsersByIdAsync(int id)
        {
            var entityList = await (
                from rel in _context.Set<UserInDepartments>().Where(a => a.IsDelete == 0)
                join user in _context.Set<UserProfiles>().Where(a => a.IsDelete == 0)
                    on rel.UserID equals user.ID
                where rel.DepartmentID == id
                select new fvmEditUsers
                {
                    ID = rel.ID,
                    UserID = rel.UserID,
                    DepartmentID = rel.DepartmentID,
                    Name = user.Name,
                    IsDelete = user.IsDelete,
                }
            ).ToListAsync();

            return entityList;
        }

        /// <inheritdoc/>
        public async Task<List<fvmEarthlyBranch>> FindEarthlyBranchByIdAsync(int id)
        {
            var entityList = await (
                from entity in _context.Set<EarthlyBranchInDepartments>().Where(a => a.IsDelete == 0)
                where entity.DepartmentID == id
                select new fvmEarthlyBranch
                {
                    ID = entity.ID,
                    EnumValue = entity.EnumValue,
                    DepartmentID = entity.DepartmentID,
                    IsDelete = entity.IsDelete,
                }
            ).ToListAsync();

            return entityList;
        }
    }
}
