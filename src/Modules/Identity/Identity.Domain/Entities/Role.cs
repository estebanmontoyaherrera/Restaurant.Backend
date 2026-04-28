using SharedKernel.Primitive;

namespace Identity.Domain.Entities;

public class Role : StateAuditableEntity
{
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
}
