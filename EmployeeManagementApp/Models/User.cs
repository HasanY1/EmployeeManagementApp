using System.ComponentModel.DataAnnotations;
namespace FinalProject.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        public string ? Username { get; set; }

        [Required]
        public string ? Password { get; set; }

        [Required]
        public string ? Role { get; set; }
    }
}
