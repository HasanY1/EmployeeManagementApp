using EmployeeManagementApp.DAL.Implementations;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication2.Dal.Implementations;

namespace EmployeeManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestsController : ControllerBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(LeaveRequestsController));

        private readonly IDLeaveRequest _dLeaveRequest;
        private readonly IDEmployee _dEmployee;

        public LeaveRequestsController(IDLeaveRequest dLeaveRequest, IDEmployee dEmployee)
        {
            _dLeaveRequest = dLeaveRequest;
            _dEmployee = dEmployee;
        }

        [HttpPost("createLeaveRequest")]
        public IActionResult CreateLeaveRequest([FromBody] LeaveRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    log.Warn("Invalid ModelState.");
                    return BadRequest(ModelState);
                }

                if (Guid.TryParse(model.EmployeeId, out Guid employeeId) &&
                    DateTime.TryParse(model.LeaveStartDate, out DateTime startDate) &&
                    DateTime.TryParse(model.LeaveEndDate, out DateTime endDate))
                {
                    var newLeaveRequest = new LeaveRequest
                    {
                        EmployeeID = employeeId,
                        LeaveStartDate = startDate,
                        LeaveEndDate = endDate,
                        Status = "pending"
                    };

                    bool? isSuccess = _dLeaveRequest.CreateLeaveRequest(newLeaveRequest);

                    if (isSuccess.HasValue && isSuccess.Value)
                    {
                        log.Info("Leave request created successfully.");
                        return Ok("Leave request created successfully.");
                    }
                    else
                    {
                        log.Error("Failed to create leave request.");
                        return StatusCode(500, "Failed to create leave request.");
                    }
                }
                else
                {
                    log.Error("Invalid input format.");
                    return BadRequest("Invalid input format.");
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while creating leave request: {ex.Message}");
                return StatusCode(500, $"An error occurred while creating leave request: {ex.Message}");
            }
        }

        [HttpDelete("deleteLeaveRequest/{leaveRequestId}")]
        public IActionResult DeleteLeaveRequest(Guid leaveRequestId)
        {
            try
            {
                bool? isSuccess = _dLeaveRequest.DeleteLeaveRequest(leaveRequestId);

                if (isSuccess == true)
                {
                    log.Info("Leave request deleted successfully.");
                    return Ok("Leave request deleted successfully.");
                }
                else if (isSuccess == false)
                {
                    log.Error("Failed to delete leave request.");
                    return StatusCode(500, "Failed to delete leave request.");
                }
                else
                {
                    log.Warn("Leave request not found.");
                    return NotFound("Leave request not found.");
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while deleting leave request: {ex.Message}");
                return StatusCode(500, $"An error occurred while deleting leave request: {ex.Message}");
            }
        }

        [HttpPut("updateLeaveRequest/{leaveRequestId}")]
        public IActionResult UpdateLeaveRequest(Guid leaveRequestId, [FromBody] LeaveRequestUpdateModel model)
        {
            try
            {
                var existingLeaveRequest = _dLeaveRequest.GetLeaveRequestById(leaveRequestId);

                if (existingLeaveRequest != null)
                {
                    if (DateTime.TryParse(model.LeaveStartDate, out DateTime startDate) &&
                        DateTime.TryParse(model.LeaveEndDate, out DateTime endDate))
                    {
                        existingLeaveRequest.LeaveStartDate = startDate;
                        existingLeaveRequest.LeaveEndDate = endDate;
                        existingLeaveRequest.Status = model.Status;

                        bool? isSuccess = _dLeaveRequest.UpdateLeaveRequest(existingLeaveRequest);

                        if (isSuccess == true)
                        {
                            log.Info("Leave request updated successfully.");
                            return Ok("Leave request updated successfully.");
                        }
                        else
                        {
                            log.Error("Failed to update leave request.");
                            return StatusCode(500, "Failed to update leave request.");
                        }
                    }
                    else
                    {
                        log.Error("Invalid date format.");
                        return BadRequest("Invalid date format.");
                    }
                }
                else
                {
                    log.Warn("Leave request not found.");
                    return NotFound("Leave request not found.");
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while updating leave request: {ex.Message}");
                return StatusCode(500, $"An error occurred while updating leave request: {ex.Message}");
            }
        }

        [HttpGet("viewAllLeaveRequests")]
        public IActionResult ViewAllLeaveRequests()
        {
            try
            {
                List<LeaveRequest>? leaveRequests = _dLeaveRequest.ViewAllLeaveRequests();

                if (leaveRequests != null && leaveRequests.Count > 0)
                {
                    log.Info("Retrieved all leave requests.");
                    return Ok(leaveRequests);
                }
                else
                {
                    log.Warn("No leave requests found.");
                    return NotFound("No leave requests found.");
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while fetching leave requests: {ex.Message}");
                return StatusCode(500, $"An error occurred while fetching leave requests: {ex.Message}");
            }
        }

        [HttpGet("viewLeaveRequestsByEmployeeId/{employeeId}")]
        public IActionResult ViewLeaveRequestsByEmployeeId(string employeeId)
        {
            if (Guid.TryParse(employeeId, out Guid parsedEmployeeId))
            {
                try
                {
                    var leaveRequests = _dLeaveRequest.GetLeaveRequestsByEmployeeId(parsedEmployeeId);

                    if (leaveRequests != null && leaveRequests.Any())
                    {
                        log.Info($"Retrieved leave requests for Employee ID: {employeeId}");
                        return Ok(leaveRequests);
                    }
                    else
                    {
                        log.Warn("No leave requests found for this employee.");
                        return NotFound("No leave requests found for this employee.");
                    }
                }
                catch (Exception ex)
                {
                    log.Error($"An error occurred while fetching leave requests: {ex.Message}");
                    return StatusCode(500, $"An error occurred while fetching leave requests: {ex.Message}");
                }
            }
            else
            {
                log.Error("Invalid employee ID.");
                return BadRequest("Invalid employee ID.");
            }
        }

        [HttpGet("viewLeaveRequestsInManagerDepartment/{managerId}")]
        public IActionResult ViewLeaveRequestsInManagerDepartment(string managerId)
        {
            if (Guid.TryParse(managerId, out Guid parsedManagerId))
            {
                try
                {
                    var manager = _dEmployee.GetEmployeeById(parsedManagerId);

                    if (manager != null && manager.Role == "manager")
                    {
                        var leaveRequestsInManagerDepartment = _dLeaveRequest.GetLeaveRequestsByDepartmentId(manager.DepartmentID);

                        if (leaveRequestsInManagerDepartment != null && leaveRequestsInManagerDepartment.Any())
                        {
                            log.Info($"Retrieved leave requests in Manager's Department ID: {manager.DepartmentID}");
                            return Ok(leaveRequestsInManagerDepartment);
                        }
                        else
                        {
                            log.Warn("No leave requests found in the manager's department.");
                            return NotFound("No leave requests found in the manager's department.");
                        }
                    }
                    else
                    {
                        log.Error("The provided ID does not belong to a manager.");
                        return BadRequest("The provided ID does not belong to a manager.");
                    }
                }
                catch (Exception ex)
                {
                    log.Error($"An error occurred while fetching leave requests: {ex.Message}");
                    return StatusCode(500, $"An error occurred while fetching leave requests: {ex.Message}");
                }
            }
            else
            {
                log.Error("Invalid manager ID.");
                return BadRequest("Invalid manager ID.");
            }
        }

        public class LeaveRequestModel
        {
            public string? EmployeeId { get; set; }
            public string? LeaveStartDate { get; set; }
            public string? LeaveEndDate { get; set; }
            public string? Status { get; set; }
        }

        public class LeaveRequestUpdateModel
        {
            public string? LeaveStartDate { get; set; }
            public string? LeaveEndDate { get; set; }
            public string? Status { get; set; }
        }
    }
}
