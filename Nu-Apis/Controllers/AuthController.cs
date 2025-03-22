using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Nu_Apis.Interfaces;
using Nu_BusinessService.Interfaces;
using Nu_Models.DTOs;

namespace Nu_Apis.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IRequestValidationHelpers _requestValidationHelpers;
    private readonly IAccountBusinessService _accountBusinessService;
    

    public AuthController(ILogger<AuthController> logger, IRequestValidationHelpers requestValidationHelpers,
        IAccountBusinessService accountBusinessService)
    {
        _logger = logger;
        _requestValidationHelpers = requestValidationHelpers;
        _accountBusinessService = accountBusinessService;
    }
    
    [HttpPost("login", Name = "login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        return new UnauthorizedResult();
    }
    
    [HttpGet(Name = "user")]
    public IActionResult GetCurrentUser()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return new OkObjectResult(new { username = User.Identity.Name });
        }

        return Ok(new { isAuthenticated = false });
    }

    [HttpPost("register", Name = "register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        bool requestIsValid = _requestValidationHelpers.ValidateRegisterRequest(request);
        
        if (!requestIsValid)
        {
            return new BadRequestResult();
        }
        
        var result = _accountBusinessService.RegisterNewUser(request);
        
        if (!result.Success)
        {
            return new StatusCodeResult(500);
        }
        
        return new OkResult();
    }



}