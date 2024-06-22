using System.ComponentModel.DataAnnotations;

namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Models;

public class Company : Customer
{
    public string Name { get; set; }
    [StringLength(10)]
    public string Krs { get; set; }
}