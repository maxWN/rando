using System.ComponentModel.DataAnnotations;

namespace Rando.Common;

public class UserInput
{
    public UserInput()
    { }

    [Required]
    [StringRange(AllowableValues = new[] { Common.DataType.USERS, Common.DataType.ADDRESSES, Common.DataType.APPLIANCES, Common.DataType.BANKS, Common.DataType.CREDIT_CARDS, Common.DataType.BLOOD_TYPES, Common.DataType.BEERS }, ErrorMessage = "Type must be a defined type.")]
    public string DataType { get; set; }

    [Range(1, 100, ErrorMessage = "Quantity cannot exceed 100.")]
    public int Quantity { get; set; }

    [StringRange(AllowableValues = new[] { Common.FlagType.ApiFlag, Common.FlagType.DatabaseFlag, Common.FlagType.FileFlag, "" }, ErrorMessage = "Type must be a defined type.")]
    public string FlagType { get; set; }

    [FileTypeRange(AllowableValues = new[] { ".txt", ".json", ".csv" }, ErrorMessage = "Value must contain allowed file type extension.")]
    public string FileName { get; set; }

    // TODO: Create new attribute to verify file path works for OS of end user
    public string FilePath { get; set; }

    // TODO: Create new attribute to verify arg supplied matches database name specified in config file
    public string TableName { get; set; }

    // TODO: Add new regex decorator to check if URL is valid (or matches config property)
    public string ApiUrl { get; set; }
}