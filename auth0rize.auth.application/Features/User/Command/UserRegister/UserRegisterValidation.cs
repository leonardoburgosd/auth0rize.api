using FluentValidation;

namespace auth0rize.auth.application.Features.User.Command.UserRegister
{
    public class UserRegisterValidation : AbstractValidator<UserRegister>
    {
        public UserRegisterValidation()
        {
            RuleFor(r => r.email).NotEmpty().WithMessage("Debe ingresar su correo.");
            RuleFor(r => r.email).EmailAddress().WithMessage("Debe ingresar un correo correctamente.");
            RuleFor(r => r.lastName).NotEmpty().WithMessage("Debe ingresar su apellido paterno.");
            RuleFor(r => r.name).NotEmpty().WithMessage("Debe ingresar su nombre.");
            RuleFor(r => r.motherLastName).NotEmpty().WithMessage("Debe ingresar su apellido materno.");
            RuleFor(r => r.password).NotEmpty().WithMessage("Debe ingresar su contraseña");
            RuleFor(r => r.userName).NotEmpty().WithMessage("Debe ingresar su nombre de usuario");
            //RuleFor(r => r.Campo).Must(ValidacionEjemplo());
        }
        //private bool ValidacionEjemplo()
        //{
        //    return true;
        //}
    }
}
