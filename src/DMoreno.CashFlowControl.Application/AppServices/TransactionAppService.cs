using AutoMapper;
using DMoreno.CashFlowControl.Application.AppServices.Base;
using DMoreno.CashFlowControl.Application.Interfaces;
using DMoreno.CashFlowControl.Application.ViewModels.Requests;
using DMoreno.CashFlowControl.Application.ViewModels.Responses;
using DMoreno.CashFlowControl.Domain.Entities;
using DMoreno.CashFlowControl.Domain.Extensions;
using DMoreno.CashFlowControl.Domain.Interfaces.Repositories;
using DMoreno.CashFlowControl.Domain.Interfaces.Services;
using DMoreno.CashFlowControl.Domain.Interfaces.UoW;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DMoreno.CashFlowControl.Application.AppServices;
public class TransactionAppService(
    IUnitOfWork uow,
    IMapper mapper,
    ILogger<TransactionAppService> logger,
    ITransactionService transactionService,
    ITransactionRepository transactionRepository,
    IAccountRepository accountRepository,
    ICategoryRepository categoryRepository,
    ICashFlowService cashFlowService) :
    BaseAppService(uow), ITransactionAppService
{
    public async Task<Response<AddTransactionResponseViewModel>> AddAsync(AddTransactionRequestViewModel addTransactionRequestViewModel)
    {
        logger.LogInformation("Inicio do processo de adição de transação");

        var transaction = mapper.Map<Transaction>(addTransactionRequestViewModel);

        try
        {
            if (transaction is null)
            {
                logger.LogWarning("Request vazia");

                return new(null, HttpStatusCode.BadRequest, "Não foi possível identificar os dados da transação");
            }

            var cashFlow = await cashFlowService.GetOrCreateByDateAsync(DateTime.Now.DateOnly());
            cashFlow.IncrementTotals(transaction.Type, transaction.Amount);
            transaction.CashFlowId = cashFlow.Id;
            await cashFlowService.UpdateAsync(cashFlow, cashFlow.Id);

            logger.LogInformation("Adicionando a transação {CodTransaction} na base de dados", transaction.Id.ToString());
            await transactionService.AddAsync(transaction);

            await SaveChangesAsync();

            logger.LogInformation("Transação adicionada");
            var transactionResponse = mapper.Map<AddTransactionResponseViewModel>(transaction);
            return new(transactionResponse, message: "Transação adicionada");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Não foi possível adicionar a transação {CodTransaction} na base de dados", transaction.Id.ToString());
            return new(null, HttpStatusCode.InternalServerError, "Erro ao adicionar transação");
        }
    }

    public async Task<Response<bool>> UpdateAsync(UpdateTransactionRequestViewModel updateTransactionRequestViewModel, Guid idTransaction)
    {
        try
        {
            logger.LogInformation("Inicio do processo de alteração da transação {CodTransaction}", idTransaction.ToString());

            if (!await IsTheAccountRightAsync(updateTransactionRequestViewModel.AccountId))
            {
                logger.LogInformation("Conta {CodAccount} não encontrada para a transação {CodTransaction}", updateTransactionRequestViewModel.AccountId, idTransaction.ToString());
                return new(false, HttpStatusCode.BadRequest, "Conta não encontrada");
            }

            if (!await IsTheCategoryRightAsync(updateTransactionRequestViewModel.CategoryId))
            {
                logger.LogInformation("Categoria {CodCategory} não encontrada para a transação {CodTransaction}", updateTransactionRequestViewModel.CategoryId, idTransaction.ToString());
                return new(false, HttpStatusCode.BadRequest, "Categoria não encontrada");
            }

            var transaction = mapper.Map<Transaction>(updateTransactionRequestViewModel);

            if (transaction is null)
            {
                logger.LogWarning("Request vazia");

                return new(false, HttpStatusCode.BadRequest, "Não foi possível identificar os dados da requisição");
            }

            var transactionDb = await transactionRepository.GetByIdAsync(idTransaction);

            if (transactionDb is null)
            {
                logger.LogInformation("Transação {CodTransaction} não encontrada", idTransaction.ToString());
                return new(false, HttpStatusCode.NotFound, "Transação não encontrada");
            }

            if (transactionDb.CashFlow.ReleaseDate != DateTime.Now.DateOnly())
            {
                logger.LogWarning("Tentativa de alteração de transação de dias anteriores");
                return new(false, HttpStatusCode.Unauthorized, "Não é possível alterar transação de dias anteriores");
            }

            transactionDb.CashFlow.UpdateTotals(transactionDb.Type, transactionDb.Amount, transaction.Type, transaction.Amount);
            transaction.CashFlow = transactionDb.CashFlow;
            transaction.CashFlowId = transactionDb.CashFlowId;

            logger.LogInformation("Atualizando a transação {CodTransaction} na base de dados", idTransaction.ToString());
            await transactionService.UpdateAsync(transaction, idTransaction);            

            await SaveChangesAsync();

            logger.LogInformation("Transação atualizada");
            return new(true, message: "Transação atualizada");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Não foi possível atualizar a transação {CodTransaction} na base de dados", idTransaction.ToString());
            return new(false, HttpStatusCode.InternalServerError, "Erro ao atualizar transação");
        }
    }

    public async Task<Response<bool>> DeleteAsync(Guid idTransaction)
    {
        try
        {
            logger.LogInformation("Inicio do processo de exclusão da transação {CodTransaction}", idTransaction.ToString());

            var transactionDb = await transactionRepository.GetByIdAsync(idTransaction);

            if (transactionDb is null)
            {
                logger.LogInformation("Transação {CodTransaction} não encontrada", idTransaction.ToString());
                return new(false, HttpStatusCode.NotFound, "Transação não encontrada");
            }

            if (transactionDb.CashFlow.ReleaseDate != DateTime.Now.DateOnly())
            {
                logger.LogWarning("Tentativa de exclusão de transação de dias anteriores");
                return new(false, HttpStatusCode.Unauthorized, "Não é possível excluir transação de dias anteriores");
            }
            transactionDb.CashFlow.UpdateTotals(transactionDb.Type, transactionDb.Amount);

            logger.LogInformation("Excluíndo a transação {CodTransaction} na base de dados", idTransaction.ToString());
            await transactionService.DeleteAsync(idTransaction);
            await cashFlowService.UpdateAsync(transactionDb.CashFlow, transactionDb.CashFlowId);

            await SaveChangesAsync();

            logger.LogInformation("Transação excluída");
            return new(true, message: "Transação excluída");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Não foi possível excluir a transação {CodTransaction} na base de dados", idTransaction.ToString());
            return new(false, HttpStatusCode.InternalServerError, "Erro ao excluir transação");
        }
    }

    public async Task<Response<GetTransactionByIdResponseViewModel>> GetByIdAsync(Guid id)
    {
        logger.LogInformation("Inicio do processo para obter a transação {CodTransaction}", id.ToString());

        var transaction = await transactionRepository.GetByIdAsync(id);

        if (transaction is null)
        {
            logger.LogInformation("Transação {CodTransaciton} não encontrada", id.ToString());
            return new(null, HttpStatusCode.NotFound, "Transação não encontrada");
        }

        logger.LogInformation("Transação {CodTransaciton} obtida", id.ToString());
        var transactionResponse = mapper.Map<GetTransactionByIdResponseViewModel>(transaction);

        return new(transactionResponse, message: "ok");
    }

    private async Task<bool> IsTheCategoryRightAsync(Guid? idCategory) =>
        !idCategory.HasValue || (await categoryRepository.GetByIdAsync(idCategory.Value)) != null;

    private async Task<bool> IsTheAccountRightAsync(Guid? idAccount) =>
        !idAccount.HasValue || (await accountRepository.GetByIdAsync(idAccount.Value)) != null;
}
