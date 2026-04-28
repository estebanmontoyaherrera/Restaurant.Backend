using Mapster;
using Ordering.Application.Interfaces.Services;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using SharedKernel.Constants;
using SharedKernel.Dtos.Commons;

namespace Ordering.Application.UseCases.Dishes.Queries.GetSelectQuery;

public class GetDishSelectHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetDishSelectQuery, IEnumerable<SelectResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<IEnumerable<SelectResponseDto>>> Handle(GetDishSelectQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<IEnumerable<SelectResponseDto>>();

        try
        {
            var data = await _unitOfWork.Dishes.GetAllAsync();

            if (data is null)
            {
                response.IsSuccess = false;
                response.Message = GlobalMessages.MESSAGE_QUERY_EMPTY;
                return response;
            }

            response.IsSuccess = true;
            response.Data = data.Adapt<IEnumerable<SelectResponseDto>>();
            response.Message = GlobalMessages.MESSAGE_QUERY;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }

        return response;
    }
}
