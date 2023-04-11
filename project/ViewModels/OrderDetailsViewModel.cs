using System.ComponentModel.DataAnnotations;
using project.Models;

namespace project.ViewModels;

public class OrderDetailsViewModel
{
    public int Id { get; set; }
    public string Number { get; set; }
    [DataType(DataType.Date)] 
    public DateTime Date { get; set; }
    public int? ProviderId { get; set; }
    public Provider? Provider { get; set; }
    public List<OrderItem>? OrderItems { get; set; }
}