using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cens.auth.application.Wrappers;
using MediatR;

namespace cens.auth.application.Features.User.Queries.GetAllById
{
    public record GetAllById(int userId): IRequest<Response<GetAllByIdResponse>>;
}