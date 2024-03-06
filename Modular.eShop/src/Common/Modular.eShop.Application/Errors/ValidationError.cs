using FluentResults;
using FluentValidation.Results;

namespace Modular.eShop.Application.Errors;

public class ValidationError : Error
{
    public ValidationError(List<ValidationFailure> failures)
    {
        Failures = failures;
    }

    public List<ValidationFailure> Failures { get; }
}
