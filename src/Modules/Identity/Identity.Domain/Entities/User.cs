using SharedKernel.Primitive;

namespace Identity.Domain.Entities;

public class User : StateAuditableEntity
{
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Password { get; set; } = null!;
}
