using AutoMapper;
using DMoreno.CashFlowControl.Application.AppServices.Base;
using DMoreno.CashFlowControl.Application.Interfaces;
using DMoreno.CashFlowControl.Application.ViewModels.Requests;
using DMoreno.CashFlowControl.Application.ViewModels.Responses;
using DMoreno.CashFlowControl.Domain.Entities;
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
    ITransactionRepository transactionRepository) :
    BaseAppService(uow), ITransactionAppService
{
    public async Task<Response<AddTransactionResponseViewModel>> AddAsync(AddTransactionRequestViewModel addTransactionRequestViewModel)
    {
        logger.LogInformation("Inicio do processo de adição de transação");

        var transaction = mapper.Map<Transaction>(addTransactionRequestViewModel);

        if (transaction is null)
        {
            logger.LogWarning("Request vazia");

            return new(null, HttpStatusCode.BadRequest, "Não foi possível identificar os dados da transação");
        }

        try
        {
            logger.LogInformation("Adicionando a transação {CodTransaction} na base de dados", transaction.Id.ToString());
            await transactionService.AddAsync(transaction);

            if (!await SaveChangesAsync())
            {
                logger.LogError("Não foi possível adicionar a transação {CodTransaction} na base de dados", transaction.Id.ToString());
                return new(null, HttpStatusCode.UnprocessableContent, "Transação não adicionada");
            }

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

    public async Task<Response<GetTransactionByIdResponseViewModel>> GetByIdAsync(Guid id)
    {
        logger.LogInformation("Inicio do processo para obter a transação {CodTransaction}", id.ToString());

        var transaction = await transactionRepository.GetByIdAsync(id);

        if (transaction is null)
        {
            logger.LogInformation("Transação {CodTransaciton} não encontrada", id.ToString());
            return new(null, HttpStatusCode.BadRequest, "Transação não encontrada");
        }

        logger.LogInformation("Transação {CodTransaciton} obtida", id.ToString());
        var transactionResponse = mapper.Map<GetTransactionByIdResponseViewModel>(transaction);

        return new(transactionResponse, message: "ok");
    }
}
