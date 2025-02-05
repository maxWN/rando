namespace Rando.Models
{
    public class Bank
    {
        public int Id { get; set; }
        public string? Uid { get; set; }
        public string? AccountNumber { get; set; }
        public string? Iban { get; set; }
        public string? BankName { get; set; }
        public int RoutingNumber { get; set; }
        public string? SwiftBic { get; set; }
    }
}