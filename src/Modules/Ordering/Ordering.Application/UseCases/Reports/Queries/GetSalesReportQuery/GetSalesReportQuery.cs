using Ordering.Application.Dtos.Reports;
using SharedKernel.Abstractions.Messaging;

namespace Ordering.Application.UseCases.Reports.Queries.GetSalesReportQuery;

public class GetSalesReportQuery : IQuery<SalesReportResponseDto>
{
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
}
