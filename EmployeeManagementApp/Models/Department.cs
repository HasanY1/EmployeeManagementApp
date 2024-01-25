using System.ComponentModel.DataAnnotations;
namespace FinalProject.Models
{
    public class Department
    {
        [Key]
        public Guid DepartmentID { get; set; }

        [Required]
        public string ? DepartmentName { get; set; }

        public ICollection<Employee> ? Employees { get; set; }
    }
}
