﻿using ElectronApp.Database.Contexts;
using ElectronApp.Database.Entities;
using ElectronApp.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ElectronApp.Areas.UserManage.Services
{
    /// <summary>
    /// UserProfiles相關操作
    /// </summary>
    public class ListService<V, T> : IBaseListService<V, T>
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
