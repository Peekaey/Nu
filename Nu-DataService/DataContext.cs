using Microsoft.EntityFrameworkCore;
using Nu_Models.DatabaseModels;

namespace Nu_DataService;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }
    
    public DbSet<Account> Accounts { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<UserProfilePicture> UserProfilePictures { get; set; }
}