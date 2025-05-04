
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
    private readonly ILibraryBusinessService _libraryBusinessService;

    public FileController(ILogger<FileController> logger, IApiRequestValidationHelpers apiRequestValidationHelpers,
        ILibraryBusinessService libraryBusinessService)
    {
        _logger = logger;
        _apiRequestValidationHelpers = apiRequestValidationHelpers;
        _libraryBusinessService = libraryBusinessService;
    }

    [HttpGet("image/{id}")]
    public IActionResult GetOriginalImage(int id)
    {
        var file = _libraryBusinessService.GetImageById(id);
        return Ok(file);
    }

    [HttpGet("download/{id}")]
    public IActionResult DownloadFile(int id)
    {
        try
        {
            return _libraryBusinessService.DownloadFile(id);
        }
        catch (FileNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading file {Id}", id);
            return StatusCode(500, "Error downloading file");
        }
    }
}