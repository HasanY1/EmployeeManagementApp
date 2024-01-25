using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace FinalProject.Models
{
    public class Attendance
    {
        [Key]
        public Guid AttendanceID { get; set; }

        [Required]
        [ForeignKey("Employee")]
        public Guid EmployeeID { get; set; }
        public Employee ? Employee { get; set; }

        [Required]
        public DateTime AttendanceDate { get; set; }

        public TimeSpan TimeIn { get; set; }
        public TimeSpan TimeOut { get; set; }
    }
}
