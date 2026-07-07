using FluentValidation;
using MyRecipeBook.Application.UseCases.Shared.Validators;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.User.Register
{
    public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
    {
        public RegisterUserValidator()
        {
            RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_NAME_REQUIRED);
            RuleFor(user => user.Email).NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_EMAIL_REQUIRED);
            RuleFor(user => user.Password).Password();
            When(user => user.Email.IsNotEmpty(), () =>
            {
                RuleFor(user => user.Email).EmailAddress().WithMessage(ResourceMessagesException.VALIDATION_EMAIL_INVALID);
            });
        }
    }
}
