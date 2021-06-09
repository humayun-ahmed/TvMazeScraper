using FluentValidation;
using Infrastructure.Validator.Contract;
using Infrastructure.Validator.Utility;
using Rtl.TvMaze.Api.Dto;

namespace Rtl.TvMaze.Validators
{
    public class ShowRequestValidator : AbstractValidator<ShowRequest>, Infrastructure.Validator.Contract.IValidator<ShowRequest>
    {
        public ShowRequestValidator()
        {
            RuleFor(c => c.PageNo).GreaterThan(0);
            RuleFor(c => c.PageSize).NotEmpty().NotNull();
        }

        /*public ValidationResult PerformValidation(ShowRequest model) =>
            Validate(model).ToValidationResult();*/

        public ValidationResult PerformValidation(ShowRequest model)
        {
            return Validate(model).ToValidationResult();
        }
    }
}
