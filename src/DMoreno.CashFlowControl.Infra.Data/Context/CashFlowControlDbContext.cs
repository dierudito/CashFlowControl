using DMoreno.CashFlowControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DMoreno.CashFlowControl.Infra.Data.Context;

public class CashFlowControlDbContext(DbContextOptions<CashFlowControlDbContext> options) :
    DbContext(options)
{
    public DbSet<Transaction> Transactions { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Account> Accounts { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}