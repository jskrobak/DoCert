using System.Collections;

namespace underware.Data.EntityFrameworkCore.Patterns.UnitOfWorks;

  /// <summary>Změny objektů v UnitOfWork.</summary>
  public class Changes : IEnumerable<Change>, IEnumerable
  {
    private List<Change> changes;

    /// <summary>Konstruktor.</summary>
    public Changes(IEnumerable<Change> changes) => this.changes = changes.ToList<Change>();

    /// <summary>
    /// Registrované objekty pro Insert. Pro zpětnou kompatibilitu.
    /// </summary>
    [Obsolete("Changes samotné je nyní IEnumerable<Change>, obsahuje nejen entity, ale i informace k nim.")]
    public object[] Inserts
    {
      get
      {
        return this.Where<Change>((Func<Change, bool>) (item => item.ChangeType == ChangeType.Insert)).Select<Change, object>((Func<Change, object>) (item => item.Entity)).ToArray<object>();
      }
    }

    /// <summary>
    /// Registrované objekty pro Update. Pro zpětnou kompatibilitu.
    /// </summary>
    [Obsolete("Changes samotné je nyní IEnumerable<Change>, obsahuje nejen entity, ale i informace k nim.")]
    public object[] Updates
    {
      get
      {
        return this.Where<Change>((Func<Change, bool>) (item => item.ChangeType == ChangeType.Update)).Select<Change, object>((Func<Change, object>) (item => item.Entity)).ToArray<object>();
      }
    }

    /// <summary>
    /// Registrované objekty pro Delete. Pro zpětnou kompatibilitu.
    /// </summary>
    [Obsolete("Changes samotné je nyní IEnumerable<Change>, obsahuje nejen entity, ale i informace k nim.")]
    public object[] Deletes
    {
      get
      {
        return this.Where<Change>((Func<Change, bool>) (item => item.ChangeType == ChangeType.Delete)).Select<Change, object>((Func<Change, object>) (item => item.Entity)).ToArray<object>();
      }
    }

    /// <summary>
    /// Vrátí všechny změněné objekty (bez ohledu na způsob změny).
    /// Pro zpětnou kompatibilitu.
    /// </summary>
    [Obsolete("Changes samotné je nyní IEnumerable<Change>, obsahuje nejen entity, ale i informace k nim.")]
    public object[] GetAllChanges()
    {
      return this.Select<Change, object>((Func<Change, object>) (item => item.Entity)).ToArray<object>();
    }

    IEnumerator<Change> IEnumerable<Change>.GetEnumerator()
    {
      return (IEnumerator<Change>) this.changes.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.changes.GetEnumerator();
  }