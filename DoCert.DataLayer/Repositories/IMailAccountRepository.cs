using DoCert.Model;
using Havit.Data.Patterns.Repositories;

namespace DoCert.DataLayer.Repositories;

public interface IMailAccountRepository: IRepository<MailAccount>
{
    Task<MailAccount> GetDefaultAsync(CancellationToken cancellationToken);
}