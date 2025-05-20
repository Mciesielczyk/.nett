namespace System_Zarz.Data;

public class Task
{
    public int Id { get; set; }
    public string Description { get; set; } = null!;
    public decimal LaborCost { get; set; }
    
    public List<OrderTask> OrderTasks { get; set; } = new();
}

public class OrderTask
{
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    
    public int TaskId { get; set; }
    public Task Task { get; set; } = null!;
}
