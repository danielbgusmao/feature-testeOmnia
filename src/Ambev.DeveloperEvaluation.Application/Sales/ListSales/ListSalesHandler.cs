using AutoMapper;
using Ambev.DeveloperEvaluation.ORM;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

/// <summary>
/// Handler for processing ListSalesCommand requests
/// </summary>
public class ListSalesHandler : IRequestHandler<ListSalesCommand, ListSalesResult>
{
    private readonly DefaultContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<ListSalesHandler> _logger;

    /// <summary>
    /// Initializes a new instance of ListSalesHandler
    /// </summary>
    /// <param name="context">The database context</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="logger">The logger instance</param>
    public ListSalesHandler(DefaultContext context, IMapper mapper, ILogger<ListSalesHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Handles the ListSalesCommand request
    /// </summary>
    /// <param name="request">The ListSales command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of sales matching the filters</returns>
    public async Task<ListSalesResult> Handle(ListSalesCommand request, CancellationToken cancellationToken)
    {
        // Validate and set default pagination values
        var page = request.Page < 1 ? 1 : request.Page;
        var pageSize = request.PageSize < 1 ? 10 : (request.PageSize > 100 ? 100 : request.PageSize);

        _logger.LogInformation(
            "Listing sales | Page: {Page} | PageSize: {PageSize} | SaleNumber: {SaleNumber} | CustomerId: {CustomerId} | StartDate: {StartDate} | EndDate: {EndDate} | IsCancelled: {IsCancelled}",
            page, pageSize, request.SaleNumber, request.CustomerId, request.StartDate, request.EndDate, request.IsCancelled);

        // Build the query with filters
        var query = _context.Sales.AsNoTracking();

        // Apply filters only if they are provided
        if (!string.IsNullOrWhiteSpace(request.SaleNumber))
        {
            _logger.LogDebug("Applying filter: SaleNumber contains {SaleNumber}", request.SaleNumber);
            query = query.Where(s => s.SaleNumber.Contains(request.SaleNumber));
        }

        if (request.CustomerId.HasValue)
        {
            _logger.LogDebug("Applying filter: CustomerId equals {CustomerId}", request.CustomerId);
            query = query.Where(s => s.CustomerId == request.CustomerId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.CustomerName))
        {
            _logger.LogDebug("Applying filter: CustomerName contains {CustomerName}", request.CustomerName);
            query = query.Where(s => s.CustomerName.Contains(request.CustomerName));
        }

        if (request.BranchId.HasValue)
        {
            _logger.LogDebug("Applying filter: BranchId equals {BranchId}", request.BranchId);
            query = query.Where(s => s.BranchId == request.BranchId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.BranchName))
        {
            _logger.LogDebug("Applying filter: BranchName contains {BranchName}", request.BranchName);
            query = query.Where(s => s.BranchName.Contains(request.BranchName));
        }

        if (request.IsCancelled.HasValue)
        {
            _logger.LogDebug("Applying filter: IsCancelled equals {IsCancelled}", request.IsCancelled);
            query = query.Where(s => s.IsCancelled == request.IsCancelled.Value);
        }

        if (request.StartDate.HasValue)
        {
            _logger.LogDebug("Applying filter: SaleDate >= {StartDate} (Kind={Kind})", request.StartDate, request.StartDate.Value.Kind);
            query = query.Where(s => s.SaleDate >= request.StartDate.Value);
        }

        if (request.EndDate.HasValue)
        {
            _logger.LogDebug("Applying filter: SaleDate <= {EndDate} (Kind={Kind})", request.EndDate, request.EndDate.Value.Kind);
            query = query.Where(s => s.SaleDate <= request.EndDate.Value);
        }

        // Get total items before pagination
        var totalItems = await query.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        // Apply ordering
        var orderBy = (request.OrderBy ?? "saleDate").ToLower();
        var orderDirection = (request.OrderDirection ?? "desc").ToLower();
        var isAscending = orderDirection == "asc";

        _logger.LogDebug("Applying ordering: OrderBy={OrderBy}, OrderDirection={OrderDirection}", orderBy, orderDirection);

        query = orderBy switch
        {
            "salenumber" => isAscending 
                ? query.OrderBy(s => s.SaleNumber)
                : query.OrderByDescending(s => s.SaleNumber),
            "totalamount" => isAscending
                ? query.OrderBy(s => s.TotalAmount)
                : query.OrderByDescending(s => s.TotalAmount),
            "customername" => isAscending
                ? query.OrderBy(s => s.CustomerName)
                : query.OrderByDescending(s => s.CustomerName),
            "branchname" => isAscending
                ? query.OrderBy(s => s.BranchName)
                : query.OrderByDescending(s => s.BranchName),
            _ => isAscending
                ? query.OrderBy(s => s.SaleDate)
                : query.OrderByDescending(s => s.SaleDate)
        };

        // Apply pagination
        var skip = (page - 1) * pageSize;
        var sales = await query
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        // Map to result
        var items = _mapper.Map<List<ListSalesItemResult>>(sales);

        _logger.LogInformation(
            "Listed {Count} sales | Page: {Page} of {TotalPages} | Total items: {TotalItems}",
            items.Count, page, totalPages, totalItems);

        return new ListSalesResult
        {
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = totalPages,
            Items = items
        };
    }
}
