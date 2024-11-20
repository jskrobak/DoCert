using System.Diagnostics.Contracts;

namespace underware.Linq;

  /// <summary>Extension methods for IEnumerable&lt;T&gt;.</summary>
  public static class EnumerableExt
  {
    /// <summary>
    /// Optionally extends the query according to source with a condition based on predicate if condition is true.
    /// If condition is false, returns the unmodified query.
    /// </summary>
    /// <typeparam name="TSource">Type of object in source.</typeparam>
    /// <param name="source">Extended query.</param>
    /// <param name="condition">Condition determining whether to extend the query with a condition.</param>
    /// <param name="predicate">Condition that optionally extends the query.</param>
    /// <returns>Query optionally extended with a condition.</returns>
    public static 
    #nullable disable
    IEnumerable<TSource> WhereIf<TSource>(
      this IEnumerable<TSource> source,
      bool condition,
      Func<TSource, bool> predicate)
    {
      return !condition ? source : source.Where<TSource>(predicate);
    }

    /// <summary>Left outer join.</summary>
    public static IEnumerable<TResult> LeftJoin<TLeft, TRight, TKey, TResult>(
      this IEnumerable<TLeft> leftSource,
      IEnumerable<TRight> rightSource,
      Func<TLeft, TKey> leftKeySelector,
      Func<TRight, TKey> rightKeySelector,
      Func<TLeft, TRight, TResult> resultSelector)
    {
      return leftSource.GroupJoin(rightSource, (Func<TLeft, TKey>) (l => leftKeySelector(l)), (Func<TRight, TKey>) (r => rightKeySelector(r)), (l, joinData) => new
      {
        l = l,
        joinData = joinData
      }).SelectMany(_param1 => _param1.joinData.DefaultIfEmpty<TRight>(), (_param1, right) => resultSelector(_param1.l, right));
    }

    /// <summary>Right outer join.</summary>
    public static IEnumerable<TResult> RightJoin<TLeft, TRight, TKey, TResult>(
      this IEnumerable<TLeft> leftSource,
      IEnumerable<TRight> rightSource,
      Func<TLeft, TKey> leftKeySelector,
      Func<TRight, TKey> rightKeySelector,
      Func<TLeft, TRight, TResult> resultSelector)
    {
      return rightSource.GroupJoin(leftSource, (Func<TRight, TKey>) (r => rightKeySelector(r)), (Func<TLeft, TKey>) (l => leftKeySelector(l)), (r, joinData) => new
      {
        r = r,
        joinData = joinData
      }).SelectMany(_param1 => _param1.joinData.DefaultIfEmpty<TLeft>(), (_param1, left) => resultSelector(left, _param1.r));
    }

    /// <summary>
    /// Full outer join.
    /// Unlike other methods, full outer join is evaluated immediately, results are not affected by changes
    /// in source data (leftSource, rightSource) after calling this method and before actual query evaluation (IEnumerable).
    /// </summary>
    public static IEnumerable<TResult> FullOuterJoin<TLeft, TRight, TKey, TResult>(
      this IEnumerable<TLeft> leftSource,
      IEnumerable<TRight> rightSource,
      Func<TLeft, TKey> leftKeySelector,
      Func<TRight, TKey> rightKeySelector,
      Func<TLeft, TRight, TResult> resultSelector)
    {
      ILookup<TKey, TLeft> leftLookup = leftSource.ToLookup<TLeft, TKey>(leftKeySelector);
      ILookup<TKey, TRight> rightLookup = rightSource.ToLookup<TRight, TKey>(rightKeySelector);
      HashSet<TKey> source = new HashSet<TKey>(((IEnumerable<IGrouping<TKey, TLeft>>) leftLookup).Select<IGrouping<TKey, TLeft>, TKey>((Func<IGrouping<TKey, TLeft>, TKey>) (p => p.Key)));
      source.UnionWith(((IEnumerable<IGrouping<TKey, TRight>>) rightLookup).Select<IGrouping<TKey, TRight>, TKey>((Func<IGrouping<TKey, TRight>, TKey>) (p => p.Key)));
      return (IEnumerable<TResult>) source.SelectMany((Func<TKey, IEnumerable<TLeft>>) (key => leftLookup[key].DefaultIfEmpty<TLeft>()), (key, xLeft) => new
      {
        key = key,
        xLeft = xLeft
      }).SelectMany(_param1 => rightLookup[_param1.key].DefaultIfEmpty<TRight>(), (_param1, xRight) => resultSelector(_param1.xLeft, xRight)).ToList<TResult>();
    }

    /// <summary>Skip last items.</summary>
    public static IEnumerable<TSource> SkipLast<TSource>(IEnumerable<TSource> source, int count)
    {
      return count <= 0 ? source.Select<TSource, TSource>((Func<TSource, TSource>) (item => item)) : EnumerableExt.SkipLastInternal<TSource>(source, count);
    }

    private static IEnumerable<TSource> SkipLastInternal<TSource>(
      IEnumerable<TSource> source,
      int count)
    {
      int sourceItems;
      if (source is IList<TSource>)
      {
        IList<TSource> sourceList = (IList<TSource>) source;
        sourceItems = sourceList.Count;
        for (int i = 0; i < sourceItems - count; ++i)
          yield return sourceList[i];
        sourceList = (IList<TSource>) null;
      }
      else
      {
        IEnumerator<TSource> sourceEnumerator = source.GetEnumerator();
        TSource[] buffer = new TSource[count];
        for (sourceItems = 0; sourceItems < count && sourceEnumerator.MoveNext(); ++sourceItems)
          buffer[sourceItems] = sourceEnumerator.Current;
        sourceItems = 0;
        while (sourceEnumerator.MoveNext())
        {
          TSource source1 = buffer[sourceItems];
          buffer[sourceItems] = sourceEnumerator.Current;
          sourceItems = (sourceItems + 1) % count;
          yield return source1;
        }
        sourceEnumerator = (IEnumerator<TSource>) null;
        buffer = (TSource[]) null;
      }
    }

    /// <summary>Skip last items.</summary>
    public static IEnumerable<TSource> SkipLastWhile<TSource>(
      this IEnumerable<TSource> source,
      Func<TSource, bool> predicate)
    {
      List<TSource> buffer = new List<TSource>();
      foreach (TSource source1 in source)
      {
        TSource item = source1;
        if (predicate(item))
        {
          buffer.Add(item);
        }
        else
        {
          if (buffer.Count > 0)
          {
            foreach (TSource source2 in buffer)
              yield return source2;
            buffer.Clear();
          }
          yield return item;
        }
        item = default (TSource);
      }
    }

    /// <summary>Skip last items.</summary>
    public static IEnumerable<TSource> SkipLastWhile<TSource>(
      this IEnumerable<TSource> source,
      Func<TSource, int, bool> predicate)
    {
      List<TSource> buffer = new List<TSource>();
      int idx = 0;
      foreach (TSource source1 in source)
      {
        TSource item = source1;
        if (predicate(item, idx++))
        {
          buffer.Add(item);
        }
        else
        {
          if (buffer.Count > 0)
          {
            foreach (TSource source2 in buffer)
              yield return source2;
            buffer.Clear();
          }
          yield return item;
        }
        item = default (TSource);
      }
    }

    /// <summary>
    /// Splits data into segments (chunks) of maximum size according to size.
    /// For example, input data of 2500 records at size 1000 will be split into three segments (chunks) - 1000, 1000, and 500 records.
    /// </summary>
    /// <param name="source">Source data.</param>
    /// <param name="size">Size of one segment (chunk). The smallest possible value is 1.</param>
    public static IEnumerable<T[]> Chunkify<T>(this IEnumerable<T> source, int size)
    {
      Contract.Requires<ArgumentNullException>(source != null, nameof (source));
      Contract.Requires<ArgumentOutOfRangeException>(size > 0, nameof (size));
      using (IEnumerator<T> iter = source.GetEnumerator())
      {
        while (iter.MoveNext())
        {
          T[] array = new T[size];
          array[0] = iter.Current;
          int newSize;
          for (newSize = 1; newSize < size && iter.MoveNext(); ++newSize)
            array[newSize] = iter.Current;
          if (size != newSize)
            Array.Resize<T>(ref array, newSize);
          yield return array;
        }
      }
    }

    /// <summary>
    /// Indicates whether the collection contains all items of another collection.
    /// </summary>
    /// <param name="source">The collection in which we verify the existence of values.</param>
    /// <param name="lookupItems">The values whose existence we verify in the collection.</param>
    /// <returns>True if the source contains all elements of lookupItems.</returns>
    public static bool ContainsAll<T>(this IEnumerable<T> source, IEnumerable<T> lookupItems)
    {
      return !lookupItems.Except<T>(source).Any<T>();
    }
  }