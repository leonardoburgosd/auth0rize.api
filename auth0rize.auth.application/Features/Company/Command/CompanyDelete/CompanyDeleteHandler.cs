using auth0rize.auth.application.Extensions;
using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using MediatR;

namespace auth0rize.auth.application.Features.Company.Command.CompanyDelete
{
    internal class CompanyDeleteHandler : IRequestHandler<CompanyDelete, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyDeleteHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Handle(CompanyDelete request, CancellationToken cancellationToken)
        {
            // Buscar la compañía
            var companyList = await _unitOfWork.Repository<domain.Company.Company>().QueryAsync<domain.Company.Company>(
                new Dictionary<string, object> { { "Id", request.Id } },
                Schemas.Organization
            );

            var company = companyList.FirstOrDefault();

            if (company == null)
            {
                throw new ApiException("Compañía no encontrada.");
            }

            // Soft delete
            company.IsDeleted = true;
            company.DateDeleted = DateTime.Now;
            company.UserDeleted = request.UserDeleted;

            await _unitOfWork.Repository<domain.Company.Company>().UpdateAsync(company, Schemas.Organization);

            return new Response<bool>(true, "Compañía eliminada exitosamente.");
        }
    }
}
