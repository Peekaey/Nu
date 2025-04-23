using System.Diagnostics;
using System.Transactions;
using Microsoft.Extensions.Logging;
using Nu_DataService.Interfaces;
using Nu_Models;
using Nu_Models.DatabaseModels;
using Nu_Models.DTOs;
using Nu_Models.Enums;
using Nu_Models.Extensions;
using Nu_Models.Results;

namespace Nu_DataService.Services;

public class IndexingService : IIndexingService
{
    private readonly ILogger<IndexingService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationConfigurationSettings _applicationConfigurationSettings;


    public IndexingService (ILogger<IndexingService> logger, IUnitOfWork unitOfWork,
        ApplicationConfigurationSettings applicationConfigurationSettings)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _applicationConfigurationSettings = applicationConfigurationSettings;
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
        

        // TODO Simplify
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
        
        // TODO Simplify
        foreach (var file in libraryFiles)
        {
            file.SetCreatedDate();
            file.SystemCreationTime = DateTimeExtensions.ConvertDateTimeToUtc(file.SystemCreationTime);
        }

        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            // Save Folder Themselves then map afterwards
            var existingFolders = _unitOfWork.LibraryFolderIndexRepository
                .GetAll()
                .ToList();
            
            var existingFolderPaths = existingFolders
                .ToDictionary(f => f.FolderPath + "/" + f.FolderName, f => f);
            
            var newFolders = libraryFolders
                .Where(f => !existingFolderPaths.ContainsKey(f.FolderPath + "/" + f.FolderName))
                .OrderBy(f => f.FolderLevel)
                .ToList();
            
            var allFolders = _unitOfWork.LibraryFolderIndexRepository
                .GetAll()
                .ToList();
            
            var folderLookup = allFolders
                .ToDictionary(f => f.FolderPath + "/" + f.FolderName, f => f);
            
            _unitOfWork.LibraryFolderIndexRepository.AddRange(newFolders);
            _unitOfWork.SaveChanges();
            
            
            // Map Folders Afterwards
            foreach (var folder in newFolders)
            {
                if (folder.FolderLevel == (int)FolderLevel.Root)
                {
                    continue;
                }
                var folderParentPath = Directory.GetParent(folder.FolderPath);
                var normalisedFolderParentPath = folderParentPath.FullName.Replace("\\", "/");
                var parentFolder = newFolders.FirstOrDefault(f => f.FolderPath == normalisedFolderParentPath && f.FolderLevel != folder.FolderLevel);
                if (parentFolder != null)
                {
                    folder.ParentFolder = parentFolder;
                }
                
                
            }
            
            _unitOfWork.SaveChanges();
            
            var existingFiles = _unitOfWork.LibraryFileIndexRepository
                .GetAll()
                .ToList();
            
            var existingFileFolderPaths = existingFiles
                .ToDictionary(f => f.FullName, f => f);

            var newFiles = libraryFiles
                .Where(f => !existingFileFolderPaths.ContainsKey(f.FullName))
                .ToList();
            
            foreach (var file in newFiles)
            {
                var parentFolder = newFolders.FirstOrDefault(f => f.FolderPath == file.DirectoryName);
                if (parentFolder != null)
                {
                    file.ParentFolderId = parentFolder.Id;
                }
            }

            _unitOfWork.LibraryFileIndexRepository.AddRange(newFiles);
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