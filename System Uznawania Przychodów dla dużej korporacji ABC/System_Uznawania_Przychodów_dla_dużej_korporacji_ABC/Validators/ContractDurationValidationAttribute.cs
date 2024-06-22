using System.ComponentModel.DataAnnotations;

namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Validators;

public class ContractDurationValidationAttribute : ValidationAttribute
{
    private readonly string _startDatePropertyName;

    public ContractDurationValidationAttribute(string startDatePropertyName)
    {
        _startDatePropertyName = startDatePropertyName;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var endDate = (DateTime)value;

        var startDateProperty = validationContext.ObjectType.GetProperty(_startDatePropertyName);
        if (startDateProperty == null)
        {
            throw new ArgumentException("Invalid property name.");
        }

        var startDate = (DateTime)startDateProperty.GetValue(validationContext.ObjectInstance);

        var duration = endDate - startDate;
        
        if (duration.TotalDays < 3 || duration.TotalDays > 30)
        {
            return new ValidationResult("Contract duration must be between 3 and 30 days.");
        }

        return ValidationResult.Success;
    }
}