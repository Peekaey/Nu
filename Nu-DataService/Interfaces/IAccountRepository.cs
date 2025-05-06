using Nu_Models.DatabaseModels;

namespace Nu_DataService.Interfaces;

public interface IAccountRepository
{
    Task AddAsync(Account account);
    Task<Account?> GetAsync(int id);
    void Add(Account account);
    void Remove(Account account);
    void Update(Account account);
    Account? Get(int id);
    Account? GetByAccountUsername(string username);
}