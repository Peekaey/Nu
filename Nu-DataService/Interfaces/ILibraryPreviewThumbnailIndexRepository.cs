using Nu_Models.DatabaseModels;

namespace Nu_DataService.Interfaces;

public interface ILibraryPreviewThumbnailIndexRepository
{
    Task AddAsync(LibraryPreviewThumbnailIndex previewThumbnailIndex);
    void Add(LibraryPreviewThumbnailIndex previewThumbnailIndex);
    void AddRange(IEnumerable<LibraryPreviewThumbnailIndex> previewThumbnailIndexes);
    Task AddRangeAsync(List<LibraryPreviewThumbnailIndex> previewThumbnailIndexes);
    void Remove(LibraryPreviewThumbnailIndex previewThumbnailIndex);
    void Update(LibraryPreviewThumbnailIndex previewThumbnailIndex);
    Task<LibraryPreviewThumbnailIndex?> GetAsync(int id);
    LibraryPreviewThumbnailIndex? Get(int id);
    List<LibraryPreviewThumbnailIndex> GetAll();
}