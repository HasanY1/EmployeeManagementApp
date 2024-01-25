using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace FinalProject.Models
{
    public class LeaveRequest
    {
        [Key]
        public Guid LeaveRequestID { get; set; }

        [Required]
        [ForeignKey("Employee")]
        public Guid EmployeeID { get; set; }
        public Employee ? Employee { get; set; }

        [Required]
        public DateTime LeaveStartDate { get; set; }

        [Required]
        public DateTime LeaveEndDate { get; set; }

        public string ? Status { get; set; }
    }
}
