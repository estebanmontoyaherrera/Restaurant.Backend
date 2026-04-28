using Mapster;
using Ordering.Application.Dtos.Dishes;
using Ordering.Application.Interfaces.Services;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Ordering.Application.UseCases.Dishes.Queries.GetByIdQuery;

public class GetDishByIdHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetDishByIdQuery, DishByIdResponseDto>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<DishByIdResponseDto>> Handle(GetDishByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<DishByIdResponseDto>();

        try
        {
            var data = await _unitOfWork.Dishes.GetByIdAsync(request.DishId);

            if (data is null)
            {
                response.IsSuccess = false;
                response.Message = "No records found.";
                return response;
            }

            response.IsSuccess = true;
            response.Data = data.Adapt<DishByIdResponseDto>();
            response.Message = "Query successful.";
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}
