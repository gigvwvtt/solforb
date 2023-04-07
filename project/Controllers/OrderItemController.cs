using Microsoft.AspNetCore.Mvc;
using project.Data;
using project.Models;

namespace project.Controllers;

public class OrderItemController : Controller
{
    private IDbRepository<OrderItem> _orderItemRepository;


    public OrderItemController(IDbRepository<OrderItem> orderItemRepository)
    {
        _orderItemRepository = orderItemRepository;
    }
    
    

    public IActionResult Create(object orderId)
    {
        throw new NotImplementedException();
    }

    public IActionResult Delete(int id)
    {
        throw new NotImplementedException();
    }
}