using Microsoft.AspNetCore.Mvc.Rendering;
using project.Models;

namespace project.ViewModels;

public class EditOrderViewModel
{
    public int Id { get; set; }
    public string Number { get; set; }
    public DateTime Date { get; set; }
    public int ProviderId { get; set; }
    public Provider? Provider { get; set; }
    public List<OrderItem>? OrderItems { get; set; }
    public SelectList? Providers { get; set; }
}