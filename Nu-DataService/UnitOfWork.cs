using Microsoft.Extensions.Logging;
using Nu_DataService.Interfaces;

namespace Nu_DataService;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _context;
    private readonly ILogger<UnitOfWork> _logger;
    
    // Repository Properties
    public IAccountRepository AccountRepository { get; }
    public IUserProfileRepository UserProfileRepository { get; }
    public IUserProfilePictureRepository UserProfilePictureRepository { get; }
    
    public UnitOfWork(DataContext context, ILogger<UnitOfWork> logger,
        IAccountRepository accountRepository, IUserProfileRepository userProfileRepository,
        IUserProfilePictureRepository userProfilePictureRepository)
    {
        _context = context;
        _logger = logger;
        
        AccountRepository = accountRepository;
        UserProfileRepository = userProfileRepository;
        UserProfilePictureRepository = userProfilePictureRepository;
    }
    
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
    
    public void SaveChanges()
    {
        _context.SaveChanges();
    }
    
    public async Task BeginTransactionAsync()
    {
        await _context.Database.BeginTransactionAsync();
    }
    
    public async Task CommitTransactionAsync()
    {
        await _context.Database.CommitTransactionAsync();
    }
    
    public async Task RollbackTransactionAsync()
    {
        await _context.Database.RollbackTransactionAsync();
    }
    
    public void Dispose()
    {
        _context.Dispose();
    }
}