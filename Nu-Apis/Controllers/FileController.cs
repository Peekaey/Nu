using Microsoft.AspNetCore.Mvc;
using Nu_Apis.Interfaces;
using Nu_BusinessService.Interfaces;

namespace Nu_Apis.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class FileController
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
    

    
}