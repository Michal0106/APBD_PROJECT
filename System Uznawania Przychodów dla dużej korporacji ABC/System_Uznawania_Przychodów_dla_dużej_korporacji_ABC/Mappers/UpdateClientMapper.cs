using Microsoft.OpenApi.Extensions;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.DTOs;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Models;

namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Mappers;

public static class UpdateClientMapper
{
    public static void UpdateEntity(this UpdateClientDTO dto, Customer entity)
    {
        var dtoProperties = typeof(UpdateClientDTO).GetProperties();
        var entityProperties = typeof(Customer).GetProperties();

        foreach (var dtoProperty in dtoProperties)
        {
            var value = dtoProperty.GetValue(dto);
            if (value != null)
            {
                var entityProperty = entityProperties.FirstOrDefault(p => p.Name == dtoProperty.Name);
                if (entityProperty != null && entityProperty.CanWrite)
                {
                    entityProperty.SetValue(entity, value);
                }
            }
        }
    }
}