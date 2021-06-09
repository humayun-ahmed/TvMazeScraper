
namespace Infrastructure.Validator.Utility
{
	using ValidationResult = Infrastructure.Validator.Contract.ValidationResult;

	public static class FluentValidationExtensions
    {
        public static ValidationResult ToValidationResult(this global::FluentValidation.Results.ValidationResult validationResult)
        {
            var result = new ValidationResult();
            if (validationResult == null)
                return result;

            foreach (var validationFailure in validationResult.Errors)
                result.AddError(validationFailure.ErrorMessage, validationFailure.PropertyName, validationFailure.ErrorCode, validationFailure.AttemptedValue);

            return result;
        }
    }
}
