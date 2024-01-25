using FinalProject.Models;
using log4net;

namespace EmployeeManagementApp.DAL.Implementations
{
    public class DAttendance : IDAttendance
    {
        private readonly EmployeeManagementDbContext _context;
        private static readonly ILog log = LogManager.GetLogger(typeof(DAttendance));

        public DAttendance(EmployeeManagementDbContext context)
        {
            _context = context;
        }

        public bool? DeleteAttendance(Guid? attendanceId)
        {
            try
            {
                var attendanceToDelete = _context.Attendances?.Find(attendanceId);
                if (attendanceToDelete != null)
                {
                    _context.Attendances?.Remove(attendanceToDelete);
                    _context.SaveChanges();
                    log.Info("Attendance deleted successfully.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while deleting attendance: {ex.Message}");
                return false;
            }
            return false;
        }

        public bool? UpdateAttendance(Attendance? updatedAttendance)
        {
            try
            {
                if (updatedAttendance != null)
                {
                    _context.Attendances?.Update(updatedAttendance);
                    _context.SaveChanges();
                    log.Info("Attendance updated successfully.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while updating attendance: {ex.Message}");
                return false;
            }
            return false;
        }

        public async Task <bool>  MarkAttendance(Attendance? newAttendance)
        {
            try
            {
                if (newAttendance != null)
                {
                    _context.Attendances?.Add(newAttendance);
                    await _context.SaveChangesAsync();
                    log.Info("Attendance marked successfully.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while marking attendance: {ex.Message}");
                return false;
            }
            return false;
        }

        public Attendance? GetAttendanceById(Guid? attendanceId)
        {
            try
            {
                var attendance = _context.Attendances?.FirstOrDefault(a => a.AttendanceID == attendanceId);
                return attendance;
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while retrieving attendance: {ex.Message}");
            }
            return null;
        }

        public List<Attendance>? ViewAllAttendance()
        {
            try
            {
                var attendances = _context.Attendances?.ToList();
                return attendances ?? new List<Attendance>();
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while viewing all attendance: {ex.Message}");
            }
            return null;
        }

        public List<Attendance>? GetAttendanceByEmployeeId(Guid? employeeId)
        {
            try
            {
                if (_context.Attendances != null)
                {
                    return _context.Attendances.Where(a => a.EmployeeID == employeeId).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while retrieving attendance by employee ID: {ex.Message}");
            }
            return null;
        }
    }
}
