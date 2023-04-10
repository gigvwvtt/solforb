namespace project.ViewModels;

public class EditOrderItemViewModel
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string Name { get; set; }
    public decimal Quantity { get; set; } = 1;
    public string Unit { get; set; }
}