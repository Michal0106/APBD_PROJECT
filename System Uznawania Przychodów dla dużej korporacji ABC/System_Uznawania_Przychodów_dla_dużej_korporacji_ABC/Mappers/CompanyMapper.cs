using Newtonsoft.Json;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.DTOs;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Models;

namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Mappers;

public static class CompanyMapper
{
    public static Company CreateCompany(this CustomerDTO dto)
    {
        return new Company
        {
            Krs = dto.Krs,
            Name = dto.CompanyName,
            Address = dto.Address,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Discriminator = CustomerType.COMPANY
        };
    }
}