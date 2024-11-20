using DoCert.Contracts;
using DoCert.Model;

namespace DoCert.Services;

public interface IDataService
{
    public Task<DataFragmentResult<Donate>> GetDonatesAsync(int start, int? count, CancellationToken cancellationToken = default);
    public Task InsertDonatesAsync(List<Donate> donates, CancellationToken cancellationToken = default);
}