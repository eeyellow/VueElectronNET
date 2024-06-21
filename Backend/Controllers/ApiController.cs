using ElectronApp.Database.Contexts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectronApp.Controllers
{
    /// <summary>
    /// API
    /// </summary>
    public class ApiController : BaseController
    {
        private readonly ILogger<ApiController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly DatabaseContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="logger">The logger</param>
        /// <param name="webHostEnvironment"></param>
        /// <param name="context">The database context</param>
        public ApiController(
            IConfiguration configuration,
            ILogger<ApiController> logger,
            IWebHostEnvironment webHostEnvironment,
            DatabaseContext context) : base(configuration)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        [EnableCors("FrontendPolicy")]
        public async Task<JsonResult> Basic()
        {
            var deps = await _context.Departments
                .Where(a => a.IsDelete == 0).ToListAsync();

            return Json(deps);
        }
    }
}
