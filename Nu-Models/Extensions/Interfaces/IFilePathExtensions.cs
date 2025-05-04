using Nu_Models.DatabaseModels;

namespace Nu_Models.Extensions.Interfaces;

public interface IFilePathExtensions
{
    string MapImageUrlToServerUrl(LibraryFileIndex libraryFile);
    string MapImageUrlToServerUrl(string libraryFile);
    List<string> MapImageUrlsToServerUrls(List<LibraryFileIndex> libraryFiles);
    List<string> MapImageUrlsToServerUrls(List<string> libraryFiles);
    string GeneratePreviewThumbnailSaveLocation(string rootFilePath);
    public string? GetLastFolderName(string folderPath);
    string NormaliseFolderPathToOs(string folderPath);
}