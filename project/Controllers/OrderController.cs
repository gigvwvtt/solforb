using Microsoft.AspNetCore.Mvc;
using project.Data;
using project.Models;

namespace project.Controllers;

public class OrderController : Controller
{
    private IDbRepository<Order> _orderRepository;

    public OrderController(IDbRepository<Order> orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<IActionResult> Index()
    {
        var orders = await _orderRepository.GetAll();
        return View(orders);
    }


    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Order order)
    {
        return View("Index");
    }
}