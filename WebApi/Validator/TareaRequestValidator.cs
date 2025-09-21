using Core.Contracts.Request;
using FluentValidation;
namespace WebApi.Validator
{
    public class TareaRequestValidator : AbstractValidator<TareaRequest>
    {
        public TareaRequestValidator()
        {
            RuleFor(x => x.Titulo)
                .NotEmpty()
                .WithMessage("El título de la tarea es obligatorio.")
                .MaximumLength(255)
                .WithMessage("El título no puede exceder los 255 caracteres.");

            RuleFor(x => x.Descripcion)
                .MaximumLength(255)
                .WithMessage("La descripción no puede exceder los 255 caracteres.");

            RuleFor(x => x.Estado)
                .NotEmpty()
                .WithMessage("El estado de la tarea es obligatorio.")
                .Must(x => new[] { "Pendiente", "EnProgreso", "Completada", "Cancelada" }.Contains(x))
                .WithMessage("El estado de la tarea debe ser 'Pendiente', 'EnProgreso', 'Completada' o 'Cancelada'.");

            RuleFor(x => x.Prioridad)
                .NotEmpty()
                .WithMessage("La prioridad de la tarea es obligatoria.")
                .InclusiveBetween(1, 5)
                .WithMessage("La prioridad de la tarea debe ser un número entre 1 y 5.");
        }
    }
}