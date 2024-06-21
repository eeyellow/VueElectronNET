using System.ComponentModel;

namespace ElectronApp.Enums
{
    /// <summary>
    /// Vue模式
    /// </summary>
    public enum VueModeEnum
    {
        /// <summary> 編輯 </summary>
        [Description("編輯")]
        Edit = 1,
        /// <summary> 檢視 </summary>
        [Description("檢視")]
        Detail = 2,
    }
}
