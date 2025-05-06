
using Microsoft.AspNetCore.Mvc;
using Nu_Apis.Interfaces;
using Nu_BusinessService.Interfaces;

namespace Nu_Apis.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class FileController : ControllerBase
{
    private readonly ILogger<FileController> _logger;
    private readonly IApiRequestValidationHelpers _apiRequestValidationHelpers;
    private readonly ILibraryBusinessFileService _libraryBusinessFileService;

    public FileController(ILogger<FileController> logger, IApiRequestValidationHelpers apiRequestValidationHelpers,
        ILibraryBusinessFileService libraryBusinessFileService)
    {
        _logger = logger;
        _apiRequestValidationHelpers = apiRequestValidationHelpers;
        _libraryBusinessFileService = libraryBusinessFileService;
    }

    // TODO Add Endpoint Verification
    [HttpGet("image/{id}")]
    public IActionResult GetOriginalImage(int id)
    {
        var file = _libraryBusinessFileService.GetImagePathById(id);

        if (!file.Success)
        {
            if (file.StatusCode == 404)
            {
                return NotFound(file.ErrorMessage);
            }
            else
            {
                return StatusCode(500, "Internal error retrieving image");
            }
        }
        return Ok(file.Data);
    }

    [HttpGet("download/{id}")]
    public IActionResult DownloadFile(int id)
    {
        var file = _libraryBusinessFileService.DownloadFile(id);
        
        if (!file.Success)
        {
            if (file.StatusCode == 404)
            {
                return NotFound(file.ErrorMessage);
            }
            else
            {
                return StatusCode(500, "Internal error retrieving file");
            }
        }
        return Ok(file.Data);
    }
}