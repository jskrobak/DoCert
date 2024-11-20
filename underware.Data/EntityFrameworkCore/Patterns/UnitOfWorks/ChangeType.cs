using System.Diagnostics;

namespace underware.Data.EntityFrameworkCore.Patterns.UnitOfWorks;

/// <summary>Typ změny.</summary>
public enum ChangeType
{
    /// <summary>Delete</summary>
    [DebuggerDisplay("Delete")] Delete = 2,
    /// <summary>Update</summary>
    [DebuggerDisplay("Update")] Update = 3,
    /// <summary>Insert</summary>
    [DebuggerDisplay("Insert")] Insert = 4,
}