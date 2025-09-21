using Core.Contracts.Request;
using FluentValidation;

namespace WebApi.Validator
{
    public class SubTareaRequestValidator : AbstractValidator<SubTareaRequest>
    {
        public SubTareaRequestValidator()
        {
            RuleFor(x => x.Titulo)
                .NotEmpty()
                .WithMessage("El titulo de la subtarea es obligatorio");

            RuleFor(x => x.Estado)
                .Must(e => e == true || e == false)
                .WithMessage("El estado debe ser 0 (Pendiente) o 1 (Completada).");

        }
    }
}