using SharedKernel.Primitive;

namespace Ordering.Domain.Entities;

public class OrderDetail : StateAuditableEntity
{
    public int OrderId { get; set; }
    public int DishId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string? Notes { get; set; }

    public Order Order { get; set; } = null!;
    public Dish Dish { get; set; } = null!;
}
