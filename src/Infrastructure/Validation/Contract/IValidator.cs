namespace Infrastructure.Validator.Contract
{
    public interface IValidator<in T> where T: class 
    {
        ValidationResult PerformValidation(T model);
    }
}
