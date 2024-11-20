using Bogus;
using Bogus.Extensions.UnitedKingdom;
using DoCert.Model;

namespace DoCert.Services;

public class FakeDataService
{
    public List<Donate> PrepareFakeDonates()
    {
        var list = new List<Donate>();
        
        var faker = new Faker("cz");

        for (var i = 1; i <= 200; i++)
        {
            var donor = new Donor()
            {
                Id = i,
                Name = faker.Name.FullName(),
                Email = faker.Internet.Email(),
                Birthdate = faker.Date.Between(DateTime.Now.AddYears(-100), DateTime.Now.AddYears(-18)),
            };
            var acc = new BankAccount()
            {
                Id = i,
                AccountNumber = faker.Finance.Account()
            };

            for (var j = 1; j <= 12; j++)
            {
                list.Add(new Donate()
                {
                    Id = (i * 100) + j,
                    Amount = faker.Random.Number(200, 15000),
                    Date = faker.Date.Past(1),
                    Donor = donor,
                    DonorId = donor.Id,
                    BankAccount = acc,
                    BankAccountId = acc.Id,
                    VariableSymbol = faker.Finance.SortCode(),
                    SpecificSymbol = faker.Finance.SortCode(),
                    ConstantSymbol = faker.Finance.SortCode(),
                    Message = faker.Lorem.Sentence()
                });
            }
        }
        
        return list;
    }
}
    
    
