using Bogus;
using DMoreno.CashFlowControl.Domain.Extensions;
using FluentAssertions;
using System;

namespace DMoreno.CashFlowControl.UnityTests.Extensions;

public class DateTimeExtensionTests
{
    private readonly Faker faker = new();

    [Theory(DisplayName = "Should Get First Day Currectly")]
    [InlineData(2024, 7)]
    [InlineData(null, null)]
    [Trait(nameof(DateTimeExtension), nameof(DateTimeExtension.GetFirstDay))]
    public void ShouldGetFirstDayCurrectly(int? year, int? month)
    {
        // Arrange
        var date = faker.Date.Recent();
        var dateExpected = new DateTime(year ?? date.Year, month ?? date.Month, 1);

        // Act
        var response = date.GetFirstDay(year, month);

        // Assert
        response.Should().Be(dateExpected);
    }

    [Theory(DisplayName = "Should Get Last Day Currectly")]
    [InlineData(2024, 7)]
    [InlineData(null, null)]
    [Trait(nameof(DateTimeExtension), nameof(DateTimeExtension.GetLastDay))]
    public void ShouldGetLastDayCurrectly(int? year, int? month)
    {
        // Arrange
        var date = faker.Date.Recent();
        var firstDay = new DateTime(year ?? date.Year, month ?? date.Month, 1);
        var dateExpected = firstDay.AddMonths(1).AddDays(-1);

        // Act
        var response = date.GetLastDay(year, month);

        // Assert
        response.Should().Be(dateExpected);
    }

    [Fact(DisplayName = "Should Get Date Only Currectly")]
    [Trait(nameof(DateTimeExtension), nameof(DateTimeExtension.DateOnly))]
    public void ShouldGetDateOnlyCurrectly()
    {
        // Arrange
        var date = faker.Date.Recent();
        var dateExpected = DateOnly.FromDateTime(date);

        // Act
        var response = date.DateOnly();

        // Assert
        response.Should().Be(dateExpected);
    }

    [Fact(DisplayName = "Should Return Null Get Date Only From Empty Date")]
    [Trait(nameof(DateTimeExtension), nameof(DateTimeExtension.DateOnly))]
    public void ShouldReturnNullGetDateOnlyFromEmptyDate()
    {
        // Arrange
        DateTime? date = null;

        // Act
        var response = date.DateOnly();

        // Assert
        response.Should().BeNull();
    }
}