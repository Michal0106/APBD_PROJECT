using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Models;

public class Payment
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int CustomerId { get; set; }
    [Required]
    public int ContractId { get; set; }
    [Required]
    public double Amount { get; set; }
    [Required]
    public DateTime PaymentDate { get; set; }
    public bool IsRefunded { get; set; }

    [ForeignKey(nameof(CustomerId))] 
    public Customer Customer { get; set; }

    [ForeignKey(nameof(ContractId))]
    public Contract Contract { get; set; }
}