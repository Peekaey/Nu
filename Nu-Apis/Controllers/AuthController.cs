using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Nu_Apis.Interfaces;
using Nu_BusinessService.Interfaces;
using Nu_Models;
using Nu_Models.DTOs;
using Nu_Models.Enums;

namespace Nu_Apis.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IApiRequestValidationHelpers _apiRequestValidationHelpers;
    private readonly IAccountBusinessService _accountBusinessService;
    

    public AuthController(ILogger<AuthController> logger, IApiRequestValidationHelpers apiRequestValidationHelpers,
        IAccountBusinessService accountBusinessService)
    {
        _logger = logger;
        _apiRequestValidationHelpers = apiRequestValidationHelpers;
        _accountBusinessService = accountBusinessService;
    }
    
    [HttpPost("login", Name = "login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return new BadRequestResult();
        }
        bool requestValid = _apiRequestValidationHelpers.ValidateLoginRequest(request);
        
        if (!requestValid)
        {
            return new BadRequestResult();
        }
        
        var authResult = _accountBusinessService.AuthenticateUser(request);
        
        if (authResult.AuthenticationResultType == AuthenticationResultType.RequestFailure)
        {
            return new StatusCodeResult(500);
        }
        
        if (authResult.AuthenticationResultType == AuthenticationResultType.InvalidCredentials)
        {
            return new UnauthorizedResult();
        }
        // TODO Find Common Declaration for Expires with JwTokenHelper
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false, // Set to true in production
            SameSite = SameSiteMode.Lax, // Set to SameSiteMode.Strict in production
            Path = "/", // Ensures cookie is sent on all requests within the domain
            Expires = DateTime.UtcNow.AddHours(1)
        };

        Response.Cookies.Append("Nu_JWToken", authResult.Cookie , cookieOptions);
        return new OkResult();
    }

    [Authorize]
    [HttpGet("getcurrentuser", Name = "getcurrentuser")]
    public IActionResult GetCurrentUser()
    {
        return new OkObjectResult("You have authenticated successfully.");
    }
    

    [HttpPost("register", Name = "register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return new BadRequestResult();
        }
        bool requestValid = _apiRequestValidationHelpers.ValidateRegisterRequest(request);
        
        if (!requestValid)
        {
            return new BadRequestResult();
        }
        
        var accountExists = _accountBusinessService.DoesAccountExist(request.Email);
        
        if (accountExists)
        {
            return new ConflictResult();
        }
        
        var result = _accountBusinessService.RegisterNewUser(request);
        
        if (!result.Success)
        {
            return new StatusCodeResult(500);
        }
        
        return new OkResult();
    }



}