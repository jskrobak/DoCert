using DoCert.Model;
using Havit.Data.Patterns.Repositories;

namespace DoCert.DataLayer.Repositories;

public interface IAgendaRepository: IRepository<Agenda>
{
    Task<Agenda> GetDefaultAsync(CancellationToken cancellationToken = default);
}