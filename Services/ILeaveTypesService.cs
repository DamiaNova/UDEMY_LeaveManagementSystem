using LeaveManagementSystem.Web.Models.LeaveTypes;

namespace LeaveManagementSystem.Web.Services
{
    /// <summary>
    /// Automatikom generiran Interface (contract) za Service klasu
    /// </summary>
    public interface ILeaveTypesService
    {
        Task CreateRecordInDatabase(LeaveTypeCreateVM viewModelRecord);
        Task DeteleRecordAsync(int id);
        Task<IEnumerable<LeaveTypeReadOnlyVM>> GetAllRecords();
        Task<T?> GetRecordAsync<T>(int id) where T : class;
        Task UpdateRecordInDatabase(LeaveTypeEditVM viewModelRecord);
    }
}