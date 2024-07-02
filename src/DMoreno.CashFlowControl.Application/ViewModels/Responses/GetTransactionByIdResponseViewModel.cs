namespace DMoreno.CashFlowControl.Application.ViewModels.Responses;

public record GetTransactionByIdResponseViewModel 
{
    public GetTransactionByIdResponseViewModel() { }

    public GetTransactionByIdResponseViewModel(
    Guid id,
    string date,
    string amount,
    string? description,
    Guid? categoryId,
    Guid? accountId)
    {
        Id = id;
        Date = date;
        Amount = amount;
        Description = description;
        CategoryId = categoryId;
        AccountId = accountId;
    }

    public Guid Id { get; set; }
    public string Date { get; set; }
    public string Amount { get; set; }
    public string? Description { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? AccountId { get; set; }
}