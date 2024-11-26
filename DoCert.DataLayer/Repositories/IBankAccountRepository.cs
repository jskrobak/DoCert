using DoCert.Model;
using Havit.Data.EntityFrameworkCore.Patterns.QueryServices;
using Havit.Data.Patterns.Repositories;

namespace DoCert.DataLayer.Repositories;

public interface IBankAccountRepository: IRepository<BankAccount>
{
    Task<DataFragment<BankAccount>> GetFragmentAsync(DonorFilter filter, int start, int? count,
        CancellationToken cancellationToken = default);
    
    Task<BankAccount?> GetAccountByNumberAsync(string number, CancellationToken cancellationToken = default);
}