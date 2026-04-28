using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence.Context.Configurations;

internal sealed class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("OrderDetailId");
        builder.Property(e => e.Notes).HasMaxLength(255);

        builder.HasOne(e => e.Order)
            .WithMany(e => e.OrderDetails)
            .HasForeignKey(e => e.OrderId);

        builder.HasOne(e => e.Dish)
            .WithMany(e => e.OrderDetails)
            .HasForeignKey(e => e.DishId);
    }
}
