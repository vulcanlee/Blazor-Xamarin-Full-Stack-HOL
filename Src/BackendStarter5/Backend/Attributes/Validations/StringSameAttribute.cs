using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Attributes.Validations
{
    public class StringSameAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public StringSameAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessageString;
            var currentValue = (string)value;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (property == null)
                throw new ArgumentException("Property with this name not found");

            var comparisonValue = (string)property.GetValue(validationContext.ObjectInstance);

            if (currentValue == comparisonValue)
                return ValidationResult.Success;
            else
                return new ValidationResult(ErrorMessage);

        }
    }
}
