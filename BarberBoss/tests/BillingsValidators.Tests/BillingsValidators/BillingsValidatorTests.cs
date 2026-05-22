using BarberBoss.Application.UseCases.Billings;
using BarberBoss.Communication.Enums;
using BarberBoss.Communication.Requests;
using BarberBoss.Exception;
using FluentValidation.TestHelper;


namespace BillingsValidators.Tests.BillingsValidators;
public class BillingsValidatorTests
{
    private readonly BillingsValidator _validator;

    public BillingsValidatorTests()
    {
        _validator = new BillingsValidator();
    }

    // testando a data

    [Fact]
    public void Date_WhenEmpty_ShouldHaveError()
    {
        var request = new RequestBillingsJson { Date = default };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Date)
              .WithErrorMessage(ResourceErrorMessages.DATE_REQUIRED);
    }

    [Fact]
    public void Date_WhenFilled_ShouldNotHaveError()
    {
        var request = BuildValidRequest();

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(x => x.Date);
    }

    // testando o BarberName 

    [Fact]
    public void BarberName_WhenEmpty_ShouldHaveRequiredError()
    {
        var request = BuildValidRequest();
        request.BarberName = string.Empty;

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.BarberName)
              .WithErrorMessage(ResourceErrorMessages.BARBER_NAME_REQUIRED);
    }

    [Theory]
    [InlineData("A")]           
    [InlineData("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA")] 
    public void BarberName_WhenOutsideLengthBounds_ShouldHaveLengthError(string name)
    {
        var request = BuildValidRequest();
        request.BarberName = name;

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.BarberName)
              .WithErrorMessage(ResourceErrorMessages.BARBER_NAME_LENGTH);
    }

    [Theory]
    [InlineData("Jo")]          
    [InlineData("ValidName")]   
    public void BarberName_WhenValid_ShouldNotHaveError(string name)
    {
        var request = BuildValidRequest();
        request.BarberName = name;

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(x => x.BarberName);
    }

    // testando o ClientName 

    [Fact]
    public void ClientName_WhenEmpty_ShouldHaveRequiredError()
    {
        var request = BuildValidRequest();
        request.ClientName = string.Empty;

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.ClientName)
              .WithErrorMessage(ResourceErrorMessages.CLIENT_NAME_REQUIRED);
    }

    [Theory]
    [InlineData("A")]           
    [InlineData("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA")] 
    public void ClientName_WhenOutsideLengthBounds_ShouldHaveLengthError(string name)
    {
        var request = BuildValidRequest();
        request.ClientName = name;

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.ClientName)
              .WithErrorMessage(ResourceErrorMessages.CLIENT_NAME_LENGTH);
    }

    [Theory]
    [InlineData("Jo")]          // exactly 2 chars – minimum boundary
    [InlineData("Valid Client Name")]
    public void ClientName_WhenValid_ShouldNotHaveError(string name)
    {
        var request = BuildValidRequest();
        request.ClientName = name;

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(x => x.ClientName);
    }

    // testando o ServiceName 

    [Fact]
    public void ServiceName_WhenEmpty_ShouldHaveRequiredError()
    {
        var request = BuildValidRequest();
        request.ServiceName = string.Empty;

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.ServiceName)
              .WithErrorMessage(ResourceErrorMessages.SERVICE_NAME_REQUIRED);
    }

    [Theory]
    [InlineData("A")]           
    [InlineData("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA")] 
    public void ServiceName_WhenOutsideLengthBounds_ShouldHaveLengthError(string name)
    {
        var request = BuildValidRequest();
        request.ServiceName = name;

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.ServiceName)
              .WithErrorMessage(ResourceErrorMessages.SERVICE_NAME_LENGTH);
    }

    [Theory]
    [InlineData("Haircut")]
    [InlineData("Beard Trim")]
    public void ServiceName_WhenValid_ShouldNotHaveError(string name)
    {
        var request = BuildValidRequest();
        request.ServiceName = name;

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(x => x.ServiceName);
    }

    // testando o Amount 

    [Fact]
    public void Amount_WhenNegative_ShouldHaveGreaterThanOrEqualZeroError()
    {
        var request = BuildValidRequest();
        request.Amount = -0.01m;

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Amount)
              .WithErrorMessage(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_OR_EQUAL_ZERO);
    }

    [Fact]
    public void Amount_WhenZeroAndStatusIsNotCancelled_ShouldNotHaveError()
    {
        var request = BuildValidRequest();
        request.Amount = 0;
        request.Status = Status.Pendente; 

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(x => x.Amount);
    }

    [Fact]
    public void Amount_WhenNotZeroAndStatusIsCancelled_ShouldHaveCancelledAmountError()
    {
        var request = BuildValidRequest();
        request.Amount = 50m;
        request.Status = Status.Cancelado;

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Amount)
              .WithErrorMessage(ResourceErrorMessages.AMOUNT_MUST_BE_ZERO_WHEN_CANCELLED);
    }

    [Fact]
    public void Amount_WhenZeroAndStatusIsCancelled_ShouldNotHaveCancelledAmountError()
    {
        var request = BuildValidRequest();
        request.Amount = 0;
        request.Status = Status.Cancelado;

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(x => x.Amount);
    }

    [Fact]
    public void Amount_WhenPositive_ShouldNotHaveError()
    {
        var request = BuildValidRequest();
        request.Amount = 100m;

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(x => x.Amount);
    }

    // testando o PaymentMethod 

    [Fact]
    public void PaymentMethod_WhenInvalidEnum_ShouldHaveError()
    {
        var request = BuildValidRequest();
        request.PaymentMethod = (PaymentMethod)999;

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.PaymentMethod)
              .WithErrorMessage(ResourceErrorMessages.PAYMENT_METHOD_INVALID);
    }

    [Fact]
    public void PaymentMethod_WhenValid_ShouldNotHaveError()
    {
        var request = BuildValidRequest();

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(x => x.PaymentMethod);
    }

    // testando os Status 

    [Fact]
    public void Status_WhenInvalidEnum_ShouldHaveError()
    {
        var request = BuildValidRequest();
        request.Status = (Status)999;

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Status)
              .WithErrorMessage(ResourceErrorMessages.STATUS_INVALID);
    }

    [Fact]
    public void Status_WhenValid_ShouldNotHaveError()
    {
        var request = BuildValidRequest();

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(x => x.Status);
    }

    // trestando as Notes 

    [Fact]
    public void Notes_WhenNull_ShouldNotHaveError()
    {
        var request = BuildValidRequest();
        request.Notes = null;

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(x => x.Notes);
    }

    [Fact]
    public void Notes_WhenWithin500Chars_ShouldNotHaveError()
    {
        var request = BuildValidRequest();
        request.Notes = new string('a', 500);

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(x => x.Notes);
    }

    [Fact]
    public void Notes_WhenExceeds500Chars_ShouldHaveMaxLengthError()
    {
        var request = BuildValidRequest();
        request.Notes = new string('a', 501);

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Notes)
              .WithErrorMessage(ResourceErrorMessages.NOTES_MAX_LENGTH);
    }

    // Full valid object 

    [Fact]
    public void AllFields_WhenValid_ShouldNotHaveAnyError()
    {
        var request = BuildValidRequest();

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    // Builder 

    private static RequestBillingsJson BuildValidRequest() => new()
    {
        Date = DateTime.Today,
        BarberName = "João Silva",
        ClientName = "Carlos Souza",
        ServiceName = "Haircut",
        Amount = 50m,
        PaymentMethod = PaymentMethod.CreditCard,   
        Status = Status.Pendente,       
        Notes = null
    };
}
