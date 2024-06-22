using System.ComponentModel.DataAnnotations;

namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Models;

public class IndividualCustomer : Customer
{
    [StringLength(11)]
    public string Pesel { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsDeleted { get; set; }
}