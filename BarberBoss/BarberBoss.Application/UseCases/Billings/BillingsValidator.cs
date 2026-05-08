using BarberBoss.Communication.Enums;
using BarberBoss.Communication.Requests;
using BarberBoss.Exception;
using FluentValidation;

namespace BarberBoss.Application.UseCases.Billings;
public class BillingsValidator : AbstractValidator<RequestBillingsJson>
{
    public BillingsValidator()
    {
        // usando fluentValidation para validar os campos do RequestBillingsJson, garantindo que os dados estejam corretos antes de serem processados
        RuleFor(billings => billings.Date)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.DATE_REQUIRED);

        RuleFor(billings => billings.BarberName)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.BARBER_NAME_REQUIRED)
            .Length(2, 80)
            .WithMessage(ResourceErrorMessages.BARBER_NAME_LENGTH);
                
        RuleFor(billings => billings.ClientName)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.CLIENT_NAME_REQUIRED)
            .Length(2, 120)
            .WithMessage(ResourceErrorMessages.CLIENT_NAME_LENGTH);
                
        RuleFor(billings => billings.ServiceName)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.SERVICE_NAME_REQUIRED)
            .Length(2, 120)
            .WithMessage(ResourceErrorMessages.SERVICE_NAME_LENGTH);
                
        RuleFor(billings => billings.Amount)
            .GreaterThanOrEqualTo(0)
            .WithMessage(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_OR_EQUAL_ZERO);

        RuleFor(billings => billings.Amount)
            .Equal(0)
            .When(billings => billings.Status == Status.Cancelado)
            .WithMessage(ResourceErrorMessages.AMOUNT_MUST_BE_ZERO_WHEN_CANCELLED);
                
        RuleFor(billings => billings.PaymentMethod)
            .IsInEnum()
            .WithMessage(ResourceErrorMessages.PAYMENT_METHOD_INVALID);
                
        RuleFor(billings => billings.Status)
            .IsInEnum()
            .WithMessage(ResourceErrorMessages.STATUS_INVALID);
                
        RuleFor(billings => billings.Notes)
            .MaximumLength(500)
            .WithMessage(ResourceErrorMessages.NOTES_MAX_LENGTH)
            .When(billings => billings.Notes is not null);
    }
}
