using DMoreno.CashFlowControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DMoreno.CashFlowControl.Infra.Data.Mappings;

public class CategoryMap : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name).HasColumnType("varchar").HasMaxLength(50).IsRequired(true);
        builder.Property(c => c.Description).HasColumnType("varchar").HasMaxLength(255).IsRequired(true);
    }
}
