namespace Infrastructure.Validator.Test
{
	using System.Collections.Generic;

	using FluentValidation.Results;

	using Infrastructure.Validator.Utility;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
    public class FluentValidationExtensionsTest
    {
        [TestMethod]
        public void ToValidationResultMustSucceed()
        {
            var validationResult = new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("ProductId","Not be empty"),
                new ValidationFailure("ProductName","Not be empty")
            });

            var result = validationResult.ToValidationResult();
            Assert.AreEqual(2, result.Errors.Count);
        }
    }
}
