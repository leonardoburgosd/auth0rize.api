using auth0rize.auth.application.Extensions;
using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Application.Business;
using auth0rize.auth.domain.Primitives;
using MediatR;

namespace auth0rize.auth.application.Features.Domain.Queries.GetAllByIdQuery
{
    internal sealed class GetAllByIdQueryHandler : IRequestHandler<GetAllByIdQuery, Response<GetAllByIdQueryResponse>>
    {
        #region Inyeccion
        private readonly IUnitOfWork _unitOfWork;

        public GetAllByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        public async Task<Response<GetAllByIdQueryResponse>> Handle(GetAllByIdQuery request, CancellationToken cancellationToken)
        {
            Response<GetAllByIdQueryResponse> response = new Response<GetAllByIdQueryResponse>();

            ApplicationGet? application = await _unitOfWork.Application.getById(request.id);

            if (application is null) throw new ApiException("No se encontraron datos de esta aplicación.");

            response.Success = true;
            response.Message = "Detalle de dominio encontrado.";
            response.Data = new GetAllByIdQueryResponse()
            {
                Code = application.Code,
                Description = application.Description,
                Id = application.Id,
                Name = application.Name,
            };

            return response;
        }
    }
}
