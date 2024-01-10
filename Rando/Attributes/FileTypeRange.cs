using System.ComponentModel.DataAnnotations;

public class FileTypeRangeAttribute : ValidationAttribute
{
    public string[] AllowableValues { get; set; }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var msg = $"Please enter one of the allowable values: {string.Join(", ", (AllowableValues ?? new string[] { "No allowable values found" }))}.";
        
        if (value == null || value.ToString().Equals(""))
            return new ValidationResult(msg);

        foreach(var allowableValue in AllowableValues)
        {
            if(value.ToString().EndsWith(allowableValue)) {
                return ValidationResult.Success;
            }
        }

        return new ValidationResult(msg);
    }
}