using DMoreno.CashFlowControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DMoreno.CashFlowControl.Infra.Data.Mappings;

public class AccountMap : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable(nameof(Account));
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name).HasColumnType("varchar").HasMaxLength(50).IsRequired(true);
        builder.Property(a => a.Description).HasColumnType("varchar").HasMaxLength(255).IsRequired(true);
    }
}