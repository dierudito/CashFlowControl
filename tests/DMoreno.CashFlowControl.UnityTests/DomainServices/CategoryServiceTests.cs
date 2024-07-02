using Bogus;
using DMoreno.CashFlowControl.Domain.Entities;
using DMoreno.CashFlowControl.Domain.Interfaces.Repositories;
using DMoreno.CashFlowControl.Domain.Services;
using FluentAssertions;
using Moq;
using Moq.AutoMock;

namespace DMoreno.CashFlowControl.UnityTests.DomainServices;

public class CategoryServiceTests
{
    private readonly Faker faker;
    private readonly Mock<ICategoryRepository> categoryRepository;
    private readonly CategoryService service;

    public CategoryServiceTests()
    {
        faker = new();
        var mocker = new AutoMocker();
        categoryRepository = mocker.GetMock<ICategoryRepository>();
        service = mocker.CreateInstance<CategoryService>();
    }

    [Fact(DisplayName = "Should Add Category Successfully")]
    [Trait(nameof(CategoryService), nameof(CategoryService.AddAsync))]
    public async Task ShouldAddCategorySuccessfully()
    {
        // Arrange
        var category = new Category { Name = faker.Lorem.Word(), Description = faker.Lorem.Word() };

        categoryRepository
            .Setup(t => t.AddAsync(It.IsAny<Category>()))
            .ReturnsAsync(category);

        // Act
        var response = await service.AddAsync(category);

        // Assert
        response.Should().BeEquivalentTo(category);
        categoryRepository.Verify(t => t.AddAsync(It.Is<Category>(entity =>
        entity.Id == category.Id &&
        entity.Name == category.Name &&
        entity.Description == category.Description
        )), Times.Once());
    }

    [Fact(DisplayName = "Should Update Category Successfully")]
    [Trait(nameof(CategoryService), nameof(CategoryService.UpdateAsync))]
    public async Task ShouldUpdateCategorySuccessfully()
    {
        // Arrange
        var category = new Category { Name = faker.Lorem.Word(), Description = faker.Lorem.Word() };
        var idCategory = Guid.NewGuid();

        categoryRepository
            .Setup(t => t.UpdateAsync(It.IsAny<Category>(), It.IsAny<Guid>()))
            .ReturnsAsync(category);

        // Act
        var response = await service.UpdateAsync(category, idCategory);

        // Assert
        response.Should().BeEquivalentTo(category);
        categoryRepository.Verify(t => t.UpdateAsync(It.Is<Category>(entity =>
        entity.Id == category.Id &&
        entity.Name == category.Name &&
        entity.Description == category.Description
        ), idCategory), Times.Once());
    }

    [Fact(DisplayName = "Should Delete Category Successfully")]
    [Trait(nameof(CategoryService), nameof(CategoryService.DeleteAsync))]
    public async Task ShouldDeleteCategorySuccessfully()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        await service.DeleteAsync(id);

        // Assert
        categoryRepository.Verify(t => t.DeleteAsync(id), Times.Once());
    }
}
