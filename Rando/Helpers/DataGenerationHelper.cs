using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bogus.DataSets;
using Bogus;
using Rando.Models;

namespace Rando.Helpers
{
    public class DataGenerationHelper
    {
        public DataGenerationHelper() { }

        public static T? GenerateRandomData<T>() where T : class, new()
        {
            var faker = new Faker();

            if (typeof(T) == typeof(User))
            {
                var user = new Faker<User>()
                    .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                    .RuleFor(u => u.LastName, f => f.Name.LastName())
                    .RuleFor(u => u.Email, f => f.Internet.Email())
                    .Generate();
                return user as T;
            }
            else if (typeof(T) == typeof(Bank))
            {
                var bank = new Faker<Bank>()
                    .RuleFor(b => b.BankName, f => f.Company.CompanyName())
                    .RuleFor(b => b.AccountNumber, f => f.Finance.Account())
                    .Generate();
                return bank as T;
            }
            else if (typeof(T) == typeof(CreditCard))
            {
                var creditCard = new Faker<CreditCard>()
                    .RuleFor(c => c.CardNumber, f => f.Finance.CreditCardNumber())
                    .RuleFor(c => c.ExpiryDate, f => f.Date.Future())
                    .Generate();
                return creditCard as T;
            }
            else if (typeof(T) == typeof(Address))
            {
                var address = new Faker<Address>()
                    .RuleFor(a => a.StreetName, f => f.Address.StreetAddress())
                    .RuleFor(a => a.City, f => f.Address.City())
                    .RuleFor(a => a.ZipCode, f => f.Address.ZipCode())
                    .Generate();
                return address as T;
            }
            else
            {
                throw new ArgumentException("Unsupported type");
            }
        }
    }
}
