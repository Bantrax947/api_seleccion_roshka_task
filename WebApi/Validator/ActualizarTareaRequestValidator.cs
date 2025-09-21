using Core.Contracts.Request;
using FluentValidation;

namespace WebApi.Validator
{
    public class ActualizarTareaRequestValidator : AbstractValidator<ActualizarTareaRequest>
    {
        public ActualizarTareaRequestValidator()
        {
            RuleFor(x => x.Titulo)
                .NotEmpty()
                .WithMessage("El título es obligatorio.");

            RuleFor(x => x.Prioridad)
                .InclusiveBetween(1, 5)
                .WithMessage("La prioridad debe estar entre 1 y 5.");

            RuleFor(x => x.Estado)
            .Must(x => new[] { "Pendiente", "EnProgreso", "Completada", "Cancelada" }.Contains(x))
            .When(x => !string.IsNullOrWhiteSpace(x.Estado))
            .WithMessage("El estado de la tarea debe ser 'Pendiente', 'EnProgreso', 'Completada' o 'Cancelada'.");
        }
    }
}