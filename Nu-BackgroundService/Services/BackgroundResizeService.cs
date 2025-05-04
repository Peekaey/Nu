using System.Collections.Concurrent;
using System.Net.Mime;
using Nu_Cache.Interfaces;
using Nu_Models;
using Nu_Models.DTOs;
using Nu_Models.Extensions.Interfaces;
using Nu_Models.Results;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Nu_Cache.Services;

public class BackgroundResizeService : IBackgroundResizeService
{
    private readonly ILogger<BackgroundResizeService> _logger;
    private readonly ApplicationConfigurationSettings _applicationConfigurationSettings;
    private readonly IFilePathExtensions _filePathExtensions;
    
    public BackgroundResizeService(ILogger<BackgroundResizeService> logger, ApplicationConfigurationSettings applicationConfigurationSettings, IFilePathExtensions filePathExtensions)
    {
        _filePathExtensions = filePathExtensions;
        _applicationConfigurationSettings = applicationConfigurationSettings;
        _logger = logger;
    }

    // TODO Optimise Memory Usage and Fix large unmanaged memmory usage
    public PreviewThumbnailGenerateResult GeneratePreviewThumbnails(List<FileDTO> fileDtos, string rootFolderPath)
    {
        var previewThumbnails = new ConcurrentBag<PreviewThumbnailDTO>();
        var thumbnailFolderPath = _filePathExtensions.GeneratePreviewThumbnailSaveLocation(rootFolderPath);
        _logger.Log(LogLevel.Information, $"Generating Preview Thumbnails for {fileDtos.Count} files in {thumbnailFolderPath}");

        var options = new ParallelOptions
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount
        };
        
        var lockObjects = new ConcurrentDictionary<string, object>();

        var batchSize = 10;
        var batches = fileDtos
            .Chunk(batchSize)
            .ToList();
        
        Parallel.ForEach(batches, options, batch =>
        {
            foreach (var fileDto in batch)
            {
                var lastFolderName = _filePathExtensions.GetLastFolderName(fileDto.DirectoryName);
                var thumbnailFileName = $"{lastFolderName}-{fileDto.FileName}.jpg";
                var thumbnailFullFileName = Path.Combine(thumbnailFolderPath, $"{lastFolderName}-{fileDto.FileName}.jpg");
                var lockKey = thumbnailFullFileName.ToLower();
                var lockObject = lockObjects.GetOrAdd(lockKey, new object());

                lock (lockObject)
                {
                    try
                    {
                        // Load Image from disk
                        using (var image = Image.Load<Rgba32>(fileDto.FullName))
                        {
                            // Disable metadata copying
                            image.Metadata.ExifProfile = null;
                            image.Metadata.XmpProfile = null;
                            image.Metadata.IptcProfile = null;

                            // Resize the image to a thumbnail size
                            var thumbnail = image.Clone(ctx => ctx.Resize(new ResizeOptions
                            {
                                Size = new Size(600, 300),
                                Mode = ResizeMode.Pad
                            }));
                            
                            // Save the thumbnail to disk
                            thumbnail.Save(thumbnailFullFileName, new JpegEncoder
                            {
                                Quality = 75
                            });

                            // Add the thumbnail to the list
                            previewThumbnails.Add(new PreviewThumbnailDTO
                            {
                                FileName = thumbnailFileName,
                                FullName = thumbnailFullFileName,
                                FileType = ".jpg",
                                DirectoryName = "Nu-PreviewThumbnails",
                                CreationTime = DateTime.UtcNow,
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Log(LogLevel.Error, $"Error generating thumbnail for {fileDto.FullName}: {ex.Message}");
                    }
                    finally
                    {
                        lockObjects.TryRemove(lockKey, out _);
                    }
                }
            }
        });
        _logger.Log(LogLevel.Information, $"Generated {previewThumbnails.Count} Preview Thumbnails in {thumbnailFolderPath}");
        return PreviewThumbnailGenerateResult.AsSuccess(previewThumbnails.ToList());
    }

}