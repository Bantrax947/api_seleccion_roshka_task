using Core.Contracts.Request;
using FluentValidation;

namespace WebApi.Validator
{
    public class PagedRequestValidator : AbstractValidator<PagedRequest>
    {
        public PagedRequestValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1).WithMessage("El número de página debe ser 1 o superior.");
            RuleFor(x => x.Limit)
                .GreaterThanOrEqualTo(1).WithMessage("El límite de resultados debe ser 1 o superior.");
            RuleFor(x => x.Order)
                .Must(order => order?.ToLower() == "asc" || order?.ToLower() == "desc")
                .WithMessage("El orden solo puede ser 'asc' o 'desc'.");
        }
    }
}