using System.Data;

namespace underware.Data.Patterns.Repositories.Exceptions;

public class ObjectNotFoundException : DataException
{
    /// <summary>Konstruktor.</summary>
    /// <remarks>
    /// Pro možnost použití s Moq - Throws vyžaduje typ výjimky s bez parametrickým konstruktorem.
    /// </remarks>
    public ObjectNotFoundException()
    {
    }

    /// <summary>Konstruktor.</summary>
    /// <param name="message">Popis výjimky.</param>
    public ObjectNotFoundException(string message)
        : base(message)
    {
    }
}