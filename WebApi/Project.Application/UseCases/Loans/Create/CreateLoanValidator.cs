using FluentValidation;

namespace Project.Application.UseCases.Loans.Create
{
    public class CreateLoanValidator : AbstractValidator<CreateLoanCommand>
    {
        public CreateLoanValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.ResourceId).NotEmpty();
            RuleFor(x => x.StartDate).NotEmpty();
            RuleFor(x => x.DueDate).GreaterThan(x => x.StartDate);
        }
    }
}
