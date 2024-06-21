using System.ComponentModel;

namespace ElectronApp.Enums
{
    /// <summary>
    /// 檔案上傳類型
    /// 注意: 為避免使用者透過改ID即可下載其他檔案
    /// 此處的Enum需分類為
    /// 1.可供訪客下載 
    /// 2.需登入後才可下載
    /// 並於 UploadFileController/Download 進行簡易判斷
    /// </summary>
    public enum UploadFileRefTypeEnum
    {
        /// <summary>部門管理</summary>
        [Description("部門管理")]
        Department = 1,
        
    }
}
