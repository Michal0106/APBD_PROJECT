namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.DTOs;

public class PaymentResponseDTO
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int ContractId { get; set; }
    public double Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public bool IsRefunded { get; set; }
}