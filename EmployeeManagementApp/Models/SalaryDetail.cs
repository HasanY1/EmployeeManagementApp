using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace FinalProject.Models
{
    public class SalaryDetail
    {
        [Key]
        public Guid SalaryID { get; set; }

        [Required]
        public Guid EmployeeID { get; set; }

        [ForeignKey("EmployeeID")]
        public Employee ? Employee { get; set; }

        [Required]
        public decimal BasicSalary { get; set; }
        public decimal Bonuses { get; set; }
        public decimal Deductions { get; set; }
    }

}
