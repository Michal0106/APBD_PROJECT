using System.Text.Json.Serialization;

namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CustomerType
{
    INDIVIDUAL,COMPANY
}