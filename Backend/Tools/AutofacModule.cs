using Autofac;
using System.Text.RegularExpressions;
using System.Reflection;

namespace ElectronApp.Tools
{
    /// <summary>
    /// Autofac Module
    /// </summary>
    public class AutofacModule : Autofac.Module
    {
        /// <summary>
        /// 註冊 至 Container
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            //取得目前正在執行之程式碼的組件
            var assembly = Assembly.GetExecutingAssembly();
            //取得目標Type
            var types = assembly.GetTypes()
                .Where(t => !t.IsInterface)
                .Where(t => !t.IsAbstract)
                .ToList();

            CommonRegister(builder, types, "Service");
            CommonRegister(builder, types, "Repository");
        }
    
        private static void CommonRegister(ContainerBuilder builder, List<Type> types, string pattern)
        {
            var filterTypes = types.Where(t => 
                Regex.IsMatch(t.Name, pattern + "$|" + pattern + "`[0-9]+$")
            );

            //泛型類別與一般類別的注入方法不同
            foreach (var type in filterTypes)
            {
                if (type.IsGenericType)
                {
                    builder.RegisterGeneric(type)
                           .AsImplementedInterfaces()
                           .InstancePerLifetimeScope();
                }
                else
                {
                    builder.RegisterType(type)
                           .AsImplementedInterfaces()
                           .InstancePerLifetimeScope();
                }
            }
        }
    }
}
