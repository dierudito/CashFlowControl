using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DMoreno.CashFlowControl.Infra.Data.Context;

public class CashFlowControlDbContext(DbContextOptions<CashFlowControlDbContext> options) :
    DbContext(options)
{

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}