using DMoreno.CashFlowControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DMoreno.CashFlowControl.Infra.Data.Mappings;

public class TransactionMap : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Date).IsRequired(true);
        builder.Property(t => t.Type).IsRequired(true);
        builder.Property(t => t.Amount).HasColumnType("money").IsRequired(true);
        builder.Property(t => t.AccountId).IsRequired(false);
        builder.Property(t => t.CategoryId).IsRequired(false);
        builder.Property(t => t.Description)
            .HasColumnType("nvarchar")
            .HasMaxLength(500)
            .IsRequired(false);

        builder
            .HasOne(t => t.Account)
            .WithMany()
            .HasForeignKey(t => t.AccountId)
            .IsRequired(false);

        builder
            .HasOne(t => t.Category)
            .WithMany()
            .HasForeignKey(t => t.CategoryId)
            .IsRequired(false);
    }
}
