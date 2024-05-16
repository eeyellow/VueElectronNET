using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections;

namespace ElectronApp.Tools
{
    /// <summary>
    /// ModelState擴充方法
    /// </summary>
    public static class ModelStateExtensions
    {
        /// <summary>
        /// 序列化為字典
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public static IDictionary ToSerializedDictionary(this ModelStateDictionary modelState)
        {
            return modelState.ToDictionary(
                k => k.Key,
                v => v.Value?.Errors.Select(x => x.ErrorMessage).ToArray()
            );
        }

        /// <summary>
        /// 序列化為清單
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public static List<KeyValuePair<string, List<string>>> ToSerializedList(this ModelStateDictionary modelState)
        {
            return modelState.Where(m => m.Value?.Errors.Any() == true).Reverse().ToDictionary(
                k => k.Key,
                v => v.Value?.Errors.Select(x => x.ErrorMessage).ToList() ?? new List<string>()
            ).ToList();
        }
    }
}