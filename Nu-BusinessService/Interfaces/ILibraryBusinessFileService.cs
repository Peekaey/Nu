using Nu_Models;

namespace Nu_BusinessService.Interfaces;

public interface ILibraryBusinessFileService
{
    List<LibraryImageDTO> GetImageCardImageData(List<LibraryImageDTO> libraryFiles);
}