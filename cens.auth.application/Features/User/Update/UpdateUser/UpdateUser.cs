using System;
using cens.auth.application.Common.Entities;
using cens.auth.application.Wrappers;
using MediatR;

namespace cens.auth.application.Features.User.Update.UpdateUser
{
    public record UpdateUser(int id, string name, string lastName, string motherLastName, DateTime Birthday, string userName, string password, string email,
                                    SecurityTokenData SecurityTokenData): IRequest<Response<bool>>;

}