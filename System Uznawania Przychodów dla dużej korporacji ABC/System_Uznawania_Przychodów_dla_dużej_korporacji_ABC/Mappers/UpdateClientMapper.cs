using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.DTOs;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Models;

namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Mappers;

public static class UpdateClientMapper
{
    public static void UpdateEntity(this UpdateClientDTO dto, Customer entity)
    {
        entity.Address = dto.Address;
        entity.Email = dto.Email;
        entity.PhoneNumber = dto.PhoneNumber;
    }
}