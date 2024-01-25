using Microsoft.EntityFrameworkCore;
namespace FinalProject.Models
{
    public class EmployeeManagementDbContext : DbContext
    {
        public DbSet<Employee> ? Employees { get; set; }
        public DbSet<Department> ? Departments { get; set; }
        public DbSet<SalaryDetail> ? SalaryDetails { get; set; }
        public DbSet<LeaveRequest> ? LeaveRequests { get; set; }
        public DbSet<Attendance> ? Attendances { get; set; }
        public DbSet<User> ? Users { get; set; }
        public EmployeeManagementDbContext(DbContextOptions<EmployeeManagementDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentID);

            modelBuilder.Entity<Employee>()
                   .HasMany(e => e.SalaryDetails)
                   .WithOne(sd => sd.Employee)
                   .HasForeignKey(sd => sd.EmployeeID)
                   .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Department>()
                .HasMany(d => d.Employees)
                .WithOne(e => e.Department)
                .HasForeignKey(e => e.DepartmentID);

            modelBuilder.Entity<SalaryDetail>()
                .HasKey(sd => sd.SalaryID);

            modelBuilder.Entity<LeaveRequest>()
                .HasKey(lr => lr.LeaveRequestID);

            modelBuilder.Entity<Attendance>()
                .HasKey(a => a.AttendanceID);

            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=127.0.0.1;database=test;uid=root;pwd=Hasan123123;");
        }
    }
}