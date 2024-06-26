using System.ComponentModel.DataAnnotations;

namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.DTOs;

public class PaymentDTO
{
    [Required]
    public int CustomerId { get; set; }
    [Required]
    public double Amount { get; set; }
    [Required]
    public DateTime PaymentDate { get; set; }
}