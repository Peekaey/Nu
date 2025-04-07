using Nu_Models.Results;

namespace Nu_Cache.Interfaces;

public interface IBackgroundFileService
{
    Task<FileReaderServiceResult> GetParentStorageFiles(string rootFolderPath);
}