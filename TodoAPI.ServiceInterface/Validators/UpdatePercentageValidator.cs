using ServiceStack.FluentValidation;
using TodoAPI.DbModels;
using TodoAPI.ServiceModel;

namespace TodoAPI.ServiceInterface;
public class UpdatePercentageValidator : AbstractValidator<UpdatePercentage>
{
    public UpdatePercentageValidator()
    {
        RuleFor(x => x.PercentageOfCompleteness).NotEmpty().InclusiveBetween(0, 100).WithMessage("Percentage should not be empty and between 0 and 100.");
    }
}
