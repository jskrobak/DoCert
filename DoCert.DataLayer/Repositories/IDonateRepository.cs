using DoCert.Model;
using Havit.Data.EntityFrameworkCore.Patterns.QueryServices;
using Havit.Data.Patterns.Repositories;

namespace DoCert.DataLayer.Repositories;

public interface IDonateRepository: IRepository<Donate>
{
    Task<DataFragment<Donate>> GetFragmentAsync(int start, int? count,
        CancellationToken cancellationToken = default);
}