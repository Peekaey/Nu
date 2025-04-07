using Microsoft.Extensions.Logging;
using Moq;
using Nu_Cache.Services;

namespace Nu_Tests.Nu_BackgroundService.Tests;

[TestFixture]
public class BackgroundFolderServiceTests
{
    [Test]
    public void GetStorageFolders_ShouldReturnFolders()
    {
        // Arrange
        var folderPath = "";
        var logger = new Mock<ILogger<BackgroundFolderService>>().Object;
        var folderService = new BackgroundFolderService(logger);

        // Act
        var result = folderService.GetStorageFolders(folderPath);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Success);
    }
    
    [Test]
    public void FolderExists_ShouldReturnTrue_WhenDirectoryExists()
    {
        // Arrange
        var folderPath = "";
        var logger = new Mock<ILogger<BackgroundFolderService>>().Object;
        var folderService = new BackgroundFolderService(logger);

        // Act
        var result = folderService.FolderExists(folderPath);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result);
        
    }
    
}