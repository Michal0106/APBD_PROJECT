using System.ComponentModel.DataAnnotations;

namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Validators;

using System;
using System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property)]
public class EndDateValidationAttribute : ValidationAttribute
{
    private readonly string _startDatePropertyName;

    public EndDateValidationAttribute(string startDatePropertyName)
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

        if (endDate <= startDate)
        {
            return new ValidationResult("End Date must be after Start Date.");
        }

        return ValidationResult.Success;
    }
}
