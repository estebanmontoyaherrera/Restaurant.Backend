namespace SharedKernel.Primitive
{
    public abstract class StateAuditableEntity : AuditableEntity
    {
        public string? State { get; set; }
    }
}
