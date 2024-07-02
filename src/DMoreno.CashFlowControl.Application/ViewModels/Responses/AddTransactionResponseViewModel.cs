namespace DMoreno.CashFlowControl.Application.ViewModels.Responses;

public record AddTransactionResponseViewModel
{
    public AddTransactionResponseViewModel()
    {
        
    }

    public AddTransactionResponseViewModel(
    Guid id,
    string amount,
    string? description)
    {
        Id = id;
        Amount = amount;
        Description = description;
    }

    public Guid Id { get; set; }
    public string Amount { get; set; }
    public string? Description { get; set; }
}
