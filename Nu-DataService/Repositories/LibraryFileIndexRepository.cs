using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nu_DataService.Interfaces;
using Nu_Models.DatabaseModels;

namespace Nu_DataService.Repositories;

public class LibraryFileIndexRepository : ILibraryFileIndexRepository
{
    private readonly DataContext _context;
    private readonly ILogger<LibraryFileIndexRepository> _logger;

    public LibraryFileIndexRepository(DataContext context, ILogger<LibraryFileIndexRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<LibraryFileIndex> AddAsync(LibraryFileIndex fileIndex)
    {
        await _context.LibraryFileIndexes.AddAsync(fileIndex);
        await _context.SaveChangesAsync();
        return fileIndex;
    }
    
    public void Add(LibraryFileIndex fileIndex)
    {
        _context.LibraryFileIndexes.Add(fileIndex);
    }
    
    public async Task<List<LibraryFileIndex>> AddRangeAsync(List<LibraryFileIndex> fileIndexes)
    {
        await _context.LibraryFileIndexes.AddRangeAsync(fileIndexes);
        await _context.SaveChangesAsync();
        return fileIndexes;
    }
    
    public void AddRange(List<LibraryFileIndex> fileIndexes)
    {
        _context.LibraryFileIndexes.AddRange(fileIndexes);
    }
    
    public async Task<LibraryFileIndex> RemoveAsync(LibraryFileIndex fileIndex)
    {
        _context.LibraryFileIndexes.Remove(fileIndex);
        await _context.SaveChangesAsync();
        return fileIndex;
    }
    
    public void Remove(LibraryFileIndex fileIndex)
    {
        _context.LibraryFileIndexes.Remove(fileIndex);
    }
    
    public async Task<LibraryFileIndex> UpdateAsync(LibraryFileIndex fileIndex)
    {
        _context.LibraryFileIndexes.Update(fileIndex);
        await _context.SaveChangesAsync();
        return fileIndex;
    }
    
    public void Update(LibraryFileIndex fileIndex)
    {
        _context.LibraryFileIndexes.Update(fileIndex);
    }
    
    public async Task<LibraryFileIndex> GetAsync(int id)
    {
        return await _context.LibraryFileIndexes.FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public LibraryFileIndex Get(int id)
    {
        return _context.LibraryFileIndexes.FirstOrDefault(x => x.Id == id);
    }
}