using Nu_Models.Results;

namespace Nu_Cache.Interfaces;

public interface IBackgroundOrchestratorService
{
    Task<ServiceResult> IndexLibraryContents(string rootFolderPath);
}