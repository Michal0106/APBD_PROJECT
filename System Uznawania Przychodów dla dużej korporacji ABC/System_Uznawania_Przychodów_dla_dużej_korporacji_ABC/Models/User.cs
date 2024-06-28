using System.ComponentModel.DataAnnotations;

namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string Role { get; set; }
}