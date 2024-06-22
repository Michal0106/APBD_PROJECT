using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Validators;

namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.DTOs;

public class ContractDto
{
    public int CustomerId { get; set; }
    public int ProductId { get; set; }
    public double Price { get; set; }
    public int YearsOfUpdateSupport { get; set; } = 1;
    public string SoftwareVersion { get; set; }
    public DateTime StartDate { get; set; }
    
    [ContractDurationValidation(nameof(StartDate))]
    public DateTime EndDate { get; set; }
}