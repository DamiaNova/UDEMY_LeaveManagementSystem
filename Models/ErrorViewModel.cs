namespace LeaveManagementSystem.Web.Models
{
    /// <summary>
    /// ViewModel klasa
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Public property (tip: nullable string) za ID requesta
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// Public property koji vraća TRUE ako je property RequestId popunjen s vrijednošću
        /// </summary>
        public bool ShowRequestId => !(string.IsNullOrEmpty(RequestId));
    }
}
