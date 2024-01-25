using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Dal.Implementations;
using log4net;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IDUser _dUser;
    private static readonly ILog log = LogManager.GetLogger(typeof(UserController));

    public UserController(IDUser dUser)
    {
        _dUser = dUser;
    }

    [HttpPost("login")]
    public IActionResult LoginUser(LoginModel model)
    {
        try
        {
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Username or password cannot be null or empty. Please provide valid input.");
            }

            int roleNumber = _dUser.LoginUser(model.Username, model.Password);

            if (roleNumber != 0)
            {
                log.Info($"User '{model.Username}' logged in with role '{roleNumber}'.");
                return Ok(new { Role = roleNumber });
            }
            else
            {
                log.Warn($"Invalid credentials for user '{model.Username}'.");
                return Unauthorized("Invalid credentials.");
            }
        }
        catch (Exception ex)
        {
            log.Error($"An error occurred during user login: {ex.Message}");
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpPost("register")]
    public IActionResult RegisterUser(LoginModel model)
    {
        try
        {
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Username or password cannot be null or empty. Please provide valid input.");
            }

            var existingEmployee = _dUser.GetEmployeeByUsername(model.Username);

            if (existingEmployee == null)
            {
                var newUser = new User
                {
                    Username = model.Username,
                    Password = model.Password,
                    Role = "employee"
                };

                bool? isSuccess = _dUser.CreateUser(newUser);

                if (isSuccess.HasValue && isSuccess.Value)
                {
                    log.Info($"User '{model.Username}' registered successfully.");
                    return Ok("User registered successfully.");
                }
                else
                {
                    log.Error($"Failed to register user '{model.Username}'.");
                    return StatusCode(500, "Failed to register user.");
                }
            }
            else
            {
                log.Warn($"Employee ID '{model.Username}' already exists in the system.");
                return BadRequest("Employee ID already exists in the system.");
            }
        }
        catch (Exception ex)
        {
            log.Error($"An error occurred during user registration: {ex.Message}");
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
}

public class LoginModel
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}
