using Microsoft.Extensions.Logging;
using Moq;
using Nu_Cache.Services;

namespace Nu_Tests.Nu_BackgroundService.Tests;

[TestFixture]
public class BackgroundFileServiceTests
{
    [Test]
    public async Task GetParentStorageFiles_ShouldReturnFiles()
    {
        // Arrange
        var filePath = "";
        var logger = new Mock<ILogger<BackgroundFileService>>().Object;
        var fileService = new BackgroundFileService(logger);

        // Act
        var result = await fileService.GetParentStorageFiles(filePath);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Success);
    }
}