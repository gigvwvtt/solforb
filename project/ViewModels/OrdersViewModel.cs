using Microsoft.AspNetCore.Mvc.Rendering;
using project.Models;

namespace project.ViewModels;

public class OrdersViewModel
{
    public IEnumerable<Order>? Orders { get; set; }
    public SelectList? OrdersNumbers { get; set; }
    public SelectList? ItemsNames { get; set; }
    public SelectList? Units { get; set; }
    public SelectList? Providers { get; set; }
}