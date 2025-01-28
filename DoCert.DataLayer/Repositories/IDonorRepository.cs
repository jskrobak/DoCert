using DoCert.Model;
using Havit.Blazor.Components.Web.Bootstrap;
using Havit.Data.EntityFrameworkCore.Patterns.QueryServices;
using Havit.Data.Patterns.Repositories;

namespace DoCert.DataLayer.Repositories;

public interface IDonorRepository: IRepository<Donor>
{
    Task<DataFragment<Donor>> GetFragmentAsync(DonorFilter filter, GridDataProviderRequest<Donor> request,
        CancellationToken cancellationToken = default);
    
    Task<Donor?> GetDonorByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<Donor>> GetAllWithoutCertificate();
}