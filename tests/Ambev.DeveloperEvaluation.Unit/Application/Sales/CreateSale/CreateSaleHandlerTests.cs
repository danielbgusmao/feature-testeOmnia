using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.ORM;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.CreateSale;

public class CreateSaleHandlerTests
{
    private static DefaultContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<DefaultContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new DefaultContext(options);
    }

    private static IMapper CreateMapper()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CreateSaleProfile>();
        });

        return configuration.CreateMapper();
    }

    [Fact]
    public async Task Should_Create_Sale_With_Discount_Applied()
    {
        await using var context = CreateContext();

        var mapper = CreateMapper();
        var logger = Substitute.For<ILogger<CreateSaleHandler>>();

        var handler = new CreateSaleHandler(context, logger, mapper);

        var command = new CreateSaleCommand
        {
            SaleNumber = "SALE-TEST-001",
            CustomerId = Guid.NewGuid(),
            CustomerName = "Daniel Customer",
            BranchId = Guid.NewGuid(),
            BranchName = "Main Branch",
            Items =
            [
                new CreateSaleItemCommand
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = "Product A",
                    Quantity = 10,
                    UnitPrice = 100
                }
            ]
        };

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("SALE-TEST-001", result.SaleNumber);
        Assert.Equal(800m, result.TotalAmount);
        Assert.False(result.IsCancelled);
        Assert.Single(result.Items);

        var item = result.Items.First();

        Assert.Equal(10, item.Quantity);
        Assert.Equal(0.20m, item.DiscountPercentage);
        Assert.Equal(200m, item.DiscountAmount);
        Assert.Equal(800m, item.TotalAmount);

        var saleInDatabase = await context.Sales
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == result.Id);

        Assert.NotNull(saleInDatabase);
        Assert.Equal(800m, saleInDatabase.TotalAmount);
        Assert.Single(saleInDatabase.Items);
    }
}