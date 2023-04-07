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
        var order = _orderRepository.GetByXWithInclude(o=>o.Id == id, p=>p.Provider).ToList()[0];
        if (order == null)
        {
            return View("Error");
        }

        var orderItems = _orderItemRepository.GetByXWithInclude(oi => oi.OrderId == id).ToList();

        var detailsViewModel = new DetailsViewModel()
        {
            Id = order.Id,
            Date = order.Date,
            Number = order.Number,
            Provider = order.Provider,
            ProviderId = order.ProviderId,
            OrderItems = orderItems 
        };
        
        return View(detailsViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var order = _orderRepository.GetByXWithInclude(o=>o.Id == id, o=>o.Provider).ToList();
        if (order == null) return View("Error");
        
        var providersRaw = await _providerRepository.GetDistinctTs(p => p);
        var providers = new SelectList(providersRaw,
            "Id", "Name");
        
        var orderItems = _orderItemRepository.GetByXWithInclude(oi => oi.OrderId == id).ToList();
        
        var editViewModel = new EditViewModel()
        {
            Date = order[0].Date,
            Number = order[0].Number,
            Provider = order[0].Provider,
            Providers = providers,
            OrderItems = orderItems 
        };
        
        return View(editViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditViewModel editOrderViewModel)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Ошибка при редактировании");
            return View("Edit", editOrderViewModel);
        }

        var order = new Order()
        {
            Id = id,
            Date = editOrderViewModel.Date,
            Number = editOrderViewModel.Number,
            Provider = editOrderViewModel.Provider,
            ProviderId = editOrderViewModel.ProviderId
        };

        _orderRepository.Update(order);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var order = await _orderRepository.GetById(id);
        if (order == null) return View("Error");

        _orderRepository.Delete(order);
        return RedirectToAction("Index");
    }
}