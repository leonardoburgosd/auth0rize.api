using FluentValidation.Results;

namespace cens.auth.application.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException() : base("Se han producido uno o mas errores de validación.")
        {
            Errors = new List<string>();
        }

        public List<string> Errors { get; set; }
        public ValidationException(IEnumerable<ValidationFailure> failures) : this()
        {
            foreach (ValidationFailure failure in failures)
            {
                Errors.Add(failure.ErrorMessage);
            }
        }
    }
}
