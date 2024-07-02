namespace DMoreno.CashFlowControl.Application.ViewModels.Responses;

public record DailyConsolidatedBalanceResponseViewModel
{
    public DailyConsolidatedBalanceResponseViewModel() { }

    public DailyConsolidatedBalanceResponseViewModel(string date, string openingBalance, string totalCredits, string totalDebits, string closingBalance)
    {
        Date = date;
        OpeningBalance = openingBalance;
        TotalCredits = totalCredits;
        TotalDebits = totalDebits;
        ClosingBalance = closingBalance;
    }

    public string Date { get; init; }
    public string OpeningBalance { get; init; }
    public string TotalCredits { get; init; }
    public string TotalDebits { get; init; }
    public string ClosingBalance { get; init; }
}
