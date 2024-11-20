using DoCert.Contracts;
using Havit.Data.EntityFrameworkCore.Patterns.QueryServices;

namespace DoCert.DataLayer;

public static class DataFragmentExtensions
{
    public static DataFragmentResult<TItem> ToDataFragmentResult<TItem>(this DataFragment<TItem> source)
    {
        return new DataFragmentResult<TItem>
        {
            Data = source.Data,
            TotalCount = source.TotalCount
        };
    }
}