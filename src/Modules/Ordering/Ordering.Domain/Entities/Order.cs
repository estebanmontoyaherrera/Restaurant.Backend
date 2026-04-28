using SharedKernel.Primitive;

namespace Ordering.Domain.Entities;

public class Order : StateAuditableEntity
{
    public int TableNumber { get; set; }
    public string WaiterName { get; set; } = null!;
    public string Status { get; set; } = null!;

    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
