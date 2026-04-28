using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Dtos.Dishes;
using Ordering.Application.Interfaces.Services;
using Ordering.Application.UseCases.Dishes.Commands.CreateCommand;
using Ordering.Application.UseCases.Dishes.Commands.DeleteCommand;
using Ordering.Application.UseCases.Dishes.Commands.ToggleAvailabilityCommand;
using Ordering.Application.UseCases.Dishes.Commands.UpdateCommand;
using Ordering.Application.UseCases.Dishes.Queries.GetAllQuery;
using Ordering.Application.UseCases.Dishes.Queries.GetByIdQuery;
using Ordering.Application.UseCases.Dishes.Queries.GetSelectQuery;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Dtos.Commons;
using SharedKernel.Helpers;

namespace Restaurant.Api.Controllers.Ordering;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class DishController(IDispatcher dispatcher, IExcelService excelService, IPdfService pdfService) : ControllerBase
{
    private readonly IDispatcher _dispatcher = dispatcher;
    private readonly IExcelService _excelService = excelService;
    private readonly IPdfService _pdfService = pdfService;

    [HttpGet("list")]
    public async Task<IActionResult> DishList([FromQuery] GetAllDishQuery query)
    {
        var response = await _dispatcher.Dispatch<GetAllDishQuery, IEnumerable<DishResponseDto>>(query, CancellationToken.None);
        return Ok(response);
    }

    [HttpGet("excel")]
    public async Task<IActionResult> DishReportExcel([FromQuery] GetAllDishQuery query)
    {
        var response = await _dispatcher.Dispatch<GetAllDishQuery, IEnumerable<DishResponseDto>>(query, CancellationToken.None);
        var fileBytes = _excelService.GenerateToExcel(response.Data!, ReportColumns.GetColumnsDishes());
        return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    [HttpGet("pdf")]
    public async Task<IActionResult> DishReportPdf([FromQuery] GetAllDishQuery query)
    {
        var response = await _dispatcher.Dispatch<GetAllDishQuery, IEnumerable<DishResponseDto>>(query, CancellationToken.None);
        var fileBytes = _pdfService.GenerateToPdf(response.Data!, ReportColumns.GetColumnsDishes(), "Dishes");
        return File(fileBytes, "application/pdf");
    }

    [HttpGet("select")]
    public async Task<IActionResult> DishSelect()
    {
        var response = await _dispatcher.Dispatch<GetDishSelectQuery, IEnumerable<SelectResponseDto>>(new GetDishSelectQuery(), CancellationToken.None);
        return Ok(response);
    }

    [HttpGet("{dishId:int}")]
    public async Task<IActionResult> DishById(int dishId)
    {
        var response = await _dispatcher.Dispatch<GetDishByIdQuery, DishByIdResponseDto>(new GetDishByIdQuery { DishId = dishId }, CancellationToken.None);
        return Ok(response);
    }

    [HttpPost("create")]
    public async Task<IActionResult> DishCreate([FromBody] CreateDishCommand command)
    {
        var response = await _dispatcher.Dispatch<CreateDishCommand, bool>(command, CancellationToken.None);
        return Ok(response);
    }

    [HttpPut("update")]
    public async Task<IActionResult> DishUpdate([FromBody] UpdateDishCommand command)
    {
        var response = await _dispatcher.Dispatch<UpdateDishCommand, bool>(command, CancellationToken.None);
        return Ok(response);
    }

    [HttpPut("toggle/{dishId:int}")]
    public async Task<IActionResult> DishToggle(int dishId)
    {
        var response = await _dispatcher.Dispatch<ToggleDishAvailabilityCommand, bool>(new ToggleDishAvailabilityCommand { DishId = dishId }, CancellationToken.None);
        return Ok(response);
    }

    [HttpPut("delete/{dishId:int}")]
    public async Task<IActionResult> DishDelete(int dishId)
    {
        var response = await _dispatcher.Dispatch<DeleteDishCommand, bool>(new DeleteDishCommand { DishId = dishId }, CancellationToken.None);
        return Ok(response);
    }
}
