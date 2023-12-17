
using cens.auth.application.Wrappers;
using cens.auth.domain.Primitives;
using MediatR;

namespace cens.auth.application.Features.Application.Queries.GetApplication
{
    public class GetApplicationQueryHandler : IRequestHandler<GetApplicationQuery, Response<List<GetApplicationQueryResponse>>>
    {
        #region Inyeccion
        private readonly IUnitOfWork _unitOfWork;
        public GetApplicationQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        public async Task<Response<List<GetApplicationQueryResponse>>> Handle(GetApplicationQuery request, CancellationToken cancellationToken)
        {
            Response<List<GetApplicationQueryResponse>> response = new Response<List<GetApplicationQueryResponse>>();

            IEnumerable<domain.Application.Application> applications = await _unitOfWork.Application.get();
            if (applications is null)
            {
                response.Success = false;
                response.Message = "No se encontraron datos.";
            }

            List<GetApplicationQueryResponse> applicationsResponse = new List<GetApplicationQueryResponse>();

            applications!.ToList().ForEach(a =>
            {
                applicationsResponse.Add(new GetApplicationQueryResponse()
                {
                    ApplicationId = a.Id,
                    Key = a.Key,
                    Icon = a.Icon,
                    Name = a.Name,
                    Description = a.Description
                });
            });

            response.Success = true;
            response.Message = "Lista de datos encontrados.";
            response.Data = applicationsResponse;

            return response;
        }
    }
}