using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project.Models;

public class OrderItem
{
    [Key]
    public int Id { get; set; }
    [ForeignKey("Order")]
    public int OrderId { get; set; }
    public Order? Order { get; set; }
    public string Name { get; set; }
    [Column(TypeName = "decimal(18,3)")] 
    public decimal Quantity { get; set; }
    public string Unit { get; set; }
}