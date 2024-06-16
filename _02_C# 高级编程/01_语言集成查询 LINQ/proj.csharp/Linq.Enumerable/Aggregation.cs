using System.Numerics;

namespace Linq;
// 聚合
public static partial class Enumerable_Aggregation
{
    // Aggregate
    public static TSource Aggregate<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, TSource> func)
    {
        Common.NullCheck(source, func);

        using (IEnumerator<TSource> e = source.GetEnumerator())
        {
            if (!e.MoveNext())
                throw new InvalidOperationException("Sequence contains no elements");
            TSource result = e.Current;
            while (e.MoveNext())
                result = func(result, e.Current);
            return result;
        }
    }
    public static TAccumulate Aggregate<TSource, TAccumulate>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
    {
        Common.NullCheck(source, func);

        TAccumulate result = seed;
        foreach (TSource element in source)
            result = func(result, element);
        return result;
    }

    public static TResult Aggregate<TSource, TAccumulate, TResult>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate, TResult> resultSelector)
    {
        Common.NullCheck(source, func, resultSelector);

        TAccumulate result = seed;
        foreach (TSource element in source)
            result = func(result, element);
        return resultSelector(result);
    }
    // Average
    public static double Average(this IEnumerable<int> source)
    {
        using (IEnumerator<int> e = source.GetEnumerator())
        {
            if (!e.MoveNext())
                throw new InvalidOperationException("Sequence contains no elements");

            long sum = e.Current;
            long count = 1;

            while (e.MoveNext())
            {
                checked { sum += e.Current; }
                count++;
            }

            return (double)sum / count;
        }
    }


}


