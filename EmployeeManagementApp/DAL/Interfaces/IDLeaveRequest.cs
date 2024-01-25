using FinalProject.Models;

namespace EmployeeManagementApp.DAL.Implementations
{
    public interface IDLeaveRequest
    {
        public bool? CreateLeaveRequest(LeaveRequest? newLeaveRequest);
        public bool? DeleteAttendance(Guid? attendanceId);
        public bool? DeleteLeaveRequest(Guid? leaveRequestId);
        public LeaveRequest? GetLeaveRequestById(Guid? leaveRequestId);
        public List<LeaveRequest>? GetLeaveRequestsByDepartmentId(Guid? departmentId);
        public List<LeaveRequest>? GetLeaveRequestsByEmployeeId(Guid? employeeId);
        public bool? UpdateLeaveRequest(LeaveRequest? updatedLeaveRequest);
        public List<LeaveRequest>? ViewAllLeaveRequests();
    }
}