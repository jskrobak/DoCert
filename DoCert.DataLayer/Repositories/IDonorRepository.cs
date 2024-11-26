using DoCert.Model;
using Havit.Data.EntityFrameworkCore.Patterns.QueryServices;
using Havit.Data.Patterns.Repositories;

namespace DoCert.DataLayer.Repositories;

public interface IDonorRepository: IRepository<Donor>
{
    Task<DataFragment<Donor>> GetFragmentAsync(DonorFilter filter, int start, int? count,
        CancellationToken cancellationToken = default);
    
    Task<Donor?> GetDonorByNameAsync(string name, CancellationToken cancellationToken = default);
}