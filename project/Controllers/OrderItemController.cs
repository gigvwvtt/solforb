using Microsoft.AspNetCore.Mvc;
using project.Data;
using project.Models;
using project.ViewModels;

namespace project.Controllers;

public class OrderItemController : Controller
{
    private readonly IDbRepository<OrderItem> _orderItemRepository;
    private readonly IDbRepository<Order> _orderRepository;

    public OrderItemController(IDbRepository<OrderItem> orderItemRepository, IDbRepository<Order> orderRepository)
    {
        _orderItemRepository = orderItemRepository;
        _orderRepository = orderRepository;
    }
    
    [HttpGet]
    public IActionResult Create(int orderId)
    {
        var createOrderItemViewModel = new CreateOrderItemViewModel()
        {
            OrderId = orderId
        };
        return View(createOrderItemViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CreateOrderItemViewModel createOrderItemViewModel)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Ошибка при создании элемента заказа");
            return View(createOrderItemViewModel);
        }
        
        var checkItemId = _orderRepository
            .GetWithFilter(o=>o.Id == createOrderItemViewModel.OrderId, o=>o.Number == createOrderItemViewModel.Name)
            .Any();
            
        if (checkItemId)
        {
            ModelState.AddModelError("", "Наименование товара не может быть таким же как номер заказа");
            return View(createOrderItemViewModel);
        }

        var orderItem = new OrderItem()
        {
            Name = createOrderItemViewModel.Name,
            OrderId = createOrderItemViewModel.OrderId,
            Quantity = createOrderItemViewModel.Quantity,
            Unit = createOrderItemViewModel.Unit
        };
        
        _orderItemRepository.Add(orderItem);
        
        return RedirectToAction("Edit", "Order", new { id = createOrderItemViewModel.OrderId });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var orderItemInDb = await _orderItemRepository.GetById(id);
        if (orderItemInDb == null) return View("Error");
        
        var editOrderItemViewModel = new EditOrderItemViewModel()
        {
            Id = id,
            Name = orderItemInDb.Name,
            OrderId = orderItemInDb.OrderId,
            Quantity = orderItemInDb.Quantity,
            Unit = orderItemInDb.Unit
        };
        
        return View(editOrderItemViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditOrderItemViewModel editOrderItemViewModel, int id)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Ошибка при редактировании элемента заказа");
            return View(editOrderItemViewModel);
        }
        
        var orderItemInDb = _orderItemRepository.GetWithFilter(o=>o.Id == id).ToList();
        if (orderItemInDb == null) return NotFound();

        var orderItem = new OrderItem()
        {
            Id = id,
            Name = editOrderItemViewModel.Name,
            OrderId = editOrderItemViewModel.OrderId,
            Quantity = editOrderItemViewModel.Quantity,
            Unit = editOrderItemViewModel.Unit
        };

        _orderItemRepository.Update(orderItem);
        return RedirectToAction("Edit", "Order", new { id = orderItem.OrderId});
    }
    
    [Route("/[controller]/Delete/{id:int}")]
    [HttpGet]
    public async Task<IActionResult> Delete([FromRoute]int id)
    {
        var orderItem = await _orderItemRepository.GetById(id);
        if (orderItem == null) return View("Error");

        _orderItemRepository.Delete(orderItem);
        var prevPage = Request.GetTypedHeaders().Referer?.ToString();
        return Redirect(prevPage);
    }
}