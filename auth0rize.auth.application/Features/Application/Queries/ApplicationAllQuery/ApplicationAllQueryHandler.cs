using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Application.Business;
using auth0rize.auth.domain.Primitives;
using MediatR;

namespace auth0rize.auth.application.Features.Application.Queries.ApplicationAllQuery
{
    internal sealed class ApplicationAllQueryHandler : IRequestHandler<ApplicationAllQuery, Response<List<ApplicationAllQueryResponse>>>
    {
        #region Inyeccion
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationAllQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        public async Task<Response<List<ApplicationAllQueryResponse>>> Handle(ApplicationAllQuery request, CancellationToken cancellationToken)
        {
            Response<List<ApplicationAllQueryResponse>> response = new Response<List<ApplicationAllQueryResponse>>();

            List<ApplicationGet> application = await _unitOfWork.Application.get(request.userId);

            if (application is null) throw new KeyNotFoundException("No se encontraron aplicativos relacionados.");

            List<ApplicationAllQueryResponse> applicationQuery = new List<ApplicationAllQueryResponse>();

            application.ForEach(a =>
            {
                applicationQuery.Add(new ApplicationAllQueryResponse()
                {
                    Code = a.Code,
                    Description = a.Description,
                    Name = a.Name
                });
            });

            response.Success = true;
            response.Message = "Se encontraron datos.";
            response.Data = applicationQuery;

            return response;
        }

    }
}
