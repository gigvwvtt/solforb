using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project.Models;

public class Order
{
    [Key]
    public int Id { get; set; } 
    [DisplayName("Номер заказа")]
    public string Number { get; set; }
    [DisplayName("Дата заказа")]
    [DataType(DataType.Date)] 
    public DateTime Date { get; set; }
    [ForeignKey("Provider")]
    [DisplayName("Поставщик")]
    public int ProviderId { get; set; }
    public Provider? Provider { get; set; }
}
