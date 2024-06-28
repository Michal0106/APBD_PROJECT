using System.ComponentModel.DataAnnotations;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Validators;

namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.DTOs;

public class ContractDto
{
    [Required]
    public int CustomerId { get; set; }
    [Required]
    public int ProductId { get; set; }
    [Required]
    public double Price { get; set; }
    public readonly int YearsOfUpdateSupport = 1;
    [Required]
    public string SoftwareVersion { get; set; }
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    [ContractDurationValidation(nameof(StartDate))]
    public DateTime EndDate { get; set; }
}