using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nu_DataService.Interfaces;
using Nu_Models.DatabaseModels;

namespace Nu_DataService.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly DataContext _context;
    private readonly ILogger<AccountRepository> _logger;
    
    public AccountRepository(DataContext context, ILogger<AccountRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<Account> AddAsync(Account account)
    {
        await _context.Accounts.AddAsync(account);
        await _context.SaveChangesAsync();
        return account;
    }
    
    public void Add(Account account)
    {
        _context.Accounts.Add(account);
    }
    
    public async Task<Account> RemoveAsync(Account account)
    {
        _context.Accounts.Remove(account);
        await _context.SaveChangesAsync();
        return account;
    }
    
    public void Remove(Account account)
    {
        _context.Accounts.Remove(account);
    }
    
    public async Task<Account> UpdateAsync(Account account)
    {
        _context.Accounts.Update(account);
        await _context.SaveChangesAsync();
        return account;
    }
    
    public void Update(Account account)
    {
        _context.Accounts.Update(account);
    }
    
    public async Task<Account> GetAsync(int id)
    {
        return await _context.Accounts.FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public Account Get(int id)
    {
        return _context.Accounts.FirstOrDefault(x => x.Id == id);
    }
    
    public Account? GetByAccountUsername(string username)
    {
        return _context.Accounts.FirstOrDefault(x => x.UserName == username);
    }
    

}