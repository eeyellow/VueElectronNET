// Licensed to the .NET Foundation under one or more agreements.

using ElectronApp.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using System.Reflection;

namespace ElectronApp.Database.Extensions
{
    /// <summary>
    /// 提供加入軟刪除查詢過濾器的擴充方法
    /// </summary>
    public static class SoftDeleteQueryFilter
    {
        /// <summary>
        /// 加入軟刪除查詢過濾器
        /// 參考 https://www.thereformedprogrammer.net/introducing-the-efcore-softdeleteservices-library-to-automate-soft-deletes/
        /// </summary>
        /// <param name="builder">ModelBuilder 物件</param>
        public static void Apply(ModelBuilder builder)
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                //other automated configurations left out
                if (typeof(IDelete).IsAssignableFrom(entityType.ClrType))
                {
                    entityType.AddSoftDeleteQueryFilter();
                }
            }
        }
    }

    /// <summary>
    /// DbContext 擴充方法
    /// </summary>
    public static class SoftDeleteExtension
    {
        /// <summary>
        /// 加入軟刪除查詢過濾器
        /// 參考 https://www.thereformedprogrammer.net/introducing-the-efcore-softdeleteservices-library-to-automate-soft-deletes/
        /// </summary>
        /// <param name="entityData"></param>
        public static void AddSoftDeleteQueryFilter(this IMutableEntityType entityData)
        {
            var methodToCall = typeof(SoftDeleteExtension)?.GetMethod(nameof(GetSoftDeleteFilter), BindingFlags.NonPublic | BindingFlags.Static)?.MakeGenericMethod(entityData.ClrType);
            var filter = methodToCall?.Invoke(null, Array.Empty<object>());
            entityData.SetQueryFilter((LambdaExpression)filter!);
            entityData.AddIndex(entityData.FindProperty(nameof(IDelete.IsDelete))!);
        }

        /// <summary>
        /// 取得軟刪除查詢過濾器
        /// 參考 https://www.thereformedprogrammer.net/introducing-the-efcore-softdeleteservices-library-to-automate-soft-deletes/
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        private static Expression<Func<TEntity, bool>> GetSoftDeleteFilter<TEntity>() where TEntity : class, IDelete
        {
            Expression<Func<TEntity, bool>> filter = x => x.IsDelete == 0;
            return filter;
        }

        /// <summary>
        /// 取得所有資料(包含已刪除)
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="dbSet"></param>
        /// <returns></returns>
        public static IQueryable<TModel> FindAllWithDelete<TModel>(this DbSet<TModel> dbSet) where TModel : class
        {
            return dbSet.IgnoreQueryFilters();
        }
    }
}
