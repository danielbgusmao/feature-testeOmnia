using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// Profile for mapping UpdateSale feature requests to commands and results to responses
/// </summary>
public class UpdateSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for UpdateSale feature
    /// </summary>
    public UpdateSaleProfile()
    {
        CreateMap<UpdateSaleRequest, Application.Sales.UpdateSale.UpdateSaleCommand>();
        CreateMap<UpdateSaleItemRequest, Application.Sales.UpdateSale.UpdateSaleItemCommand>();
        
        CreateMap<Application.Sales.UpdateSale.UpdateSaleResult, UpdateSaleResponse>();
        CreateMap<Application.Sales.UpdateSale.UpdateSaleItemResult, UpdateSaleItemResponse>();
    }
}
