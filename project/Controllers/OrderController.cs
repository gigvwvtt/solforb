using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using project.Data;
using project.Models;
using project.ViewModels;

namespace project.Controllers;

public class OrderController : Controller
{
    private IDbRepository<Order> _orderRepository;
    private IDbRepository<Provider> _providerRepository;
    private IDbRepository<OrderItem> _orderItemRepository;

    public OrderController(IDbRepository<Order> orderRepository, IDbRepository<Provider> providerRepository,
        IDbRepository<OrderItem> orderItemRepository)
    {
        _orderRepository = orderRepository;
        _providerRepository = providerRepository;
        _orderItemRepository = orderItemRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate, string? selectedNumber, int? selectedProvider)
    {
        var orders = _orderRepository.GetAllWithInclude(o=>o.Provider);

        var ordersNumbersRaw = await _orderRepository.GetDistinctTs(p => new { p.Number });
        var ordersNumbers = new SelectList(ordersNumbersRaw.Select(o => o.Number));

        var itemsNamesRaw = await _orderItemRepository.GetDistinctTs(p => new { p.Name });
        var itemsNames = new SelectList(itemsNamesRaw.Select(o => o.Name));

        var itemsUnitsRaw = await _orderItemRepository.GetDistinctTs(p => new { p.Unit });
        var itemsUnits = new SelectList(itemsUnitsRaw.Select(o => o.Unit));

        var providersRaw = await _providerRepository.GetDistinctTs(p => p);
        var providers = new SelectList(providersRaw, "Id", "Name");

        

        //filters
        if (startDate != null && endDate != null)
        {
            orders = orders.Where(o => o.Date > startDate && o.Date < endDate);
        }

        if (selectedNumber != null)
        {
            orders = orders.Where(o => o.Number == selectedNumber);
        }

        if (selectedProvider != null)
        {
            orders = orders.Where(o => o.ProviderId == selectedProvider);
        }

        var ordersViewModel = new OrdersViewModel
        {
           Orders = orders,
           OrdersNumbers = ordersNumbers,
           ItemsNames = itemsNames,
           Units = itemsUnits,
           Providers = providers
        };
        
        return View(ordersViewModel);
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

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var orders = await _orderRepository.GetAll();

        var providers = await _providerRepository.GetAll();
        ViewBag.AvailableProviders = new SelectList(providers.Select(p => p.Name).Distinct());
        return View();
    }
}