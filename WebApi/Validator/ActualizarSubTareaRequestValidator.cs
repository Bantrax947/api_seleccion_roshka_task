using Core.Contracts.Request;
using FluentValidation;

namespace WebApi.Validator
{
    public class ActualizarSubTareaRequestValidator : AbstractValidator<ActualizarSubTareaRequest>
    {
        public ActualizarSubTareaRequestValidator()
        {
            RuleFor(x => x.Titulo)
                .NotEmpty().WithMessage("El título es obligatorio.")
                .MaximumLength(255).WithMessage("El título no puede exceder los 255 caracteres.");

            RuleFor(x => x.Estado)
                .NotNull().WithMessage("El estado es obligatorio.");
        }
    }
}