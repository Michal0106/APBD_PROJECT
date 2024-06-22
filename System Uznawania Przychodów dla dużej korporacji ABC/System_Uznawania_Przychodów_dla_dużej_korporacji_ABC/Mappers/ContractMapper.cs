using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.DTOs;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Models;

namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Mappers;

public static class ContractMapper
{
    public static Contract CreateContract(this ContractDto contractDto, int additionalYearsOfUpdateSupport)
    {
        return new Contract
        {
            CustomerId = contractDto.CustomerId,
            ProductId = contractDto.ProductId,
            Price = contractDto.Price,
            YearsOfUpdateSupport = contractDto.YearsOfUpdateSupport + additionalYearsOfUpdateSupport,
            IsPaid = false,
            IsSigned = false,
            StartDate = contractDto.StartDate,
            EndDate = contractDto.EndDate,
        };
    }
}