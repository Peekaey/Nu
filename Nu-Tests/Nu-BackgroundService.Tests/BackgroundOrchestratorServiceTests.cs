using Microsoft.Extensions.Logging;
using Moq;
using Nu_Cache.Interfaces;
using Nu_Cache.Services;
using Nu_DataService.Interfaces;
using Nu_DataService.Services;

namespace Nu_Tests.Nu_BackgroundService.Tests;

[TestFixture]
public class BackgroundOrchestratorServiceTests
{
    [Test]
    public async Task IndexLibraryContents_ShouldReturnSuccess()
    {
        // Arrange
        var filePath = "";
        var logger = new Mock<ILogger<BackgroundOrchestratorService>>().Object;
        var folderService = new Mock<IBackgroundFolderService>();
        folderService.Setup(fs => fs.FolderExists(It.IsAny<string>())).Returns(true);
        var fileService = new Mock<IBackgroundFileService>();
        
        var indexingService = new Mock<IIndexingService>();
        var orchestratorService = new BackgroundOrchestratorService(fileService.Object, folderService.Object, indexingService.Object, logger);
        
        // Act
        var result = await orchestratorService.IndexLibraryContents(filePath);
        
        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Success);
    }
}