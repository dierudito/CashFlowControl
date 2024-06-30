using AutoMapper;
using Bogus;
using DMoreno.CashFlowControl.Application.AppServices;
using DMoreno.CashFlowControl.Application.ViewModels.Requests;
using DMoreno.CashFlowControl.Application.ViewModels.Responses;
using DMoreno.CashFlowControl.Domain.Entities;
using DMoreno.CashFlowControl.Domain.Interfaces.Repositories;
using DMoreno.CashFlowControl.Domain.Interfaces.Services;
using DMoreno.CashFlowControl.Domain.Interfaces.UoW;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using System.Linq.Expressions;
using System.Net;

namespace DMoreno.CashFlowControl.UnityTests.AppServices;

public class CategoryAppServiceTests
{
    private readonly Faker faker;
    private readonly Mock<IMapper> mapper;
    private readonly Mock<ICategoryService> categoryService;
    private readonly Mock<ICategoryRepository> categoryRepository;
    private readonly Mock<ITransactionRepository> transactionRepository;
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly CategoryAppService appService;

    public CategoryAppServiceTests()
    {
        var mocker = new AutoMocker();
        faker = new();

        mapper = mocker.GetMock<IMapper>();
        categoryService = mocker.GetMock<ICategoryService>();
        categoryRepository = mocker.GetMock<ICategoryRepository>();
        transactionRepository = mocker.GetMock<ITransactionRepository>();
        unitOfWork = mocker.GetMock<IUnitOfWork>();

        appService = mocker.CreateInstance<CategoryAppService>();
    }

    [Fact(DisplayName = "Should Add Category Successfully")]
    [Trait(nameof(CategoryAppService), nameof(CategoryAppService.AddAsync))]
    public async Task ShouldAddCategorySuccessfully()
    {
        // Arrange
        var request = new CategoryRequestViewModel(faker.Lorem.Word(), faker.Lorem.Text());
        var category = new Category { Description = request.Description, Name = request.Name };
        var categoryResponse = new CategoryResponseViewModel(category.Id, category.Name, category.Description);

        mapper
            .Setup(m => m.Map<Category>(It.IsAny<CategoryRequestViewModel>()))
            .Returns(category);
        mapper
            .Setup(m => m.Map<CategoryResponseViewModel>(It.IsAny<Category>()))
            .Returns(categoryResponse);

        // Act
        var response = await appService.AddAsync(request);

        // Assert
        response.Data.Should().BeEquivalentTo(categoryResponse);
        response.Code.Should().Be(HttpStatusCode.OK);
        categoryService.Verify(t => t.AddAsync(It.Is<Category>(entity =>
        entity.Id == category.Id &&
        entity.Name == request.Name &&
        entity.Description == request.Description)), Times.Once());

        unitOfWork.Verify(u => u.CommitAsync(), Times.Once());
    }

    [Fact(DisplayName = "Should Return BadRequest When Adding Category The Request Conversion Fails")]
    [Trait(nameof(CategoryAppService), nameof(CategoryAppService.AddAsync))]
    public async Task ShouldReturnBadRequestWhenAddingCategoryTheRequestConversionFails()
    {
        // Arrange
        var request = new CategoryRequestViewModel(faker.Lorem.Word(), faker.Lorem.Text());

        // Act
        var response = await appService.AddAsync(request);

        // Assert
        response.Code.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "Should Not Persist Data When Adding Category The Request Conversion Fails")]
    [Trait(nameof(CategoryAppService), nameof(CategoryAppService.AddAsync))]
    public async Task ShouldNotPersistDataWhenAddingCategoryTheRequestConversionFails()
    {
        // Arrange
        var request = new CategoryRequestViewModel(faker.Lorem.Word(), faker.Lorem.Text());

        // Act
        var response = await appService.AddAsync(request);

        // Assert
        categoryService.Verify(t => t.AddAsync(It.IsAny<Category>()), Times.Never());
    }

    [Fact(DisplayName = "Should Return InternalServerError When Adding Category The Error Occurs While Persisting Data")]
    [Trait(nameof(CategoryAppService), nameof(CategoryAppService.AddAsync))]
    public async Task ShouldReturnInternalServerErrorWhenAddingCategoryTheErrorOccursWhilePersistingData()
    {
        // Arrange
        var request = new CategoryRequestViewModel(faker.Lorem.Word(), faker.Lorem.Text());
        var category = new Category { Description = faker.Lorem.Text(), Name = faker.Lorem.Word() };

        mapper
            .Setup(m => m.Map<Category>(It.IsAny<CategoryRequestViewModel>()))
            .Returns(category);
        categoryService
            .Setup(u => u.AddAsync(It.IsAny<Category>()))
            .ThrowsAsync(new Exception());

        // Act
        var response = await appService.AddAsync(request);

        // Assert
        response.Code.Should().Be(HttpStatusCode.InternalServerError);
    }

    [Fact(DisplayName = "Should Return Category Correctly When Getting Category By Id")]
    [Trait(nameof(CategoryAppService), nameof(CategoryAppService.GetByIdAsync))]
    public async Task ShouldReturnCategoryCorrectlyGettingCategoryById()
    {
        // Arrange
        var id = Guid.NewGuid();
        var category = new Category { Description = faker.Lorem.Text(), Name = faker.Lorem.Word() };
        var categoryResponse = new CategoryResponseViewModel(category.Id, category.Name, category.Description);

        categoryRepository
            .Setup(t => t.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(category);

        mapper
            .Setup(m => m.Map<CategoryResponseViewModel>(It.IsAny<Category>()))
            .Returns(categoryResponse);

        // Act
        var response = await appService.GetByIdAsync(id);

        // Assert
        response.Code.Should().Be(HttpStatusCode.OK);
        response.Data.Should().BeEquivalentTo(categoryResponse);
    }

    [Fact(DisplayName = "Should Return NotFound When Getting Category By Id Not Found The Category")]
    [Trait(nameof(CategoryAppService), nameof(CategoryAppService.GetByIdAsync))]
    public async Task ShouldReturnNotFoundWhenGettingCategoryByIdNotFoundTheCategory()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var response = await appService.GetByIdAsync(id);

        // Assert
        response.Code.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Should Update Category Successfully")]
    [Trait(nameof(CategoryAppService), nameof(CategoryAppService.UpdateAsync))]
    public async Task ShouldUpdateCategorySuccessfully()
    {
        // Arrange
        var idCategory = Guid.NewGuid();

        var request = new CategoryRequestViewModel(faker.Lorem.Word(), faker.Lorem.Text());
        var category = new Category { Description = request.Description, Name = request.Name, Id = idCategory };

        categoryService
            .Setup(t => t.UpdateAsync(It.IsAny<Category>(), It.IsAny<Guid>()))
            .ReturnsAsync(category);
        mapper
            .Setup(m => m.Map<Category>(It.IsAny<CategoryRequestViewModel>()))
            .Returns(category);

        // Act
        var response = await appService.UpdateAsync(request, idCategory);

        // Assert
        response.Code.Should().Be(HttpStatusCode.OK);
        categoryService.Verify(t => t.UpdateAsync(It.Is<Category>(entity =>
        entity.Id == idCategory &&
        entity.Name == request.Name &&
        entity.Description == request.Description), idCategory), Times.Once());

        unitOfWork.Verify(u => u.CommitAsync(), Times.Once());
    }

    [Fact(DisplayName = "Should Return NotFound When Updating Category Not Found The Category")]
    [Trait(nameof(CategoryAppService), nameof(CategoryAppService.UpdateAsync))]
    public async Task ShouldReturnNotFoundWhenUpdatingCategoryNotFoundTheCategory()
    {
        // Arrange
        var idCategory = Guid.NewGuid();
        var request = new CategoryRequestViewModel(faker.Lorem.Word(), faker.Lorem.Text());
        var category = new Category { Description = request.Description, Name = request.Name, Id = idCategory };

        mapper
            .Setup(m => m.Map<Category>(It.IsAny<CategoryRequestViewModel>()))
            .Returns(category);

        // Act
        var response = await appService.UpdateAsync(request, idCategory);

        // Assert
        response.Code.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Should Return BadRequest When Updating Category The Request Conversion Fails")]
    [Trait(nameof(CategoryAppService), nameof(CategoryAppService.UpdateAsync))]
    public async Task ShouldReturnBadRequestWhenUpdatingCategoryTheRequestConversionFails()
    {
        // Arrange
        var idCategory = Guid.NewGuid();
        var request = new CategoryRequestViewModel(faker.Lorem.Word(), faker.Lorem.Text());

        // Act
        var response = await appService.UpdateAsync(request, idCategory);

        // Assert
        response.Code.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "Should Not Persist Data When Updating Category The Request Conversion Fails")]
    [Trait(nameof(CategoryAppService), nameof(CategoryAppService.UpdateAsync))]
    public async Task ShouldNotPersistDataWhenUpdatingCategoryTheRequestConversionFails()
    {
        // Arrange
        var idCategory = Guid.NewGuid();
        var request = new CategoryRequestViewModel(faker.Lorem.Word(), faker.Lorem.Text());

        // Act
        var response = await appService.UpdateAsync(request, idCategory);

        // Assert
        categoryService.Verify(t => t.UpdateAsync(It.IsAny<Category>(), It.IsAny<Guid>()), Times.Never());
    }

    [Fact(DisplayName = "Should Return InternalServerError When Updating Category The Error Occurs While Persisting Data")]
    [Trait(nameof(CategoryAppService), nameof(CategoryAppService.UpdateAsync))]
    public async Task ShouldReturnInternalServerErrorWhenUpdatingCategoryTheErrorOccursWhilePersistingData()
    {
        // Arrange
        var idCategory = Guid.NewGuid();
        var request = new CategoryRequestViewModel(faker.Lorem.Word(), faker.Lorem.Text());
        var category = new Category { Description = request.Description, Name = request.Name, Id = idCategory };

        mapper
            .Setup(m => m.Map<Category>(It.IsAny<CategoryRequestViewModel>()))
            .Returns(category);
        categoryService
            .Setup(t => t.UpdateAsync(It.IsAny<Category>(), It.IsAny<Guid>()))
            .ThrowsAsync(new Exception());

        // Act
        var response = await appService.UpdateAsync(request, idCategory);

        // Assert
        response.Code.Should().Be(HttpStatusCode.InternalServerError);
    }










    [Fact(DisplayName = "Should Delete Category Successfully")]
    [Trait(nameof(CategoryAppService), nameof(CategoryAppService.DeleteAsync))]
    public async Task ShouldDeleteCategorySuccessfully()
    {
        // Arrange
        var idCategory = Guid.NewGuid();

        // Act
        var response = await appService.DeleteAsync(idCategory);

        // Assert
        response.Code.Should().Be(HttpStatusCode.OK);
        categoryService.Verify(t => t.DeleteAsync(idCategory), Times.Once());

        unitOfWork.Verify(u => u.CommitAsync(), Times.Once());
    }

    [Fact(DisplayName = "Should Return InternalServerError When Deleting Category The Error Occurs While Persisting Data")]
    [Trait(nameof(CategoryAppService), nameof(CategoryAppService.DeleteAsync))]
    public async Task ShouldReturnInternalServerErrorWhenDeletingCategoryTheErrorOccursWhilePersistingData()
    {
        // Arrange
        var idCategory = Guid.NewGuid();

        categoryService
            .Setup(u => u.DeleteAsync(It.IsAny<Guid>()))
            .ThrowsAsync(new Exception());

        // Act
        var response = await appService.DeleteAsync(idCategory);

        // Assert
        response.Code.Should().Be(HttpStatusCode.InternalServerError);
    }

    [Fact(DisplayName = "Should Return NotAcceptable When Attempting To Delete a Category That Is Currently Assigned To a Transaction")]
    [Trait(nameof(CategoryAppService), nameof(CategoryAppService.DeleteAsync))]
    public async Task ShouldReturnNotAcceptableWhenAttemptingToDeleteCategoryThatIsCurrentlyAssignedToTransaction()
    {
        // Arrange
        var idCategory = Guid.NewGuid();

        transactionRepository
            .Setup(t => t.AreThereAsync(It.IsAny<Expression<Func<Transaction, bool>>>()))
            .ReturnsAsync(true);

        // Act
        var response = await appService.DeleteAsync(idCategory);

        // Assert
        response.Code.Should().Be(HttpStatusCode.NotAcceptable);
    }

    [Fact(DisplayName = "Should Not Persist Data When Attempting To Delete a Category That Is Currently Assigned To a Transaction")]
    [Trait(nameof(CategoryAppService), nameof(CategoryAppService.DeleteAsync))]
    public async Task ShouldNotPersistDataWhenAttemptingToDeleteCategoryThatIsCurrentlyAssignedToTransaction()
    {
        // Arrange
        var idCategory = Guid.NewGuid();

        transactionRepository
            .Setup(t => t.AreThereAsync(It.IsAny<Expression<Func<Transaction, bool>>>()))
            .ReturnsAsync(true);

        // Act
        var response = await appService.DeleteAsync(idCategory);

        // Assert
        categoryService.Verify(t => t.DeleteAsync(It.IsAny<Guid>()), Times.Never());

        unitOfWork.Verify(u => u.CommitAsync(), Times.Never());
    }

    [Fact(DisplayName = "Should Get All Categories Currently")]
    [Trait(nameof(CategoryAppService), nameof(CategoryAppService.GetAllAsync))]
    public async Task ShouldGetAllCategoriesCurrently()
    {
        // Arrange
        var category = new Category { Description = faker.Lorem.Text(), Name = faker.Lorem.Word() };
        var categoryResponse = new CategoryResponseViewModel(category.Id, category.Name, category.Description);
        List<CategoryResponseViewModel> categoriesResponse = [categoryResponse];

        categoryRepository
            .Setup(c => c.GetAllAsync())
            .ReturnsAsync([category]);

        mapper
            .Setup(m => m.Map<List<CategoryResponseViewModel>>(It.IsAny<IEnumerable<Category>>()))
            .Returns(categoriesResponse);

        // Act
        var response = await appService.GetAllAsync();

        // Assert
        response.Data.Should().HaveCount(categoriesResponse.Count);
        response.Data.Should().BeEquivalentTo(categoriesResponse);
    }

    [Fact(DisplayName = "Should Return NoContent Get All Categories When There Aren't Categories")]
    [Trait(nameof(CategoryAppService), nameof(CategoryAppService.GetAllAsync))]
    public async Task ShouldReturnNoContentGetAllCategoriesWhenThereAreNotCategories()
    {
        // Arrange

        // Act
        var response = await appService.GetAllAsync();

        // Assert
        response.Code.Should().Be(HttpStatusCode.NoContent);
    }
}