using System.Diagnostics.Contracts;

namespace underware.Data.EntityFrameworkCore.Patterns.SoftDeletes;

/// <summary>
/// Extension metody pro snadné filtrování IQueryable&lt;T&gt; a IEnumerable&lt;T&gt; s pomocí ISoftDeleteManager.
/// </summary>
public static class SoftDeleteManagerExtensions
{
    /// <summary>Rozšíří dotaz o odstranění smazaných objektů.</summary>
    public static IQueryable<TSource> WhereNotDeleted<TSource>(
        this IQueryable<TSource> source,
        ISoftDeleteManager softDeleteManager)
    {
        Contract.Requires<ArgumentNullException>(source != null, "source != null");
        Contract.Requires<ArgumentNullException>(softDeleteManager != null, "softDeleteManager != null");
        return !softDeleteManager.IsSoftDeleteSupported<TSource>() ? source : Queryable.Where<TSource>(source, softDeleteManager.GetNotDeletedExpressionLambda<TSource>());
    }

    /// <summary>
    /// Vrací z dat pouze záznamy, které nejsou smazané příznakem.
    /// </summary>
    public static IEnumerable<TSource> WhereNotDeleted<TSource>(
        this IEnumerable<TSource> source,
        ISoftDeleteManager softDeleteManager)
    {
        Contract.Requires<ArgumentNullException>(source != null, "source != null");
        Contract.Requires<ArgumentNullException>(softDeleteManager != null, "softDeleteManager != null");
        return !softDeleteManager.IsSoftDeleteSupported<TSource>() ? source : source.Where<TSource>(softDeleteManager.GetNotDeletedCompiledLambda<TSource>());
    }
}