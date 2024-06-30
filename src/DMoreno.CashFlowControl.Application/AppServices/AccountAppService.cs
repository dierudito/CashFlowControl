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

public class AccountAppService(
    IUnitOfWork uow,
    IMapper mapper,
    ILogger<AccountAppService> logger,
    IAccountService accountService,
    IAccountRepository accountRepository,
    ITransactionRepository transactionRepository) :
    BaseAppService(uow), IAccountAppService
{
    public async Task<Response<AccountResponseViewModel>> AddAsync(AccountRequestViewModel accountRequestViewModel)
    {
        logger.LogInformation("Inicio do processo de adição de conta");

        var account = mapper.Map<Account>(accountRequestViewModel);

        try
        {

            if (account is null)
            {
                logger.LogWarning("Request vazia");

                return new(null, HttpStatusCode.BadRequest, "Não foi possível identificar os dados da conta");
            }

            logger.LogInformation("Adicionando a conta {CodAccount} na base de dados", account.Id.ToString());
            await accountService.AddAsync(account);

            await SaveChangesAsync();

            logger.LogInformation("Conta adicionada");
            var accountResponse = mapper.Map<AccountResponseViewModel>(account);
            return new(accountResponse, message: "Conta adicionada");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Não foi possível adicionar a conta {CodAccount} na base de dados", account.Id.ToString());
            return new(null, HttpStatusCode.InternalServerError, "Erro ao adicionar conta");
        }
    }

    public async Task<Response<bool>> UpdateAsync(AccountRequestViewModel accountRequestViewModel, Guid idAccount)
    {
        try
        {
            logger.LogInformation("Inicio do processo de alteração da conta {CodAccount}", idAccount.ToString());

            var account = mapper.Map<Account>(accountRequestViewModel);

            if (account is null)
            {
                logger.LogWarning("Request vazia");

                return new(false, HttpStatusCode.BadRequest, "Não foi possível identificar os dados da requisição");
            }

            logger.LogInformation("Atualizando a conta {CodAccount} na base de dados", idAccount.ToString());
            var accountUpdated = await accountService.UpdateAsync(account, idAccount);

            if (accountUpdated is null)
            {
                logger.LogInformation("Conta {CodAccount} não encontrada", idAccount.ToString());
                return new(false, HttpStatusCode.NotFound, "Conta não encontrada");
            }

            await SaveChangesAsync();

            logger.LogInformation("Conta atualizada");
            return new(true, message: "Conta atualizada");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Não foi possível atualizar a conta {CodAccount} na base de dados", idAccount.ToString());
            return new(false, HttpStatusCode.InternalServerError, "Erro ao atualizar conta");
        }
    }

    public async Task<Response<bool>> DeleteAsync(Guid idAccount)
    {
        try
        {
            logger.LogInformation("Inicio do processo de exclusão da conta {CodAccount}", idAccount.ToString());

            if (await transactionRepository.AreThereAsync(entity => entity.AccountId == idAccount))
            {
                logger.LogInformation("Não foi possível excluir a Conta {CodAccount}, pois está atribuída a transações.", idAccount.ToString());
                return new(false, HttpStatusCode.NotAcceptable, "Conta atribuída a transações");
            }

            logger.LogInformation("Excluíndo a conta {CodAccount} na base de dados", idAccount.ToString());
            await accountService.DeleteAsync(idAccount);

            await SaveChangesAsync();

            logger.LogInformation("Conta excluída");
            return new(true, message: "Conta excluída");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Não foi possível excluir a conta {CodAccount} na base de dados", idAccount.ToString());
            return new(false, HttpStatusCode.InternalServerError, "Erro ao excluir conta");
        }
    }

    public async Task<Response<AccountResponseViewModel>> GetByIdAsync(Guid id)
    {
        logger.LogInformation("Inicio do processo para obter a conta {CodAccount}", id.ToString());

        var account = await accountRepository.GetByIdAsync(id);

        if (account is null)
        {
            logger.LogInformation("Conta {CodTransaciton} não encontrada", id.ToString());
            return new(null, HttpStatusCode.NotFound, "Conta não encontrada");
        }

        logger.LogInformation("Conta {CodTransaciton} obtida", id.ToString());
        var AccountResponse = mapper.Map<AccountResponseViewModel>(account);

        return new(AccountResponse, message: "ok");
    }

    public async Task<Response<List<AccountResponseViewModel>>> GetAllAsync()
    {
        logger.LogInformation("Obtendo todas as contas");
        var accounts = await accountRepository.GetAllAsync();

        if (!accounts.Any())
        {
            logger.LogInformation("Nenhuma conta encontrada");
            return new(null, HttpStatusCode.NoContent, "Nenhuma conta encontrada");
        }

        var accountsResponse = mapper.Map<List<AccountResponseViewModel>>(accounts);
        return new(accountsResponse, message: "Contas encontradas");
    }
}