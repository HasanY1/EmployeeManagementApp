using FinalProject.Models;
using log4net;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementApp.DAL.Implementations
{
    public class DLeaveRequest : IDLeaveRequest
    {
        private readonly EmployeeManagementDbContext _context;
        private static readonly ILog log = LogManager.GetLogger(typeof(DLeaveRequest));

        public DLeaveRequest(EmployeeManagementDbContext context)
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

        public bool? CreateLeaveRequest(LeaveRequest? newLeaveRequest)
        {
            try
            {
                if (newLeaveRequest != null)
                {
                    _context.LeaveRequests?.Add(newLeaveRequest);
                    _context.SaveChanges();
                    log.Info("Leave request created successfully.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while creating leave request: {ex.Message}");
                return false;
            }
            return false;
        }

        public bool? DeleteLeaveRequest(Guid? leaveRequestId)
        {
            try
            {
                var leaveRequestToDelete = _context.LeaveRequests?.Find(leaveRequestId);
                if (leaveRequestToDelete != null)
                {
                    _context.LeaveRequests?.Remove(leaveRequestToDelete);
                    _context.SaveChanges();
                    log.Info("Leave request deleted successfully.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while deleting leave request: {ex.Message}");
                return false;
            }
            return false;
        }

        public bool? UpdateLeaveRequest(LeaveRequest? updatedLeaveRequest)
        {
            try
            {
                if (updatedLeaveRequest != null)
                {
                    _context.LeaveRequests?.Update(updatedLeaveRequest);
                    _context.SaveChanges();
                    log.Info("Leave request updated successfully.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while updating leave request: {ex.Message}");
                return false;
            }
            return false;
        }

        public LeaveRequest? GetLeaveRequestById(Guid? leaveRequestId)
        {
            try
            {
                if (leaveRequestId != null)
                {
                    var leaveRequest = _context.LeaveRequests?.FirstOrDefault(lr => lr.LeaveRequestID == leaveRequestId);
                    return leaveRequest;
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while getting leave request by id: {ex.Message}");
                return null;
            }
            return null;
        }

        public List<LeaveRequest>? ViewAllLeaveRequests()
        {
            try
            {
                var leaveRequests = _context.LeaveRequests?.ToList();
                return leaveRequests ?? new List<LeaveRequest>();
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while getting all leave requests: {ex.Message}");
                return null;
            }
        }

        public List<LeaveRequest>? GetLeaveRequestsByEmployeeId(Guid? employeeId)
        {
            try
            {
                if (employeeId != null)
                {
                    return _context.LeaveRequests?
                        .Where(lr => lr.EmployeeID == employeeId)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while getting all leave requests by employee id: {ex.Message}");
                return null;
            }
            return null;
        }

        public List<LeaveRequest>? GetLeaveRequestsByDepartmentId(Guid? departmentId)
        {
            try
            {
                if (departmentId != null)
                {
                    return _context.LeaveRequests?
                        .Include(lr => lr.Employee)
                        .Where(lr => lr.Employee != null && lr.Employee.DepartmentID == departmentId)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while getting all leave requests by in that department id: {ex.Message}");
                return null;
            }
            return null;
        }
    }
}
