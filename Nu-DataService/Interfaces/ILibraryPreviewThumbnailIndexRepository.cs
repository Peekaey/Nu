using Nu_Models.DatabaseModels;

namespace Nu_DataService.Interfaces;

public interface ILibraryPreviewThumbnailIndexRepository
{
    Task<LibraryPreviewThumbnailIndex> AddAsync(LibraryPreviewThumbnailIndex previewThumbnailIndex);
    void Add(LibraryPreviewThumbnailIndex previewThumbnailIndex);
    void AddRange(List<LibraryPreviewThumbnailIndex> previewThumbnailIndexes);
    Task<List<LibraryPreviewThumbnailIndex>> AddRangeAsync(List<LibraryPreviewThumbnailIndex> previewThumbnailIndexes);
    Task<LibraryPreviewThumbnailIndex> RemoveAsync(LibraryPreviewThumbnailIndex previewThumbnailIndex);
    void Remove(LibraryPreviewThumbnailIndex previewThumbnailIndex);
    Task<LibraryPreviewThumbnailIndex> UpdateAsync(LibraryPreviewThumbnailIndex previewThumbnailIndex);
    void Update(LibraryPreviewThumbnailIndex previewThumbnailIndex);
    Task<LibraryPreviewThumbnailIndex> GetAsync(int id);
    LibraryPreviewThumbnailIndex Get(int id);
    List<LibraryPreviewThumbnailIndex> GetAll();
}