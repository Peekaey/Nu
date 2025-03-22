using System.ComponentModel.DataAnnotations;
using Nu_Models.Enums;

namespace Nu_Models.DatabaseModels;

public class Account : IAuditable
{
    public int Id { get; set; }
    [EmailAddress]
    public required string UserName { get; set; }
    
    [StringLength(256)]
    public required string PasswordHash { get; set; }
    public Guid? PasswordSalt { get; set; }
    public AccountType AccountType { get; set; }
    public bool IsActive { get; set; }
    
    public DateTime CreatedDate { get; set; }
    public DateTime LastUpdatedDate { get; set; }
    
    public DateTime? LastLoginDate { get; set; }
    public DateTime? LastRetryDate { get; set; }
    public int? NumOfRetries { get; set; }
    
}