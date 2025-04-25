using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class IsValidEmailAddress : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var email = value as string;

        if (string.IsNullOrWhiteSpace(email))
        {
            return new ValidationResult("Email is required.");
        }

        var isValidFormat = Regex.IsMatch(email,
            @"^[^@\s]+@technzone\.com$", RegexOptions.IgnoreCase);

        if (!isValidFormat)
        {
            return new ValidationResult("Email must be in the format *@technzone.com");
        }

        return ValidationResult.Success;
    }
}
