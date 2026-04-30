using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Dtos.OrderDetails;
using Ordering.Application.Dtos.Orders;
using Ordering.Application.Dtos.Reports;
using Ordering.Application.Interfaces.Services;
using Ordering.Application.UseCases.OrderDetails.Commands.CreateCommand;
using Ordering.Application.UseCases.OrderDetails.Commands.DeleteCommand;
using Ordering.Application.UseCases.OrderDetails.Commands.UpdateCommand;
using Ordering.Application.UseCases.OrderDetails.Queries.GetByIdQuery;
using Ordering.Application.UseCases.Orders.Commands.AdvanceStatusCommand;
using Ordering.Application.UseCases.Orders.Commands.CreateCommand;
using Ordering.Application.UseCases.Orders.Commands.DeleteCommand;
using Ordering.Application.UseCases.Orders.Commands.UpdateCommand;
using Ordering.Application.UseCases.Orders.Queries.GetAllQuery;
using Ordering.Application.UseCases.Orders.Queries.GetByIdQuery;
using Ordering.Application.UseCases.Orders.Queries.GetSelectQuery;
using Ordering.Application.UseCases.Reports.Queries.GetSalesReportQuery;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Dtos.Commons;
using SharedKernel.Helpers;

namespace Restaurant.Api.Controllers.Ordering;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class OrderController(IDispatcher dispatcher, IExcelService excelService, IPdfService pdfService) : ControllerBase
{
    private readonly IDispatcher _dispatcher = dispatcher;
    private readonly IExcelService _excelService = excelService;
    private readonly IPdfService _pdfService = pdfService;

    [HttpGet("list")]
    public async Task<IActionResult> OrderList([FromQuery] GetAllOrderQuery query)
    {
        var response = await _dispatcher.Dispatch<GetAllOrderQuery, IEnumerable<OrderResponseDto>>(query, CancellationToken.None);
        return Ok(response);
    }

    [HttpGet("excel")]
    public async Task<IActionResult> OrderReportExcel([FromQuery] GetAllOrderQuery query)
    {
        var response = await _dispatcher.Dispatch<GetAllOrderQuery, IEnumerable<OrderResponseDto>>(query, CancellationToken.None);
        var fileBytes = _excelService.GenerateToExcel(response.Data!, ReportColumns.GetColumnsOrders());
        return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    [HttpGet("pdf")]
    public async Task<IActionResult> OrderReportPdf([FromQuery] GetAllOrderQuery query)
    {
        var response = await _dispatcher.Dispatch<GetAllOrderQuery, IEnumerable<OrderResponseDto>>(query, CancellationToken.None);
        var fileBytes = _pdfService.GenerateToPdf(response.Data!, ReportColumns.GetColumnsOrders(), "Orders");
        return File(fileBytes, "application/pdf");
    }

    [HttpGet("select")]
    public async Task<IActionResult> OrderSelect()
    {
        var response = await _dispatcher.Dispatch<GetOrderSelectQuery, IEnumerable<SelectResponseDto>>(new GetOrderSelectQuery(), CancellationToken.None);
        return Ok(response);
    }

    [HttpGet("{orderId:int}")]
    public async Task<IActionResult> OrderById(int orderId)
    {
        var response = await _dispatcher.Dispatch<GetOrderByIdQuery, OrderByIdResponseDto>(new GetOrderByIdQuery { OrderId = orderId }, CancellationToken.None);
        return Ok(response);
    }

    [HttpPost("create")]
    public async Task<IActionResult> OrderCreate([FromBody] CreateOrderCommand command)
    {
        var response = await _dispatcher.Dispatch<CreateOrderCommand, bool>(command, CancellationToken.None);
        return Ok(response);
    }

    [HttpPut("update")]
    public async Task<IActionResult> OrderUpdate([FromBody] UpdateOrderCommand command)
    {
        var response = await _dispatcher.Dispatch<UpdateOrderCommand, bool>(command, CancellationToken.None);
        return Ok(response);
    }


    [HttpPost("/orders/{orderId:int}/items")]
    public async Task<IActionResult> AddOrderItem(int orderId, [FromBody] CreateOrderDetailCommand command)
    {
        command.OrderId = orderId;
        var response = await _dispatcher.Dispatch<CreateOrderDetailCommand, bool>(command, CancellationToken.None);
        return Ok(response);
    }

    [HttpPut("/orders/{orderId:int}/items/{detailId:int}")]
    public async Task<IActionResult> UpdateOrderItem(int orderId, int detailId, [FromBody] UpdateOrderDetailCommand command)
    {
        command.OrderId = orderId;
        command.OrderDetailId = detailId;
        var response = await _dispatcher.Dispatch<UpdateOrderDetailCommand, bool>(command, CancellationToken.None);
        return Ok(response);
    }

    [HttpDelete("/orders/{orderId:int}/items/{detailId:int}")]
    public async Task<IActionResult> DeleteOrderItem(int orderId, int detailId)
    {
        var response = await _dispatcher.Dispatch<DeleteOrderDetailCommand, bool>(new DeleteOrderDetailCommand
        {
            OrderId = orderId,
            OrderDetailId = detailId
        }, CancellationToken.None);

        if (response.IsSuccess == true)
            return NoContent();

        return Ok(response);
    }

    [HttpPut("advance-status/{orderId:int}")]
    public async Task<IActionResult> OrderAdvanceStatus(int orderId)
    {
        var response = await _dispatcher.Dispatch<AdvanceOrderStatusCommand, bool>(new AdvanceOrderStatusCommand { OrderId = orderId }, CancellationToken.None);
        return Ok(response);
    }


    [HttpGet("sales-report")]
    public async Task<IActionResult> SalesReport([FromQuery] GetSalesReportQuery query)
    {
        var response = await _dispatcher.Dispatch<GetSalesReportQuery, SalesReportResponseDto>(query, CancellationToken.None);
        return Ok(response);
    }

    [HttpPut("delete/{orderId:int}")]
    public async Task<IActionResult> OrderDelete(int orderId)
    {
        var response = await _dispatcher.Dispatch<DeleteOrderCommand, bool>(new DeleteOrderCommand { OrderId = orderId }, CancellationToken.None);
        return Ok(response);
    }

    [HttpGet("/orders/{orderId:int}/items")]
    public async Task<IActionResult> GetOrderItems(int orderId)
    {
        var response = await _dispatcher.Dispatch<
            GetOrderDetailsByOrderIdQuery,
            IEnumerable<OrderDetailResponseDto>
        >(
            new GetOrderDetailsByOrderIdQuery { OrderId = orderId },
            CancellationToken.None
        );

        return Ok(response);
    }
}
