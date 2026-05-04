using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using System.Globalization;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;

/// <summary>
/// AutoMapper profile for ListSales operation (WebApi)
/// </summary>
public class ListSalesProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of ListSalesProfile
    /// </summary>
    public ListSalesProfile()
    {
        // Map from ListSalesRequest to ListSalesCommand with date parsing
        CreateMap<ListSalesRequest, ListSalesCommand>()
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => ParseDate(src.StartDate)))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => ParseDateEndOfDay(src.EndDate)));

        // Map from ListSalesItemResult to ListSalesItemResponse
        CreateMap<ListSalesItemResult, ListSalesItemResponse>();

        // Map from ListSalesResult to ListSalesResponse
        CreateMap<ListSalesResult, ListSalesResponse>();
    }

    /// <summary>
    /// Parses a date string in format YYYY-MM-DD to DateTime at 00:00:00 UTC
    /// </summary>
    /// <param name="dateString">The date string to parse</param>
    /// <returns>DateTime? or null if parsing fails or string is null/empty</returns>
    private static DateTime? ParseDate(string? dateString)
    {
        if (string.IsNullOrWhiteSpace(dateString))
            return null;

        try
        {
            // Parse without timezone assumptions - just get the date components
            if (DateTime.TryParseExact(dateString.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            {
                // Explicitly create UTC DateTime from parsed components
                var result = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0, DateTimeKind.Utc);
                System.Diagnostics.Debug.WriteLine($"ParseDate: '{dateString}' -> {result:O} (Kind={result.Kind})");
                return result;
            }

            System.Diagnostics.Debug.WriteLine($"ParseDate: '{dateString}' -> Failed to parse");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ParseDate: '{dateString}' -> Exception: {ex.Message}");
        }

        return null;
    }

    /// <summary>
    /// Parses a date string in format YYYY-MM-DD to DateTime at 23:59:59.999 UTC (end of day)
    /// </summary>
    /// <param name="dateString">The date string to parse</param>
    /// <returns>DateTime? or null if parsing fails or string is null/empty</returns>
    private static DateTime? ParseDateEndOfDay(string? dateString)
    {
        if (string.IsNullOrWhiteSpace(dateString))
            return null;

        try
        {
            // Parse without timezone assumptions - just get the date components
            if (DateTime.TryParseExact(dateString.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            {
                // Create end of day: 23:59:59.999 of the parsed date (UTC)
                var result = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999, DateTimeKind.Utc);
                System.Diagnostics.Debug.WriteLine($"ParseDateEndOfDay: '{dateString}' -> {result:O} (Kind={result.Kind})");
                return result;
            }

            System.Diagnostics.Debug.WriteLine($"ParseDateEndOfDay: '{dateString}' -> Failed to parse");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ParseDateEndOfDay: '{dateString}' -> Exception: {ex.Message}");
        }

        return null;
    }
}
