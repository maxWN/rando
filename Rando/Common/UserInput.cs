using System.ComponentModel.DataAnnotations;
namespace Rando.Common;

public class UserInput {

    public UserInput() {}

    [Required]
    [StringRange(AllowableValues = new[] { Common.DataType.USERS, Common.DataType.ADDRESSES, Common.DataType.APPLIANCES, Common.DataType.BANKS, Common.DataType.CREDIT_CARDS }, ErrorMessage = "Type must be a defined type.")]
    public string DataType { get; set; }
    
    [Range(1,100, ErrorMessage="Value cannot exceed 100.")]
    public int Quantity { get; set; }

    [StringRange(AllowableValues = new[] { Common.FlagType.ApiFlag, Common.FlagType.DatabaseFlag, Common.FlagType.FileFlag, "" }, ErrorMessage = "Type must be a defined type.")]
    public string FlagType { get; set; }
    
    // custom attribute for end of string (substring) file type (json, txt, csv, etc)
    [FileTypeRange(AllowableValues = new[] { ".txt", ".json", ".csv" }, ErrorMessage = "Type must be a defined type.")]
    public string FileName { get; set; }
    
    // verify file path works for OS
    public string FilePath { get; set; }
    
    // must match database name specified in config file
    public string DatabaseName { get; set; }
    
    public string ApiUrl { get; set; }
}