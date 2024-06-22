using System.ComponentModel.DataAnnotations;

namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Models;

public abstract class Product
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public ProductType Discriminator { get; set; }
    

    public ICollection<Discount> Discounts { get; set; } = new HashSet<Discount>();
}