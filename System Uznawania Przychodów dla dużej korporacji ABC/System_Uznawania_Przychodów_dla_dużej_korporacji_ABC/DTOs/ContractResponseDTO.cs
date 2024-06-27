namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.DTOs;

public class ContractResponseDTO
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
    public DateTime EndDate { get; set; }
}