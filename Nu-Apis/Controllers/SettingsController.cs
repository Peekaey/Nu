
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nu_Apis.Interfaces;
using Nu_BusinessService.Interfaces;

namespace Nu_Apis.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class SettingsController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IApiRequestValidationHelpers _apiRequestValidationHelpers;
    private readonly ILibraryBusinessService _libraryBusinessService;
    
    public SettingsController(ILogger<AuthController> logger, IApiRequestValidationHelpers apiRequestValidationHelpers,
        ILibraryBusinessService libraryBusinessService)
    {
        _logger = logger;
        _apiRequestValidationHelpers = apiRequestValidationHelpers;
        _libraryBusinessService = libraryBusinessService;
    }

    [HttpGet("rootfolderpath")]
    public IActionResult GetRootFolderPath()
    {
        var rootFolderPath = _libraryBusinessService.GetRootFolderPath();
        return Ok(rootFolderPath);
        
    }
    
}