using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence.Context.Configurations;

internal sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("OrderId");
        builder.Property(e => e.WaiterName).HasMaxLength(100);
        builder.Property(e => e.Status).HasMaxLength(30);

        builder.HasMany(e => e.OrderDetails)
            .WithOne(e => e.Order)
            .HasForeignKey(e => e.OrderId);
    }
}
