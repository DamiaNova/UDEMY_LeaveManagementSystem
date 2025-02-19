namespace LeaveManagementSystem.Web.Models.LeaveTypes
{
    /// <summary>
    /// Klasa koja se koristi za kreiranje slogova za INSERT u tablicu LeaveTypes na bazi
    /// </summary>
    public class LeaveTypeCreateVM
    {
        #region Properties
        /// <summary>
        /// Name of the leave type
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Number of days that the leave type has
        /// </summary>
        public int NumberOfDays { get; set; }
        #endregion
    }
}
