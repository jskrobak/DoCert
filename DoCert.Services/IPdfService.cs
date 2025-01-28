using DoCert.Model;

namespace DoCert.Services;

public interface IPdfService
{
    byte[] CreateCertificate(Certificate cert, Agenda agenda);
}