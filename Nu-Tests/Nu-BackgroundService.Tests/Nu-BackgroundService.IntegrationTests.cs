using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Moq;
using Nu_Cache.Interfaces;
using Nu_Cache.Services;
using Nu_DataService.Interfaces;
using Nu_DataService.Services;

namespace Nu_Tests.Nu_BackgroundService.Tests;

[TestFixture]
public class BackgroundOrchestrator_IntegrationTests
{
    private string _tempFolderPath;

    [SetUp]
    public void SetUp()
    {
        // Create a temporary folder for testing
        // _tempFolderPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        // Directory.CreateDirectory(_tempFolderPath);
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up the temporary folder after tests complete
        // if (Directory.Exists(_tempFolderPath))
        // {
        //     Directory.Delete(_tempFolderPath, recursive: true);
        // }
    }

    [Test]
    public async Task IndexLibraryContents_LiveTest_ShouldReturnSuccess()
    {
        // Arrange
        var filePath = "";
        var orchestrationServiceLogger = new LoggerFactory().CreateLogger<BackgroundOrchestratorService>();
        var backgroundFolderServiceLogger = new LoggerFactory().CreateLogger<BackgroundFolderService>();
        var backgroundFileServiceLogger = new LoggerFactory().CreateLogger<BackgroundFileService>();
        var indexingServiceLogger = new LoggerFactory().CreateLogger<IndexingService>();
        
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        IBackgroundFolderService folderService = new BackgroundFolderService(backgroundFolderServiceLogger);
        IBackgroundFileService fileService = new BackgroundFileService(backgroundFileServiceLogger);
        IIndexingService indexingService = new IndexingService(indexingServiceLogger,mockUnitOfWork.Object);
        
        var orchestratorService = new BackgroundOrchestratorService(fileService, folderService, indexingService, orchestrationServiceLogger);
        
        // Act
        var result = await orchestratorService.IndexLibraryContents(filePath);
        
        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Success);
    }
}