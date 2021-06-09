namespace Infrastructure.Validator.Contract
{
    public class ValidationFailure
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string PropertyName { get; set; }
        public object AttemptedValue { get; set; }
    }
}
