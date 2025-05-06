
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
    private readonly ISettingsBusinessService _settingsBusinessService;
    
    public SettingsController(ILogger<AuthController> logger, IApiRequestValidationHelpers apiRequestValidationHelpers,
        ISettingsBusinessService settingsBusinessService)
    {
        _logger = logger;
        _apiRequestValidationHelpers = apiRequestValidationHelpers;
        _settingsBusinessService = settingsBusinessService;
    }

    [HttpGet("rootfolderpath")]
    public IActionResult GetRootFolderPath()
    {
        var rootFolderPath = _settingsBusinessService.GetRootFolderPath();
        return Ok(rootFolderPath);
    }
    
}