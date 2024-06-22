using System.ComponentModel.DataAnnotations.Schema;

namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Models;

public class Payment
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int ContractId { get; set; }
    public double Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public bool IsRefunded { get; set; }
    
    [ForeignKey(nameof(CustomerId))]
    public Customer Customer { get; set; }  = null!;
    
    [ForeignKey(nameof(ContractId))]
    public Contract Contract { get; set; }  = null!;
}