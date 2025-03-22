using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Nu_DataService;

public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    // Fixes error when creating migration of  " The exception 'Unable to resolve service for type 'Microsoft.EntityFrameworkCore.DbContextOptions`1[Nu_DataService.DataContext]'
    // while attempting to activate 'Nu_DataService.DataContext'.' was thrown while attempting to create an instance. 
    // Happens when DbContext is registered via dependency injection and EntityFrameworkCore is unable to create an instance of DbContext
    // https://learn.microsoft.com/en-us/dotnet/api/system.data.linq.datacontext?view=netframework-4.8.1
    public DataContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=nu;Username=nu;Password=nu;Include Error Detail=true");

        return new DataContext(optionsBuilder.Options);
    }
}