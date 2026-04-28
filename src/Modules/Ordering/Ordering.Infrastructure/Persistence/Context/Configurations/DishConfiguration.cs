using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence.Context.Configurations;

internal sealed class DishConfiguration : IEntityTypeConfiguration<Dish>
{
    public void Configure(EntityTypeBuilder<Dish> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("DishId");
        builder.Property(e => e.Name).HasMaxLength(150);
        builder.Property(e => e.Description).HasMaxLength(255);
        builder.Property(e => e.Category).HasMaxLength(100);

        builder.HasIndex(e => e.Name).IsUnique();

        builder.HasMany(e => e.OrderDetails)
            .WithOne(e => e.Dish)
            .HasForeignKey(e => e.DishId);
    }
}
