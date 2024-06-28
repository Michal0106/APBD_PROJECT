using System.ComponentModel.DataAnnotations;

namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Models;

public class IndividualCustomer : Customer
{
    [Required]
    [StringLength(11)]
    public string Pesel { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    
    public bool IsDeleted { get; set; }
}