using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nu_Apis.Interfaces;

namespace Nu_Apis.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class LibraryController :ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IApiRequestValidationHelpers _apiRequestValidationHelpers;
    
    public LibraryController(ILogger<AuthController> logger, IApiRequestValidationHelpers apiRequestValidationHelpers)
    {
        _logger = logger;
        _apiRequestValidationHelpers = apiRequestValidationHelpers;
    }
    
    
}