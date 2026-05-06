using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleItemTests
{
    private readonly Guid _saleId = Guid.NewGuid();
    private readonly Guid _productId = Guid.NewGuid();

    [Fact]
    public void Should_Not_Apply_Discount_When_Quantity_Is_Less_Than_4()
    {
        var item = new SaleItem(_saleId, _productId, "Product A", 3, 100);

        Assert.Equal(0m, item.DiscountPercentage);
        Assert.Equal(0m, item.DiscountAmount);
        Assert.Equal(300m, item.TotalAmount);
    }

    [Fact]
    public void Should_Apply_10_Percent_Discount_When_Quantity_Is_Between_4_And_9()
    {
        var item = new SaleItem(_saleId, _productId, "Product A", 4, 100);

        Assert.Equal(0.10m, item.DiscountPercentage);
        Assert.Equal(40m, item.DiscountAmount);
        Assert.Equal(360m, item.TotalAmount);
    }

    [Fact]
    public void Should_Apply_20_Percent_Discount_When_Quantity_Is_Between_10_And_20()
    {
        var item = new SaleItem(_saleId, _productId, "Product A", 10, 100);

        Assert.Equal(0.20m, item.DiscountPercentage);
        Assert.Equal(200m, item.DiscountAmount);
        Assert.Equal(800m, item.TotalAmount);
    }

    [Fact]
    public void Should_Throw_DomainException_When_Quantity_Is_Greater_Than_20()
    {
        var exception = Assert.Throws<DomainException>(() =>
            new SaleItem(_saleId, _productId, "Product A", 21, 100));

        Assert.Equal("Item quantity cannot exceed 20 units", exception.Message);
    }

    [Fact]
    public void Should_Throw_DomainException_When_Quantity_Is_Zero()
    {
        var exception = Assert.Throws<DomainException>(() =>
            new SaleItem(_saleId, _productId, "Product A", 0, 100));

        Assert.Equal("Item quantity must be greater than 0", exception.Message);
    }

    [Fact]
    public void Should_Throw_DomainException_When_UnitPrice_Is_Zero()
    {
        var exception = Assert.Throws<DomainException>(() =>
            new SaleItem(_saleId, _productId, "Product A", 1, 0));

        Assert.Equal("Unit price must be greater than 0", exception.Message);
    }
}