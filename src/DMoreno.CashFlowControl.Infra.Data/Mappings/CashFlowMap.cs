using DMoreno.CashFlowControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DMoreno.CashFlowControl.Infra.Data.Mappings;
public class CashFlowMap : IEntityTypeConfiguration<CashFlow>
{
    public void Configure(EntityTypeBuilder<CashFlow> builder)
    {
        builder.ToTable("CashFlows");
        builder.HasKey(c => c.Id);
        builder.HasIndex(c => c.ReleaseDate).IsUnique(true);

        builder.Property(c => c.ReleaseDate).HasColumnType("date").IsRequired(true);
        builder.Property(c => c.OpeningBalance).HasColumnType("money").IsRequired(true);
        builder.Property(c => c.TotalCredits).HasColumnType("money").IsRequired(true);
        builder.Property(c => c.TotalDebits).HasColumnType("money").IsRequired(true);
        builder.Property(c => c.ClosingBalance).HasColumnType("money").IsRequired(true);
    }
}
