using System.ComponentModel.DataAnnotations;

public class StringRangeAttribute : ValidationAttribute
{
    public required string[] AllowableValues { get; set; }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (AllowableValues?.Contains(value?.ToString()) == true)
        {
            return ValidationResult.Success;
        }

        var msg = $"Please enter one of the allowable values: {string.Join(", ", AllowableValues ?? new string[] { "No allowable values found" })}.";
        return new ValidationResult(msg);
    }
}