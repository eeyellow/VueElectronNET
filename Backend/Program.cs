using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using Hangfire.Storage.SQLite;
using ElectronNET.API;
using ElectronNET.API.Entities;
using ElectronApp.Tools;
using ElectronApp.Database.Contexts;

namespace ElectronApp
{
    /// <summary>
    /// 主程式類別
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 主程式進入點
        /// </summary>
        /// <param name="args">命令列參數</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(
                    builder => builder.RegisterModule(new AutofacModule())
                );
            // Add services to the container.
            builder.Services
                .AddControllersWithViews()
                .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null)
                .AddRazorRuntimeCompilation();

            if (builder.Environment.EnvironmentName == "Production")
            {
                builder.WebHost.UseElectron(args);
                builder.Services.AddElectron();
            }

            var cfg = new ConfigurationBuilder()
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            var dataStorePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                $"{cfg.GetSection("AppName")?.Value ?? "LinkChainElectronApp"}"
            );
            Directory.CreateDirectory(dataStorePath);
            var mainDBConnStr = $"Data Source={Path.Combine(dataStorePath, cfg.GetConnectionString("MainSqlite") ?? "MainSqlite.db")}";
            var taskDBPath = Path.Combine(dataStorePath, cfg.GetConnectionString("TaskSqlite") ?? "TaskSqlite.db");

            // Add the main database context to the DI container
            builder.Services
                .AddDbContext<DatabaseContext>(options =>
                {
                    options.UseSqlite(mainDBConnStr);
                });

            // Add Hangfire services.
            builder.Services
                .AddHangfire(configuration => configuration
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSQLiteStorage(taskDBPath)
                );
            
            // Add the processing server as IHostedService
            builder.Services
                .AddHangfireServer();

            builder.Services.AddCors(options =>
            {
                // CORS全開，建議只用在Electron模式
                options.AddPolicy("FrontendPolicy", policy =>
                {
                    policy.AllowAnyHeader()
                          .AllowAnyMethod()
                          .SetIsOriginAllowed(origin => true)
                          .AllowCredentials();
                });
            });

            //=====================================================================
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // Electron模式下不啟用HSTS
                // app.UseHsts();
            }

            // Electron模式下不啟用Https
            // app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseHangfireDashboard();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<DatabaseContext>();
                //確認資料庫建立
                db?.Database.EnsureCreated();
            }

            // 測試Hangfire
            // RecurringJob.AddOrUpdate("測試用Job", () => Console.Write("測試成功!"), Cron.Minutely);

            if (builder.Environment.EnvironmentName == "Production")
            {
                ElectronEntryPoint.Bootstrap(builder.Environment);
            }
            else
            {
                // 自動啟動 TypeTransferExtension
                TypeTransferExtension.Generate(Path.Combine(app.Environment.ContentRootPath, "wwwroot", "js"));
            }

            WebHostEnvironmentManager.WebRootPath = app.Environment.WebRootPath;
            WebHostEnvironmentManager.ContentRootPath = app.Environment.ContentRootPath;
            WebHostEnvironmentManager.EnvironmentName = app.Environment.EnvironmentName;

            EncryptTool.SetEncryptKey("LC@ElectronApp");

            app.Run();
        }
    }

    /// <summary>
    /// Electron進入點
    /// </summary>
    public static class ElectronEntryPoint
    {
        /// <summary>
        /// 表示是否為雙擊事件的標記
        /// </summary>
        public static bool _isDoubleClickEvent = false;
        /// <summary>
        /// 啟動Electron介面
        /// </summary>
        public async static void Bootstrap(IWebHostEnvironment env)
        {
            var display = await Electron.Screen.GetPrimaryDisplayAsync();
            var maxiSize = display.WorkAreaSize;
            var option = new BrowserWindowOptions
            {
                Title = "單機應用程式",
                Center = true,
                Resizable = false,
                Height = maxiSize.Height,
                Width = maxiSize.Width,
                Movable = false,
                AcceptFirstMouse = true,
                Closable = false,
                Show = false,
                WebPreferences = new WebPreferences
                {
                    //DevTools = env.EnvironmentName != "Production",
                    DevTools = true,
                    NodeIntegration = true,
                    NodeIntegrationInWorker = true,
                    EnableRemoteModule = true,
                    ContextIsolation = false,
                    DefaultEncoding = "UTF-8",
                    //Preload = Path.Combine(env.WebRootPath, "js/preload.js")
                }
            };

            // 主視窗程式
            var browserWindow = await Electron.WindowManager.CreateWindowAsync(option);

            if (HybridSupport.IsElectronActive && Electron.Tray.MenuItems.Count == 0)
            {
                var menu = new MenuItem[] {
                    new MenuItem {
                        Label = "Exit",
                        Click = () => {
                            Electron.Tray.Destroy();
                            Electron.WindowManager.BrowserWindows.ToList().ForEach(w => w.Close());
                            Electron.App.Exit();
                        }
                    }
                };

                await Electron.Tray.Show(Path.Combine(env.WebRootPath, "images/medicine.png"), menu);
                await Electron.Tray.SetToolTip("Contact Management");

                Electron.Tray.OnClick += async (args, rectangle) => await SingleClickAsync();
                Electron.Tray.OnDoubleClick += async (args, rectangle) => await DoubleClickAsync();
            }

            //if (env.EnvironmentName == "Production")
            //{
            //    browserWindow.RemoveMenu();
            //}

            await browserWindow.WebContents.Session.ClearCacheAsync();

            browserWindow.OnMinimize += () =>
            {
                browserWindow.Hide();
                return;
            };
        }

        /// <summary>
        /// 單擊事件的處理方法
        /// </summary>
        /// <returns></returns>
        public static async Task SingleClickAsync()
        {
            await Task.Delay(200);

            if (_isDoubleClickEvent)
            {
                return;
            }
        }

        /// <summary>
        /// 雙擊事件的處理方法
        /// </summary>
        /// <returns></returns>
        private static async Task DoubleClickAsync()
        {
            _isDoubleClickEvent = true;

            await Task.Delay(215);
            _isDoubleClickEvent = false;

            // Put your code here
            Electron.WindowManager.BrowserWindows.First().Show();
        }
    }
}
