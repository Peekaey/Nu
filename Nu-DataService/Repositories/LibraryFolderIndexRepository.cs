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
    
    public async Task AddAsync(LibraryFolderIndex fileIndex)
    {
        await _context.LibraryFolderIndexes.AddAsync(fileIndex);

    }
    
    public void Add(LibraryFolderIndex fileIndex)
    {
        _context.LibraryFolderIndexes.Add(fileIndex);
    }
    
    public async Task AddRangeAsync(List<LibraryFolderIndex> folderIndexes)
    {
        await _context.LibraryFolderIndexes.AddRangeAsync(folderIndexes);
    }
    
    public void AddRange(List<LibraryFolderIndex> folderIndexes)
    {
        _context.LibraryFolderIndexes.AddRange(folderIndexes);
    }
    public void Remove(LibraryFolderIndex fileIndex)
    {
        _context.LibraryFolderIndexes.Remove(fileIndex);
    }
    
    public void Update(LibraryFolderIndex fileIndex)
    {
        _context.LibraryFolderIndexes.Update(fileIndex);
    }
    
    public async Task<LibraryFolderIndex?> GetAsync(int id)
    {
        return await _context.LibraryFolderIndexes.FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public LibraryFolderIndex? Get(int id)
    {
        return _context.LibraryFolderIndexes.FirstOrDefault(x => x.Id == id);
    }
    
    public IEnumerable<LibraryFolderIndex> GetAll()
    {
        return _context.LibraryFolderIndexes;
    }
    
    public LibraryFolderIndex? GetLibraryRootFolder()
    {
        return _context.LibraryFolderIndexes.Where(x => x.ParentFolderId == null)
            .Include(cf => cf.ChildFolders)
            .Include(cf => cf.LibraryFiles)
            .FirstOrDefault();
    }

    public LibraryFolderIndex? GetLibraryFolderWithChildren(int id)
    {
        var folder =_context.LibraryFolderIndexes
            .Include(x => x.ChildFolders)
            .Include(x => x.LibraryFiles)
            .FirstOrDefault(x => x.Id == id);
        
        return folder;
        
    }

    public IEnumerable<LibraryFolderIndex> GetFoldersByFolderName(List<string> folderNames)
    {
        return _context.LibraryFolderIndexes
            .Where(x => folderNames.Contains(x.FolderName))
            .ToList();
    }
}