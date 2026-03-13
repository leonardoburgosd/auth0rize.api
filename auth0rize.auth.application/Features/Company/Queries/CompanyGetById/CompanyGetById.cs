using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.Company.Queries.CompanyGetById
{
    public record CompanyGetById(int Id) : IRequest<Response<CompanyGetByIdResponse>>;
}
