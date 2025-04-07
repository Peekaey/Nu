using System.Diagnostics;
using System.Transactions;
using Microsoft.Extensions.Logging;
using Nu_DataService.Interfaces;
using Nu_Models.DatabaseModels;
using Nu_Models.DTOs;
using Nu_Models.Extensions;
using Nu_Models.Results;

namespace Nu_DataService.Services;

public class IndexingService : IIndexingService
{
    private readonly ILogger<IndexingService> _logger;
    private readonly IUnitOfWork _unitOfWork;


    public IndexingService (ILogger<IndexingService> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    public ServiceResult IndexLibraryContents(List<FolderDTO> folders, List<FileDTO> files)
    {
        // TODO Extract Mapping To Separate Extension Helper
        var libraryFolders = folders.Select(f => new LibraryFolderIndex
        {
            FolderName = f.FolderName,
            FolderPath = f.FolderPath,
            FolderLevel = (int)f.FolderLevel
        }).ToList();
        
        
        var folderLookup = libraryFolders.ToDictionary(f => f.FolderPath, f => f);
        
        foreach (var dto in folders)
        {
            if (dto.ParentFolder != null)
            {
                // Find the child entity using its unique FolderPathAfterRoot.
                if (folderLookup.TryGetValue(dto.FolderPath, out var currentFolder) &&
                    folderLookup.TryGetValue(dto.ParentFolder.FolderPath, out var parentFolder))
                {
                    // Set the parent navigation property.
                    currentFolder.ParentFolder = parentFolder;
                    // Set the foreign key explicitly.
                    currentFolder.ParentFolderId = parentFolder.Id;
                }
            }
        }

        // TODO Simplify/Clean
        foreach (var folder in libraryFolders)
        {
            folder.SetCreatedDate();
        }

        var libraryFiles = files.Select(f => new LibraryFileIndex
            {
                SystemCreationTime = f.CreationTime,
                DirectoryName = f.DirectoryName,
                FileType = f.Extension,
                FullName = f.FullName,
                FileName = f.FileName,
                FileSizeInMB = f.FileSizeInMB
            }).ToList();
        
        // TODO Simplify/Clean
        foreach (var file in libraryFiles)
        {
            file.SetCreatedDate();
            file.SystemCreationTime = DateTimeExtensions.ConvertDateTimeToUtc(file.SystemCreationTime);
        }

        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            _unitOfWork.LibraryFolderIndexRepository.AddRange(libraryFolders);
            _unitOfWork.SaveChanges();
            
            var savedLibraryFolders = _unitOfWork.LibraryFolderIndexRepository.GetAll();

            foreach (var file in libraryFiles)
            {
                var parentFolder = savedLibraryFolders.FirstOrDefault(f => f.FolderName == file.DirectoryName || f.FolderPath == file.DirectoryName);
                if (parentFolder != null)
                {
                    file.ParentFolderId = parentFolder.Id;
                }
            }

            _unitOfWork.LibraryFileIndexRepository.AddRange(libraryFiles);
            _unitOfWork.SaveChanges();
            try
            {
                scope.Complete();
                return ServiceResult.AsSuccess();

            }
            catch (Exception e)
            {
                _logger.LogError("Error saving library index to database");
                return ServiceResult.AsFailure("Error saving library index to database");
            }
            
        }
    }
}