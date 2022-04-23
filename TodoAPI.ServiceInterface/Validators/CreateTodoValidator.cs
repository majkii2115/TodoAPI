using ServiceStack.FluentValidation;
using TodoAPI.DbModels;
using TodoAPI.ServiceModel;

namespace TodoAPI.ServiceInterface;
public class CreateTodoValidator : AbstractValidator<CreateTodo>
{
    public CreateTodoValidator()
    {
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description should not be empty.");
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title should not be empty.");
        RuleFor(x => x.ExpirationDate).NotEmpty().WithMessage("Expiration date should not be empty and in proper format.");
    }
}
