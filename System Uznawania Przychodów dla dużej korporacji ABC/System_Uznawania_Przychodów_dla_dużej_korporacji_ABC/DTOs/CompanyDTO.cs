using System.ComponentModel.DataAnnotations;

namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.DTOs;

public class CompanyDTO
{
    [Required]
    public string KRS { get; set; }
    [Required]
    public string Name { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}