using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class Employee
    {
        [Key]
        public Guid EmployeeID { get; set; }

        [Required]
        public string ? Name { get; set; }

        public string ? ContactInfo { get; set; }

        [Required]
        public string ? Role { get; set; }

        [Required]
        [ForeignKey("Department")]
        public Guid DepartmentID { get; set; }
        public Department? Department { get; set; }
        public ICollection<SalaryDetail>? SalaryDetails { get; set; }

        public ICollection<LeaveRequest>? LeaveRequests { get; set; }
        public ICollection<Attendance>? Attendances { get; set; }
    }
}