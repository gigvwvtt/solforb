using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using project.Models;

namespace project.ViewModels;

public class CreateOrderViewModel
{
    public int Id { get; set; } 
    [DisplayName("Номер заказа")]
    public string Number { get; set; }
    [DisplayName("Дата заказа")]
    [DataType(DataType.Date)] 
    public DateTime Date { get; set; }
    [DisplayName("Поставщик")]
    public Provider? Provider { get; set; }
}