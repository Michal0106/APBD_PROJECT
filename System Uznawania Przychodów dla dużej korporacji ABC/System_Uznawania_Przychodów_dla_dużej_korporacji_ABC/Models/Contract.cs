using System.ComponentModel.DataAnnotations.Schema;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Validators;

namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Models;

public class Contract
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int ProductId { get; set; }
    public double Price { get; set; }
    public bool IsPaid { get; set; }
    public bool IsSigned { get; set; }
    public int YearsOfUpdateSupport { get; set; }
    public string SoftwareVersion { get; set; }
    public DateTime StartDate { get; set; }
        
    [ContractDurationValidation(nameof(StartDate))]
    public DateTime EndDate { get; set; }

    [ForeignKey(nameof(CustomerId))] 
    public Customer Customer { get; set; } = null!;
    
    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; }  = null!;

    public ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();
}