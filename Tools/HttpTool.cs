using ElectronApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ElectronApp.Tools
{
    /// <summary>
    /// Http工具
    /// </summary>
    public static class HttpTool
    {
        /// <summary>
        /// 建立Response
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static ObjectResult CreateResponse(StatusResultViewModel result)
        {
            return new ObjectResult(result) { StatusCode = result.StatusCode };
        }

        /// <summary>
        /// 建立Response
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static ObjectResult CreateResponse(ModelStateViewModel result)
        {
            return new ObjectResult(result) { StatusCode = result.StatusCode };
        }
    }
}
