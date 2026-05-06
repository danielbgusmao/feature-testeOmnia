using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Xunit;
using System.Linq;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleTests
{
    private static Sale CreateValidSale()
    {
        return new Sale(
            "SALE-001",
            Guid.NewGuid(),
            "Daniel Customer",
            Guid.NewGuid(),
            "Main Branch");
    }

    [Fact]
    public void Should_Add_Item_And_Recalculate_Total()
    {
        var sale = CreateValidSale();

        sale.AddItem(Guid.NewGuid(), "Product A", 4, 100);

        Assert.Single(sale.Items);
        Assert.Equal(360m, sale.TotalAmount);
    }

    [Fact]
    public void Should_Update_Existing_Item_When_Same_Product_Is_Added()
    {
        var sale = CreateValidSale();
        var productId = Guid.NewGuid();

        sale.AddItem(productId, "Product A", 4, 100);
        sale.AddItem(productId, "Product A", 6, 100);

        Assert.Single(sale.Items);
        Assert.Equal(6, sale.Items.First().Quantity);
        Assert.Equal(0.10m, sale.Items.First().DiscountPercentage);
        Assert.Equal(540m, sale.TotalAmount);
    }

    [Fact]
    public void Should_Cancel_Sale()
    {
        var sale = CreateValidSale();
        sale.AddItem(Guid.NewGuid(), "Product A", 4, 100);

        sale.Cancel();

        Assert.True(sale.IsCancelled);
        Assert.NotNull(sale.CancelledAt);
    }

    [Fact]
    public void Should_Not_Allow_Adding_Item_To_Cancelled_Sale()
    {
        var sale = CreateValidSale();
        sale.Cancel();

        var exception = Assert.Throws<DomainException>(() =>
            sale.AddItem(Guid.NewGuid(), "Product A", 1, 100));

        Assert.Equal("Cannot add items to a cancelled sale", exception.Message);
    }

    [Fact]
    public void Should_Cancel_Item_And_Recalculate_Total()
    {
        var sale = CreateValidSale();

        sale.AddItem(Guid.NewGuid(), "Product A", 4, 100);
        var itemId = sale.Items.First().Id;

        sale.CancelItem(itemId);

        Assert.True(sale.Items.First().IsCancelled);
        Assert.Equal(0m, sale.TotalAmount);
    }

    [Fact]
    public void Should_Throw_DomainException_When_Cancelling_Sale_Twice()
    {
        var sale = CreateValidSale();

        sale.Cancel();

        var exception = Assert.Throws<DomainException>(() => sale.Cancel());

        Assert.Equal("Sale is already cancelled", exception.Message);
    }

    [Fact]
    public void Should_Update_Item_Quantity_And_Recalculate_Total()
    {
        var sale = CreateValidSale();

        sale.AddItem(Guid.NewGuid(), "Product A", 4, 100);
        var itemId = sale.Items.First().Id;

        sale.UpdateItem(itemId, 10, 100);

        var item = sale.Items.First();

        Assert.Equal(10, item.Quantity);
        Assert.Equal(0.20m, item.DiscountPercentage);
        Assert.Equal(200m, item.DiscountAmount);
        Assert.Equal(800m, item.TotalAmount);
        Assert.Equal(800m, sale.TotalAmount);
    }

    [Fact]
    public void Should_Not_Update_Item_When_Quantity_Is_Greater_Than_20()
    {
        var sale = CreateValidSale();

        sale.AddItem(Guid.NewGuid(), "Product A", 4, 100);
        var itemId = sale.Items.First().Id;

        var exception = Assert.Throws<DomainException>(() =>
            sale.UpdateItem(itemId, 21, 100));

        Assert.Equal("Item quantity cannot exceed 20 units", exception.Message);
    }
}