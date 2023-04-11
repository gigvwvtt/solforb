using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using project.Data;
using project.Models;
using project.ViewModels;

namespace project.Controllers;

public class OrderController : Controller
{
    private readonly IDbRepository<Order> _orderRepository;
    private readonly IDbRepository<Provider> _providerRepository;
    private readonly IDbRepository<OrderItem> _orderItemRepository;

    public OrderController(IDbRepository<Order> orderRepository, IDbRepository<Provider> providerRepository,
        IDbRepository<OrderItem> orderItemRepository)
    {
        _orderRepository = orderRepository;
        _providerRepository = providerRepository;
        _orderItemRepository = orderItemRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate, string? selectedNumber,
        int? selectedProvider, string? selectedItemName, string? selectedItemUnit)
    {
        var orders = _orderRepository.GetAllWithInclude(o => o.Provider);

        var ordersNumbersRaw = await _orderRepository.GetDistinctTs(p => new { p.Number });
        var ordersNumbers = new SelectList(ordersNumbersRaw.Select(o => o.Number));

        var itemsNamesRaw = await _orderItemRepository.GetDistinctTs(p => new { p.Name });
        var itemsNames = new SelectList(itemsNamesRaw.Select(o => o.Name));

        var itemsUnitsRaw = await _orderItemRepository.GetDistinctTs(p => new { p.Unit });
        var itemsUnits = new SelectList(itemsUnitsRaw.Select(o => o.Unit));

        var providers = await GetAvailableProviders();


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
        
        if (selectedItemName != null)
        {
            var items = _orderItemRepository.GetWithFilter(oi => oi.Name == selectedItemName)
                .Select(oi=>oi.OrderId)
                .ToList();
            orders = orders.IntersectBy(items, o=>o.Id);
        }
        
        if (selectedItemUnit != null)
        {
            var items = _orderItemRepository.GetWithFilter(oi => oi.Unit == selectedItemUnit)
                .Select(oi=>oi.OrderId)
                .ToList();
            orders = orders.IntersectBy(items, o=>o.Id);
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
    public async Task<IActionResult> Create()
    {
        var providers = await GetAvailableProviders();

        var createOrderViewModel = new CreateOrderViewModel()
        {
            Providers = providers
        };

        return View(createOrderViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateOrderViewModel createOrderViewModel)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Ошибка при создании заказа");
            createOrderViewModel.Providers = await GetAvailableProviders();
            return View(createOrderViewModel);
        }

        var checkOrder = _orderRepository.GetWithFilter(o => o.Number == createOrderViewModel.Number,
                o => o.ProviderId == createOrderViewModel.ProviderId)
                .Any();

        if (checkOrder)
        {
            ModelState.AddModelError("", "Наименование товара не может быть таким же как номер заказа");
            createOrderViewModel.Providers = await GetAvailableProviders();
            return View(createOrderViewModel);
        }

        var newOrder = new Order()
        {
            Date = createOrderViewModel.Date,
            Number = createOrderViewModel.Number,
            ProviderId = createOrderViewModel.ProviderId
        };

        _orderRepository.Add(newOrder);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var order = _orderRepository.GetWithFilterWithInclude(o => o.Id == id, p => p.Provider).ToList()[0];
        if (order == null)
        {
            return View("Error");
        }

        var orderItems = _orderItemRepository.GetWithFilterWithInclude(oi => oi.OrderId == id).ToList();

        var detailsViewModel = new OrderDetailsViewModel()
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
        var order = _orderRepository.GetWithFilterWithInclude(o => o.Id == id, o => o.Provider).ToList();
        if (order == null) return View("Error");

        var providers = await GetAvailableProviders();

        var orderItems = _orderItemRepository.GetWithFilterWithInclude(oi => oi.OrderId == id).ToList();

        var editViewModel = new EditOrderViewModel()
        {
            Id = id,
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
    public async Task<IActionResult> Edit(int id, EditOrderViewModel editOrderViewModel)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Ошибка при редактировании заказа");
            editOrderViewModel.Providers = await GetAvailableProviders();
            return View(editOrderViewModel);
        }

        var orderInDb = _orderRepository.GetWithFilter(o=>o.Id == id).ToList();
        if (orderInDb == null) return View("Error");

        var order = new Order()
        {
            Id = orderInDb[0].Id,
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

    private async Task<SelectList> GetAvailableProviders()
    {
        var providersRaw = await _providerRepository.GetDistinctTs(p => p);
        return new SelectList(providersRaw, "Id", "Name");
    }
}