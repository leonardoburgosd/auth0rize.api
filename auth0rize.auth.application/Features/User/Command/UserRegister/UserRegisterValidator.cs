using FluentValidation;

namespace auth0rize.auth.application.Features.User.Command.UserRegister
{
    public class SuperUserRegisterValidator : AbstractValidator<SuperUserRegisterRequest>
    {
        public SuperUserRegisterValidator()
        {
            RuleFor(u => u.Name).NotEmpty().NotNull().WithMessage("Debe ingresar su nombre.");
            RuleFor(u => u.LastName).NotEmpty().NotNull().WithMessage("Debe ingresar su primer apellido.");
            RuleFor(u => u.MotherLastName).NotEmpty().NotNull().WithMessage("Debe ingresar su segundo apellido.");
            RuleFor(u => u.UserName).NotEmpty().NotNull().WithMessage("Debe ingresar su nombre de usuario.");
            RuleFor(u => u.Email).NotEmpty().NotNull().WithMessage("Debe ingresar su correo electrónico.");
            RuleFor(u => u.Email).EmailAddress().WithMessage("Formato de correo electrónico incorrecto.");
            RuleFor(u => u.Password).NotEmpty().NotNull().WithMessage("Debe ingresar su contraseña.");
        }
    }

    public class UserRegisterValidator : AbstractValidator<UserRegisterRequest>
    {
        public UserRegisterValidator()
        {
            RuleFor(u => u.Name).NotEmpty().NotNull().WithMessage("Debe ingresar su nombre.");
            RuleFor(u => u.LastName).NotEmpty().NotNull().WithMessage("Debe ingresar su primer apellido.");
            RuleFor(u => u.MotherLastName).NotEmpty().NotNull().WithMessage("Debe ingresar su segundo apellido.");
            RuleFor(u => u.UserName).NotEmpty().NotNull().WithMessage("Debe ingresar su nombre de usuario.");
            RuleFor(u => u.Email).NotEmpty().NotNull().WithMessage("Debe ingresar su correo electrónico.");
            RuleFor(u => u.Email).EmailAddress().WithMessage("Formato de correo electrónico incorrecto.");
            RuleFor(u => u.Password).NotEmpty().NotNull().WithMessage("Debe ingresar su contraseña.");
            RuleFor(u => u.Type).NotEmpty().NotNull().WithMessage("Debe indicar su tipo de usuario.");
        }
    }
}
