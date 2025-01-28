using DoCert.Model;
using Havit.Blazor.Components.Web.Bootstrap;
using Havit.Data.EntityFrameworkCore.Patterns.QueryServices;
using Havit.Data.Patterns.Repositories;

namespace DoCert.DataLayer.Repositories;

public interface IDonateRepository: IRepository<Donate>
{
    Task<DataFragment<Donate>> GetFragmentAsync(DonateFilter filter, GridDataProviderRequest<Donate> request,
        CancellationToken cancellationToken = default);

    public Task<double> CalculateDonatesSumAsync(int donorId, int year,
        CancellationToken cancellationToken = default);
}