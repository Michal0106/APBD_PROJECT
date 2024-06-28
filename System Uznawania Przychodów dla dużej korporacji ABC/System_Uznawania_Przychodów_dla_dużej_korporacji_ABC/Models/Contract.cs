using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Validators;

namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Models;

public class Contract
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int CustomerId { get; set; }
    
    [Required]
    public int ProductId { get; set; }
    
    [Required]
    public double Price { get; set; }
    public bool IsPaid { get; set; }
    public bool IsSigned { get; set; }
    public int YearsOfUpdateSupport { get; set; }
    
    [Required]
    public string SoftwareVersion { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
        
    [Required]
    [ContractDurationValidation(nameof(StartDate))]
    public DateTime EndDate { get; set; }

    [ForeignKey(nameof(CustomerId))] 
    public Customer Customer { get; set; } = null!;
    
    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; }  = null!;

    public ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();
}