using FinalProject.Models;

namespace EmployeeManagementApp.DAL.Implementations
{
    public interface IDAttendance
    {
        bool? DeleteAttendance(Guid? attendanceId);
        List<Attendance>? GetAttendanceByEmployeeId(Guid? employeeId);
        Attendance? GetAttendanceById(Guid? attendanceId);
        Task<bool> MarkAttendance(Attendance? newAttendance);
        bool? UpdateAttendance(Attendance? updatedAttendance);
        List<Attendance>? ViewAllAttendance();
    }
}