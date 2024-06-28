using System.ComponentModel.DataAnnotations;

namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Models;

public class Software : Product
{
    [Required]
    public string SoftwareVersion { get; set; }
    [Required]
    public string Category { get; set; }
}