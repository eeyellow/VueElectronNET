namespace ElectronApp.Areas.DepManage.ViewModels
{
    /// <summary>
    /// fvmList
    /// </summary>
    public class fvmList
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string ParentDepName { get; set; }
        public DateTime EstablishDate { get; set; }
        public string EstablishDateString { get; set; }
    }
}
