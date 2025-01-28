using DoCert.Contracts;
using DoCert.Model;
using Havit.Blazor.Components.Web.Bootstrap;

namespace DoCert.Services;

public interface IDataService
{
    Task<DataFragmentResult<Donate>> GetDonatesDataFragmentAsync(DonateFilter filter,
        GridDataProviderRequest<Donate> request, CancellationToken cancellationToken = default);
    Task InsertDonatesAsync(List<Donate> donates, CancellationToken cancellationToken = default);
    Task DeleteDonateAsync(Donate donate, CancellationToken cancellationToken = default);
    Task DeleteDonatesAsync(IEnumerable<Donate> donates, CancellationToken cancellationToken = default);
    
    Task ImportDonatesFromExcelAsync(Stream stream, CancellationToken cancellationToken = default);
    Task ImportDonorsFromExcelAsync(Stream stream, CancellationToken cancellationToken = default);
    
    Task<DataFragmentResult<Donor>> GetDonorsDataFragmentAsync(DonorFilter filterModel, GridDataProviderRequest<Donor> request,
        CancellationToken requestCancellationToken = default);
    Task DeleteDonorAsync(Donor donor, CancellationToken cancellationToken = default);
    Task<Agenda> GetAgendaAsync(CancellationToken cancellationToken = default);
    Task InsertAgendaAsync(Agenda agenda, CancellationToken cancellationToken = default);
    Task<MailAccount> GetMailAccountAsync(CancellationToken cancellationToken = default);
    Task InsertMailAccountAsync(MailAccount mailAccount, CancellationToken cancellationToken = default);
    Task UpdateMailAccountAsync(MailAccount mailAccount, CancellationToken cancellationToken = default);
    Task UpdateAgendaAsync(Agenda agenda, CancellationToken cancellationToken = default);
    Task<DataFragmentResult<Certificate>> GetCertificatesDataFragmentAsync(CertificateFilter filterModel,
        GridDataProviderRequest<Certificate> request, CancellationToken requestCancellationToken);
    Task DeleteCertificateAsync(Certificate cert);
    Task<Certificate> CreateCertificateAsync(Donor donor, int year, CancellationToken cancellationToken = default);
    Task SaveAgendaAsync(Agenda agenda, CancellationToken cancellationToken = default);
    Task SaveCertificateAsync(Certificate cert, CancellationToken cancellationToken = default);
    Task SaveDonorAsync(Donor donor, CancellationToken cancellationToken = default);
    Task CalculateCertificates(CancellationToken cancellationToken = default);
    Task DeleteCertificatesAsync(IEnumerable<Certificate> list, CancellationToken cancellationToken = default);
    Task<IEnumerable<Donor>> GetDonorsWithoutCertificate();
}