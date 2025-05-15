namespace System_Zarz.Data;

public class Part
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    
    public List<OrderPart> OrderParts { get; set; } = new();
}

public class OrderPart
{
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    
    public int PartId { get; set; }
    public Part Part { get; set; } = null!;
    
    public int Quantity { get; set; }
}
