namespace ElectronApp.ViewModels
{
    /// <summary>
    /// Represents the view model for displaying error information.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Gets or sets the request ID.
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// Gets a value indicating whether the request ID should be shown.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
