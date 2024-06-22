namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.DTOs;

public class PaymentDTO
{
    public int CustomerId { get; set; }
    public double Amount { get; set; }
    public DateTime PaymentDate { get; set; }
}