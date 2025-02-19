using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveManagementSystem.Web.Data
{
    /// <summary>
    /// Data Model za rad sa novom tablicom LEAVE_TYPE
    /// </summary> 
    public class LeaveType
    {
        /// <summary>
        /// Primary key: ID
        /// </summary>
        [Key]
        public int Id { get; set; } //drugi način imenovanja: NazivTabliceId (npr. LeaveTypeId)

        /// <summary>
        /// Name of the leave type
        /// </summary>
        [Column(TypeName = "nvarchar(150)")]
        public string Name { get; set; }

        /// <summary>
        /// Number of days that the leave type has
        /// </summary>
        public int NumberOfDays { get; set; }
    }
}
