namespace WEB_253503_Kalabin.Domain.Entities;

public class Clothes {
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Category Category { get; set; }
    public int Price { get; set; }
    public string? Image { get; set; }
}