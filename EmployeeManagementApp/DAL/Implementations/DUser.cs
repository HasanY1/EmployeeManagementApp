using FinalProject.Models;
using log4net;
using System.Security.Cryptography;
using System.Text;

namespace WebApplication2.Dal.Implementations
{
    public class DUser : IDUser
    {
        private readonly EmployeeManagementDbContext _context;
        private static readonly ILog log = LogManager.GetLogger(typeof(DUser));
        public DUser(EmployeeManagementDbContext context)
        {
            _context = context;
        }

        public bool? CreateUser(User? newUser)
        {
            try
            {
                if (newUser != null)
                {
                    _context.Users?.Add(newUser);
                    _context.SaveChanges();
                    log.Info("User created successfully.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while creating user: {ex.Message}");
                return false;
            }
            return false;
        }
        public Employee? GetEmployeeByUsername(string? username)
        {
            try
            {
                if (Guid.TryParse(username, out Guid employeeId))
                {
                    log.Info("Employee found by username.");
                    return _context.Employees?.FirstOrDefault(e => e.EmployeeID == employeeId);
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while getting user: {ex.Message}");
                return null;
            }
            return null;
        }
        public int LoginUser(string username, string password)
        {
            try
            {
                string hashedPassword = HashPassword(password);
                var user = _context.Users?.FirstOrDefault(u => u.Username == username && u.Password == hashedPassword);

                if (user != null)
                {
                    if (Guid.TryParse(username, out Guid employeeId))
                    {
                        if (_context.Employees != null)
                        {
                            var employee = _context.Employees.FirstOrDefault(e => e.EmployeeID == employeeId);

                            if (employee != null && employee.Role != null)
                            {
                                log.Info("User logged in successfully.");
                                return employee.Role.ToLower() switch
                                {
                                    "admin" => 1,
                                    "manager" => 2,
                                    "employee" => 3,
                                    _ => 0
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while logging in: {ex.Message}");
                return 0;
            }
            return 0;
        }
        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}