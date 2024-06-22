using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.DTOs;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Models;

namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Mappers;

public static class PaymentMapper
{
    public static Payment CreatePayment(this PaymentDTO paymentDto)
    {
        return new Payment
        {
            CustomerId = paymentDto.CustomerId,
            Amount = paymentDto.Amount,
            PaymentDate = paymentDto.PaymentDate,
            IsRefunded = false
        };
    }
}