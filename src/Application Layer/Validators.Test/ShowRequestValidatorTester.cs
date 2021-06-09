using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rtl.TvMaze.Api.Dto;

namespace Rtl.TvMaze.Validators.Test
{
    [TestClass]
    public class ShowRequestValidatorTester
    {
        [TestMethod]
        public void Should_have_error_when_PageNo_is_0_or_less()
        {
            //Setup
            var validator = new ShowRequestValidator();
            var request = new ShowRequest { PageSize = 10, PageNo = 0};

            //Act
            var result = validator.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(c => c.PageNo);
        }

        [TestMethod]
        public void Should_have_error_when_PageSize_is_0_or_less()
        {
            //Setup
            var validator = new ShowRequestValidator();
            var request = new ShowRequest { PageSize = 0, PageNo = 1 };

            //Act
            var result = validator.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(c => c.PageSize);
        }
    }
}
