using System.ComponentModel.DataAnnotations;

namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.DTOs;

public class LoginRequestDTO
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}