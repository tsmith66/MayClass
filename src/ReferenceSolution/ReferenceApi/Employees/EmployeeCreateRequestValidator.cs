using FluentValidation;

namespace ReferenceApi.Employees;

public class EmployeeCreateRequestValidator : AbstractValidator<EmployeeCreateRequest>
{
    public EmployeeCreateRequestValidator()
    {
        RuleFor(e => e.FirstName).NotEmpty();
        RuleFor(e => e.FirstName)
            .MinimumLength(3).WithMessage("We need a longer first name")
            .MaximumLength(256);
        RuleFor(e => e.LastName)
            .MinimumLength(3)
            .MaximumLength(256)
            .When(e => !string.IsNullOrEmpty(e.LastName));
    }
}