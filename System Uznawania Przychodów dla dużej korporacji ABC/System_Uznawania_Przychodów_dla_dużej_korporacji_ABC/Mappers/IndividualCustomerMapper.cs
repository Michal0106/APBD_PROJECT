using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.DTOs;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Models;

namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Mappers;

public static class IndividualCustomerMapper
{
    public static IndividualCustomer CreateIndividualCustomer(this CustomerDTO dto)
    {
        return new IndividualCustomer()
        {
            Pesel = dto.Pesel,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Address = dto.Address,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Discriminator = CustomerType.INDIVIDUAL,
            IsDeleted = false
        };
    }
}