using FluentValidation.Results;

namespace auth0rize.auth.application.Extensions
{
    public class ValidationException : Exception
    {
        public List<string> Errors { get; set; }

        public ValidationException() : base("Se han producido uno o más errores de validación.") => Errors = new List<string>();

        public ValidationException(IEnumerable<ValidationFailure> failures) : this() => failures.ToList().ForEach(failure => Errors.Add(failure.ErrorMessage));
    }
}
