namespace Rando.Models;

public class User
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public object Email { get; internal set; }
}
