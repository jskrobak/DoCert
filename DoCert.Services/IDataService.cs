using DoCert.Contracts;
using DoCert.Model;
using Havit.Blazor.Components.Web.Bootstrap;

namespace DoCert.Services;

public interface IDataService
{
    public Task<DataFragmentResult<Donate>> GetDonatesDataFragmentAsync(DonateFilter filter,
        GridDataProviderRequest<Donate> request, CancellationToken cancellationToken = default);

    public Task InsertDonatesAsync(List<Donate> donates, CancellationToken cancellationToken = default);
    Task DeleteDonateAsync(Donate donate, CancellationToken cancellationToken = default);

    Task ImportDonatesFromCsvAsync(StreamReader streamReader, ImportProfile profile,
        CancellationToken cancellationToken = default);

    Task<List<ImportProfile>> GetImportProfilesAsync(CancellationToken cancellationToken = default);
}