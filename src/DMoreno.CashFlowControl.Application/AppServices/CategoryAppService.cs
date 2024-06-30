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

public class CategoryAppService(
    IUnitOfWork uow,
    IMapper mapper,
    ILogger<CategoryAppService> logger,
    ICategoryService categoryService,
    ICategoryRepository categoryRepository,
    ITransactionRepository transactionRepository) :
    BaseAppService(uow), ICategoryAppService
{
    public async Task<Response<CategoryResponseViewModel>> AddAsync(CategoryRequestViewModel categoryRequestViewModel)
    {
        logger.LogInformation("Inicio do processo de adição de categoria");

        var category = mapper.Map<Category>(categoryRequestViewModel);

        try
        {

            if (category is null)
            {
                logger.LogWarning("Request vazia");

                return new(null, HttpStatusCode.BadRequest, "Não foi possível identificar os dados da categoria");
            }

            logger.LogInformation("Adicionando a categoria {CodCategory} na base de dados", category.Id.ToString());
            await categoryService.AddAsync(category);

            await SaveChangesAsync();

            logger.LogInformation("Categoria adicionada");
            var categoryResponse = mapper.Map<CategoryResponseViewModel>(category);
            return new(categoryResponse, message: "Categoria adicionada");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Não foi possível adicionar a categoria {CodCategory} na base de dados", category.Id.ToString());
            return new(null, HttpStatusCode.InternalServerError, "Erro ao adicionar categoria");
        }
    }

    public async Task<Response<bool>> UpdateAsync(CategoryRequestViewModel categoryRequestViewModel, Guid idCategory)
    {
        try
        {
            logger.LogInformation("Inicio do processo de alteração da categoria {CodCategory}", idCategory.ToString());

            var category = mapper.Map<Category>(categoryRequestViewModel);

            if (category is null)
            {
                logger.LogWarning("Request vazia");

                return new(false, HttpStatusCode.BadRequest, "Não foi possível identificar os dados da requisição");
            }

            logger.LogInformation("Atualizando a categoria {CodCategory} na base de dados", idCategory.ToString());

            var categoryDb = await categoryService.UpdateAsync(category, idCategory);

            if (categoryDb is null)
            {
                logger.LogInformation("Categoria {CodCategory} não encontrada", idCategory.ToString());
                return new(false, HttpStatusCode.NotFound, "Categoria não encontrada");
            }

            await SaveChangesAsync();

            logger.LogInformation("Categoria atualizada");
            return new(true, message: "Categoria atualizada");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Não foi possível atualizar a categoria {CodCategory} na base de dados", idCategory.ToString());
            return new(false, HttpStatusCode.InternalServerError, "Erro ao atualizar categoria");
        }
    }

    public async Task<Response<bool>> DeleteAsync(Guid idCategory)
    {
        try
        {
            logger.LogInformation("Inicio do processo de exclusão da categoria {CodCategory}", idCategory.ToString());

            if (await transactionRepository.AreThereAsync(entity => entity.CategoryId == idCategory))
            {
                logger.LogInformation("Não foi possível excluir a Categoria {CodCategory}, pois está atribuída a transações.", idCategory.ToString());
                return new(false, HttpStatusCode.NotAcceptable, "Categoria atribuída a transações");
            }

            logger.LogInformation("Excluíndo a categoria {CodCategory} na base de dados", idCategory.ToString());
            await categoryService.DeleteAsync(idCategory);

            await SaveChangesAsync();

            logger.LogInformation("Categoria excluída");
            return new(true, message: "Categoria excluída");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Não foi possível excluir a categoria {CodCategory} na base de dados", idCategory.ToString());
            return new(false, HttpStatusCode.InternalServerError, "Erro ao excluir categoria");
        }
    }

    public async Task<Response<CategoryResponseViewModel>> GetByIdAsync(Guid id)
    {
        logger.LogInformation("Inicio do processo para obter a categoria {CodCategory}", id.ToString());

        var category = await categoryRepository.GetByIdAsync(id);

        if (category is null)
        {
            logger.LogInformation("Categoria {CodTransaciton} não encontrada", id.ToString());
            return new(null, HttpStatusCode.NotFound, "Categoria não encontrada");
        }

        logger.LogInformation("Categoria {CodTransaciton} obtida", id.ToString());
        var categoryResponse = mapper.Map<CategoryResponseViewModel>(category);

        return new(categoryResponse, message: "ok");
    }

    public async Task<Response<List<CategoryResponseViewModel>>> GetAllAsync()
    {
        logger.LogInformation("Obtendo todas as categorias");
        var categorys = await categoryRepository.GetAllAsync();

        if (!categorys.Any())
        {
            logger.LogInformation("Nenhuma categoria encontrada");
            return new(null, HttpStatusCode.NoContent, "Nenhuma categoria encontrada");
        }

        var categorysResponse = mapper.Map<List<CategoryResponseViewModel>>(categorys);
        return new(categorysResponse, message: "Categorias encontradas");
    }
}