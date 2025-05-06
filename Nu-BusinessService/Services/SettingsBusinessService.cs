using Microsoft.Extensions.Logging;
using Nu_BusinessService.Interfaces;
using Nu_Models;

namespace Nu_BusinessService.Services;

public class SettingsBusinessService : ISettingsBusinessService
{
    private readonly ILogger<SettingsBusinessService> _logger;
    private readonly ApplicationConfigurationSettings _applicationConfigurationSettings;

    public SettingsBusinessService(ILogger<SettingsBusinessService> logger, ApplicationConfigurationSettings applicationConfigurationSettings)
    {
        _logger = logger;
        _applicationConfigurationSettings = applicationConfigurationSettings;
    }

    public string GetRootFolderPath()
    {
        return _applicationConfigurationSettings.RootFolderPath;
    }
    
}