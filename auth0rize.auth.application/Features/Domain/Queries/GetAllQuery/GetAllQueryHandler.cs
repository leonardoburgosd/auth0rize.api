using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Domain.Business;
using auth0rize.auth.domain.Primitives;
using MediatR;

namespace auth0rize.auth.application.Features.Domain.Queries.GetAllQuery
{
    internal sealed class GetAllQueryHandler : IRequestHandler<GetAllQuery, Response<List<GetAllQueryResponse>>>
    {
        #region Inyeccion
        private readonly IUnitOfWork _unitOfWork;

        public GetAllQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        public async Task<Response<List<GetAllQueryResponse>>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            Response<List<GetAllQueryResponse>> response = new Response<List<GetAllQueryResponse>>();

            List<DomainGetById>? domainDefault = await _unitOfWork.Domain.get(request.session.DomainName);
            List<GetAllQueryResponse> data = new List<GetAllQueryResponse>();
            domainDefault.ForEach(d =>
            {
                data.Add(new GetAllQueryResponse()
                {
                    Id = d.Id,
                    Name = d.Name,
                    Default = d.Default,
                    Code = d.Code
                });
            });

            response.Success = true;
            response.Message = "";
            response.Data = data;

            return response;
        }
    }
}
