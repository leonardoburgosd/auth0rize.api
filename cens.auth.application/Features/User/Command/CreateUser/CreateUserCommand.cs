using cens.auth.application.Common;
using cens.auth.application.Common.Entities;
using cens.auth.application.Common.Security;
using cens.auth.application.Wrappers;
using cens.auth.infraestructure.Persistence.Interfaces;
using MediatR;

namespace cens.auth.application.Features.User.Command.CreateUser
{
    public class CreateUserCommand : IRequest<Response<bool>>
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string MotherLastName { get; set; }
        public DateTime Birthday { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public SecurityTokenData SecurityTokenData { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateUserCommandHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }

        public async Task<Response<bool>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            Response<bool> response = new Response<bool>();

            (byte[], byte[]) generate = Encrypt.generateHash(request.Password);
            int create = await _unitOfWork.User.create(new domain.Bussines.UserCreate()
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
                throw new KeyNotFoundException("Error al crear un usuario.");

            response.Message = Message.CREATE;
            response.Success = true;
            response.Data = true;

            return response;
        }
    }
}
