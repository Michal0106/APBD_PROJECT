using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Validators;

namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Models;

public class Discount
{
    [Key]
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public double Percentage { get; set; }
    public int ProductId { get; set; }
    public DateTime StartDate { get; set; }
    
    [EndDateValidation(nameof(StartDate))]
    public DateTime EndDate { get; set; }
    
    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; } = null!;
}