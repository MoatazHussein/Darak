namespace Darak.Domain.Entities;

public class OtpChallenge
{
    public Guid Id { get; set; } = Guid.NewGuid();          
    public string PhoneNumber { get; set; } = default!;
    public string Purpose { get; set; } = default!;         
    public string CodeHash { get; set; } = default!;
    public string Salt { get; set; } = default!;
    public DateTime ExpiresAtUtc { get; set; }               
    public int AttemptCount { get; set; } = 0;               
    public int MaxAttempts { get; set; }                     
    public bool Consumed { get; set; } = false;              
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
