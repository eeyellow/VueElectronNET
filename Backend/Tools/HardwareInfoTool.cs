using Hardware.Info;
namespace ElectronApp.Tools
{
    /// <summary>
    /// 硬體資訊工具
    /// </summary>
    public class HardwareInfoTool
    {
        private static readonly IHardwareInfo hardwareInfo;

        /// <summary>
        /// static constructor
        /// </summary>
        static HardwareInfoTool()
        {
            // Initialize the hardwareInfo instance
            hardwareInfo = new HardwareInfo();
            hardwareInfo.RefreshAll();
        }

        /// <summary>
        /// 取得主機板序號
        /// </summary>
        /// <returns></returns>
        public static string GetMotherboardSN()
        {
            var motherboard = hardwareInfo.MotherboardList.FirstOrDefault();

            return motherboard.SerialNumber;
        }
    }
}
