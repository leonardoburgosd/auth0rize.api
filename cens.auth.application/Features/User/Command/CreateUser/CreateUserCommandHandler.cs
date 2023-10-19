using cens.auth.application.Common;
using cens.auth.application.Common.Security;
using cens.auth.application.Wrappers;
using cens.auth.domain.Primitives;
using cens.auth.domain.User.Business;
using MediatR;

namespace cens.auth.application.Features.User.Command.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateUserCommandHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }

        public async Task<Response<bool>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            Response<bool> response = new Response<bool>();

            (byte[], byte[]) generate = Encrypt.generateHash(request.Password);
            int create = await _unitOfWork.User.create(new UserCreate()
            {
                Birthday = request.Birthday,
                Avatar = "https://usc1.contabostorage.com/8322137c72f34b6785394a9b2fc594dc:cens.telcogo.2022/Tools%2FPhoto%20Personal%2Fdefault.png",
                LastName = request.LastName,
                MotherLastName = request.MotherLastName,
                Name = request.Name,
                Password = generate.Item2,
                Salt = generate.Item1,
                UserName = request.UserName,
                RegistrationUser = request.SecurityTokenData.UserName
            });

            if (create == 0)
                throw new KeyNotFoundException("No se pudo registrar el usuario.");

            response.Message = Message.CREATE;
            response.Success = true;
            response.Data = true;

            return response;
        }
    }
}
