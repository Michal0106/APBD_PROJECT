using System.ComponentModel.DataAnnotations;

namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Models;

public abstract class Customer
{
    [Key] 
    public int Id { get; set; }
    public string Address { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public CustomerType Discriminator { get; set; }

    public ICollection<Contract> Contracts { get; set; } = new HashSet<Contract>();
    public ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();
}