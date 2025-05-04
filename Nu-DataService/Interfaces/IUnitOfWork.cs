namespace Nu_DataService.Interfaces;

public interface IUnitOfWork
{
    IAccountRepository AccountRepository { get; }
    IUserProfileRepository UserProfileRepository { get; }
    IUserProfilePictureRepository UserProfilePictureRepository { get; }
    ILibraryFolderIndexRepository LibraryFolderIndexRepository { get; }
    ILibraryFileIndexRepository LibraryFileIndexRepository { get; }
    ILibraryPreviewThumbnailIndexRepository LibraryPreviewThumbnailIndexRepository { get; }
    Task<int> SaveChangesAsync();
    void SaveChanges();
    
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}