using ElectronApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace ElectronApp.Tools
{
    /// <summary>
    /// Router工具
    /// </summary>
    public static class RouterTool
    {
        /// <summary>
        /// 取得Areas
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAreas()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            var controlleractionlist = asm.GetTypes()
                    .Where(type => typeof(Controller).IsAssignableFrom(type))
                    .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                    .Select(x => new
                    {
                        ControllerDesc = x.DeclaringType.GetSummary(),
                        Area = x.DeclaringType.CustomAttributes.Where(c => c.AttributeType == typeof(AreaAttribute))

                    }).ToList();
            var list = new List<string>();
            foreach (var item in controlleractionlist)
            {
                if (item.Area.Count() != 0)
                {
                    var area = item.Area.Select(v => v.ConstructorArguments[0].Value.ToString()).FirstOrDefault();
                    list.Add(item.ControllerDesc + "--" + area);
                }
            }
            return list.Distinct().ToList();
        }
    }
}
