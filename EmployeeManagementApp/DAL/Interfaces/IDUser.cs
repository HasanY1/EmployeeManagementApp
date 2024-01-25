using FinalProject.Models;

namespace WebApplication2.Dal.Implementations
{
    public interface IDUser
    {
        public bool? CreateUser(User? newUser);
        public Employee? GetEmployeeByUsername(string? username);
        public int LoginUser(string username, string password);
    }
}