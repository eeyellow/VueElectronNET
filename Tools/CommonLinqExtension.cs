using ElectronApp.Interfaces;
using System.Linq.Expressions;

namespace ElectronApp.Tools
{
    /// <summary> Linq Extension </summary>
    public static class CommonLinqExtension
    {
        private const string _oy = "OrderBy";
        private const string _oyd = "OrderByDescending";
        private const string _ty = "ThenBy";
        private const string _tyd = "ThenByDescending";


        /// <summary> 擴充動態排序底層方法(含關聯資料表欄位排序) </summary>
        /// <typeparam name="T">IQueryable</typeparam>
        /// <param name="source">資料</param>
        /// <param name="property">欄位名稱</param>
        /// <param name="methodName">排序方法</param>
        /// <returns></returns>
        private static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            var props = property.Split(".");
            var type = typeof(T);
            var arg = Expression.Parameter(type, "x");
            Expression expr = arg;

            foreach (var prop in props)
            {
                var pi = type.GetProperty(prop);
                if (pi == null)
                {
                    throw new Exception($"OrderByException !! \n屬性[{prop}並不屬於該資料表[{type.Name}");
                }
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            var delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            var lambda = Expression.Lambda(delegateType, expr, arg);

            var result = typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), type)
                    .Invoke(null, new object[] { source, lambda });
            if (result == null)
            {
                return (IOrderedQueryable<T>)Enumerable.Empty<T>().AsQueryable();
            }

            return (IOrderedQueryable<T>)result;
        }

        /// <summary> 擴充動態排序 OrderBy </summary>
        /// <typeparam name="T">IQueryable</typeparam>
        /// <param name="source">資料</param>
        /// <param name="property">欄位名稱</param>
        /// <returns></returns>
        private static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property) => ApplyOrder(source, property, _oy);

        /// <summary> 擴充動態排序 OrderByDescending </summary>
        /// <typeparam name="T">IQueryable</typeparam>
        /// <param name="source">資料</param>
        /// <param name="property">欄位名稱</param>
        /// <returns></returns>
        private static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string property) => ApplyOrder(source, property, _oyd);

        /// <summary> 擴充動態排序 ThenBy </summary>
        /// <typeparam name="T">IOrderedQueryable</typeparam>
        /// <param name="source">資料</param>
        /// <param name="property">欄位名稱</param>
        /// <returns></returns>
        private static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string property) => ApplyOrder(source, property, _ty);

        /// <summary> 擴充動態排序 ThenByDescending </summary>
        /// <typeparam name="T">IOrderedQueryable</typeparam>
        /// <param name="source">資料</param>
        /// <param name="property">欄位名稱</param>
        /// <returns></returns>
        private static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string property) => ApplyOrder(source, property, _tyd);

        /// <summary> 解析關聯屬性並傳回相關的 type 及 lambda </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="properties"></param>
        /// <param name="arg"></param>
        /// <param name="type"></param>
        /// <param name="expr"></param>
        private static bool SplitTypeAndExpression<T>(string properties, Expression arg, out Type type, out Expression expr)
        {
            type = typeof(T);
            expr = arg;
            foreach (var prop in properties.Split("."))
            {
                var pi = type.GetProperty(prop);
                if (pi == null)
                {
                    throw new Exception($"OrderByException !! \n屬性[{prop}並不屬於該資料表[{type.Name}]");
                }
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }

            return true;
        }

        /// <summary> 指定排序的欄位及方法(遞增 or 遞減) </summary>
        /// <typeparam name="T">排序</typeparam>
        /// <param name="source">IQueryable</param>
        /// <param name="sortModel">查詢模型</param>
        /// <example>多欄位排序請傳入 sortModel.SortField = "PrimaryDate:desc,Code:asc" </example>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, IPageSort sortModel)
        {
            #region 多欄位排序(含關聯資料表欄位排序)

            if (sortModel.SortField.IndexOf(",", StringComparison.Ordinal) >= 0)
            {
                var fields = sortModel.SortField.Split(",");
                var actions = sortModel.SortAction.Split(",");

                for (var i = 0; i < fields.Length; i++)
                {
                    if (i == 0)
                    {
                        source = actions[i].ToLower() == "asc" ? OrderBy(source, fields[i]) : OrderByDescending(source, fields[i]);
                    }
                    else
                    {
                        var action = actions.Length <= i ? actions[0] : actions[i];
                        source = action.ToLower() == "asc" ? ThenBy((IOrderedQueryable<T>)source, fields[i]) : ThenByDescending((IOrderedQueryable<T>)source, fields[i]);
                    }
                }

                return source;
            }

            #endregion

            #region 關聯資料表欄位排序

            if (sortModel.SortField.IndexOf(".", StringComparison.Ordinal) >= 0)
            {
                return MultiOrderBy(source, sortModel.SortField, sortModel.SortAction);
            }

            #endregion

            #region 一般單欄位排序

            var type = typeof(T);
            var property = type.GetProperty(sortModel.SortField);

            if (property != null)
            {
                var parameter = Expression.Parameter(type, "pr");
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var lambda = Expression.Lambda(propertyAccess, parameter);
                var methodName = string.Equals(sortModel.SortAction, "asc", StringComparison.OrdinalIgnoreCase) ? _oy : _oyd;

                var resultExp = Expression.Call(typeof(Queryable), methodName, new[] { type, property.PropertyType }, source.Expression, Expression.Quote(lambda));
                return source.Provider.CreateQuery<T>(resultExp);
            }

            #endregion\

            return source;
        }

        /// <summary> 指定排序的欄位及方法(以關聯資料表欄位做排序) </summary>
        /// <typeparam name="T">排序</typeparam>
        /// <param name="source">IQueryable</param>
        /// <param name="properties">屬性[.屬性.屬性---].欄位名稱</param>
        /// <param name="orderType">排序方式(Asc、Desc)</param>
        private static IQueryable<T> MultiOrderBy<T>(this IQueryable<T> source, string properties, string orderType)
        {
            var dataType = typeof(T);
            var parameter = Expression.Parameter(dataType, "x");
            if (!SplitTypeAndExpression<T>(properties, parameter, out var paramType, out var propertyAccess))
            {
                return source;
            }

            var delegateType = typeof(Func<,>).MakeGenericType(dataType, paramType);
            var lambda = Expression.Lambda(delegateType, propertyAccess, parameter);
            var methodName = string.Equals(orderType, "asc", StringComparison.OrdinalIgnoreCase) ? _oy : _oyd;
            var resultExp = Expression.Call(typeof(Queryable), methodName, new[] { dataType, paramType }, source.Expression, Expression.Quote(lambda));
            return source.Provider.CreateQuery<T>(resultExp);
        }

        /// <summary> 執行 like 查詢(若關鍵字為空orNull則不做查詢動作) </summary>
        /// <typeparam name="T">查詢某欄位的關鍵字</typeparam>
        /// <param name="source">IQueryable</param>
        /// <param name="propertyName">欄位名稱</param>
        /// <param name="keyword">關鍵字</param>
        public static IQueryable<T> Like<T>(this IQueryable<T> source, string propertyName, string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return source;
            }

            if (propertyName.IndexOf(".", StringComparison.Ordinal) >= 0)
            {
                return MultiLike(source, propertyName, keyword);
            }

            var type = typeof(T);
            var property = type.GetProperty(propertyName);
            if (property != null)
            {
                var parameter = Expression.Parameter(type, "p");
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var constant = Expression.Constant("%" + keyword + "%");
                var like = typeof(string).GetMethod("Contains");

                if (like != null)
                {
                    var methodExp = Expression.Call(like, propertyAccess, constant);
                    var lambda = Expression.Lambda<Func<T, bool>>(methodExp, parameter);

                    return source.Where(lambda);
                }
            }
            return source;
        }

        /// <summary> 執行 like 查詢(以關聯資料表欄位做查詢) </summary>
        /// <typeparam name="T">查詢某欄位的關鍵字</typeparam>
        /// <param name="source">IQueryable</param>
        /// <param name="properties">屬性[.屬性.屬性---].欄位名稱</param>
        /// <param name="keyword">關鍵字</param>
        private static IQueryable<T> MultiLike<T>(this IQueryable<T> source, string properties, string keyword)
        {
            var dataType = typeof(T);
            var parameter = Expression.Parameter(dataType, "x");
            if (!SplitTypeAndExpression<T>(properties, parameter, out _, out var propertyAccess))
            {
                return source;
            }

            var constant = Expression.Constant(keyword);
            var like = typeof(string).GetMethod("Contains");
            if (like != null)
            {
                Expression lambda = Expression.Call(propertyAccess, like, constant);
                var where = Expression.Lambda<Func<T, bool>>(lambda, parameter);
                return source.Where(where);
            }
            return source;
        }
    }
}
