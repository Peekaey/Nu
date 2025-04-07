using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nu_DataService.Interfaces;
using Nu_Models.DatabaseModels;

namespace Nu_DataService.Repositories;

public class LibraryFolderIndexRepository : ILibraryFolderIndexRepository
{
    private readonly DataContext _context;
    private readonly ILogger<LibraryFolderIndexRepository> _logger;

    public LibraryFolderIndexRepository(DataContext context, ILogger<LibraryFolderIndexRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<LibraryFolderIndex> AddAsync(LibraryFolderIndex fileIndex)
    {
        await _context.LibraryFolderIndexes.AddAsync(fileIndex);
        await _context.SaveChangesAsync();
        return fileIndex;
    }
    
    public void Add(LibraryFolderIndex fileIndex)
    {
        _context.LibraryFolderIndexes.Add(fileIndex);
    }
    
    public async Task<List<LibraryFolderIndex>> AddRangeAsync(List<LibraryFolderIndex> folderIndexes)
    {
        await _context.LibraryFolderIndexes.AddRangeAsync(folderIndexes);
        await _context.SaveChangesAsync();
        return folderIndexes;
    }
    
    public void AddRange(List<LibraryFolderIndex> folderIndexes)
    {
        _context.LibraryFolderIndexes.AddRange(folderIndexes);
    }
    
    public async Task<LibraryFolderIndex> RemoveAsync(LibraryFolderIndex fileIndex)
    {
        _context.LibraryFolderIndexes.Remove(fileIndex);
        await _context.SaveChangesAsync();
        return fileIndex;
    }
    
    public void Remove(LibraryFolderIndex fileIndex)
    {
        _context.LibraryFolderIndexes.Remove(fileIndex);
    }
    
    public async Task<LibraryFolderIndex> UpdateAsync(LibraryFolderIndex fileIndex)
    {
        _context.LibraryFolderIndexes.Update(fileIndex);
        await _context.SaveChangesAsync();
        return fileIndex;
    }
    
    public void Update(LibraryFolderIndex fileIndex)
    {
        _context.LibraryFolderIndexes.Update(fileIndex);
    }
    
    public async Task<LibraryFolderIndex> GetAsync(int id)
    {
        return await _context.LibraryFolderIndexes.FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public LibraryFolderIndex Get(int id)
    {
        return _context.LibraryFolderIndexes.FirstOrDefault(x => x.Id == id);
    }
    
    public List<LibraryFolderIndex> GetAll()
    {
        return _context.LibraryFolderIndexes.ToList();
    }
}