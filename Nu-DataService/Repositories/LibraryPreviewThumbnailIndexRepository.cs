using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nu_DataService.Interfaces;
using Nu_Models.DatabaseModels;

namespace Nu_DataService.Repositories;

public class LibraryPreviewThumbnailIndexRepository : ILibraryPreviewThumbnailIndexRepository
{
    private readonly DataContext _context;
    private readonly ILogger<LibraryPreviewThumbnailIndexRepository> _logger;

    public LibraryPreviewThumbnailIndexRepository(DataContext context,
        ILogger<LibraryPreviewThumbnailIndexRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task AddAsync(LibraryPreviewThumbnailIndex previewThumbnailIndex)
    {
        await _context.LibraryPreviewThumbnailIndexes.AddAsync(previewThumbnailIndex);
    }
    
    public void Add(LibraryPreviewThumbnailIndex previewThumbnailIndex)
    {
        _context.LibraryPreviewThumbnailIndexes.Add(previewThumbnailIndex);
    }
    
    public async Task AddRangeAsync(List<LibraryPreviewThumbnailIndex> previewThumbnailIndexes)
    {
        await _context.LibraryPreviewThumbnailIndexes.AddRangeAsync(previewThumbnailIndexes);
    }
    
    public void AddRange(IEnumerable<LibraryPreviewThumbnailIndex> previewThumbnailIndexes)
    {
        _context.LibraryPreviewThumbnailIndexes.AddRange(previewThumbnailIndexes);
    }
    
    public void Remove(LibraryPreviewThumbnailIndex previewThumbnailIndex)
    {
        _context.LibraryPreviewThumbnailIndexes.Remove(previewThumbnailIndex);
    }
    
    public void Update(LibraryPreviewThumbnailIndex previewThumbnailIndex)
    {
        _context.LibraryPreviewThumbnailIndexes.Update(previewThumbnailIndex);
    }
    
    public async Task<LibraryPreviewThumbnailIndex?> GetAsync(int id)
    {
        return await _context.LibraryPreviewThumbnailIndexes.FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public LibraryPreviewThumbnailIndex? Get(int id)
    {
        return _context.LibraryPreviewThumbnailIndexes.FirstOrDefault(x => x.Id == id);
    }
    
    public List<LibraryPreviewThumbnailIndex> GetAll()
    {
        return _context.LibraryPreviewThumbnailIndexes.ToList();
    }
    
}