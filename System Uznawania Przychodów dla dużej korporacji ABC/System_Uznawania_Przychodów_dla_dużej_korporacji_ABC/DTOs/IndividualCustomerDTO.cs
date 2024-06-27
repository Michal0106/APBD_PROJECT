using System.ComponentModel.DataAnnotations;

namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.DTOs;

public class IndividualCustomerDTO
{
    [Required]
    public string Pesel { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}