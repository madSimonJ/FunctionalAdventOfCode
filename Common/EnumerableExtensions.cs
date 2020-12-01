using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public static class EnumerableExtensions
    {
        public static int AggregateUntil<TInput, TOutput>(this IEnumerable<TInput> @this, TOutput seed, Func<TOutput, TInput, TOutput> aggregateFunc, TOutput valueToStopAt)
        {
            var e = @this.GetEnumerator();
            e.MoveNext();

            int IterateThis(TOutput curr, int i = 0)
            {
                var updatedAggregate = aggregateFunc(curr, e.Current);
                return EqualityComparer<TOutput>.Default.Equals(updatedAggregate, valueToStopAt)
                    ? i + 1
                    : e.MoveNext()
                        ? IterateThis(updatedAggregate, i + 1)
                        : -1;
            }

            var returnValue = IterateThis(seed);
            return returnValue;
        }

        public static TOut AggregateAdjacent<TIn, TOut>(this IEnumerable<TIn> @this, TOut seed, Func<TOut, TIn, TIn, TOut> f)
        {
            var e = @this.GetEnumerator();
            e.MoveNext();
            TOut iterate(TOut currTotal)
            {
                var prev = e.Current;
                return e.MoveNext()
                                    ? iterate(f(currTotal, prev, e.Current))
                                    : currTotal;
            }
            return iterate(seed);
        }

        public static string Join<T>(this IEnumerable<T> @this) =>
            string.Join("", @this.Select(x => x.ToString()));

        public static bool AnyAdjacent<T>(this IEnumerable<T> @this, Func<T, T, bool> predicate)
        {
            var enumerator = @this.GetEnumerator();
            enumerator.MoveNext();
            var prev = enumerator.Current;

            while (enumerator.MoveNext())
            {
                if (predicate(prev, enumerator.Current))
                    return true;
                prev = enumerator.Current;
            }
            return false;
        }

        public static bool AnyAdjacent<T>(this IEnumerable<T> @this, Func<T, T, int, bool> predicate)
        {
            var enumerator = @this.GetEnumerator();
            enumerator.MoveNext();
            var prev = enumerator.Current;
            int i = 0;

            while (enumerator.MoveNext())
            {
                if (predicate(prev, enumerator.Current, i))
                    return true;
                prev = enumerator.Current;
                i++;
            }
            return false;
        }

        public static bool AnyAdjacentTwo<T>(this IEnumerable<T> @this, Func<T, T, T, bool> predicate)
        {
            var enumerator = @this.GetEnumerator();
            enumerator.MoveNext();
            var twoPlacesBack = enumerator.Current;
            enumerator.MoveNext();
            var onePlaceBack = enumerator.Current;

            while (enumerator.MoveNext())
            {
                if (predicate(twoPlacesBack, onePlaceBack, enumerator.Current))
                    return true;
                twoPlacesBack = onePlaceBack;
                onePlaceBack = enumerator.Current;
            }
            return false;
        }

        public static IEnumerable<T> Replace<T>(this IEnumerable<T> @this, Func<T, bool> predicate, T replacement) =>
            @this.Select(x =>
                predicate(x)
                    ? replacement
                    : x);

        public static IEnumerable<T> Except<T>(this IEnumerable<T> @this, T itemToExcemt) =>
            @this.Where(x => !EqualityComparer<T>.Default.Equals(x, itemToExcemt));

        public static IEnumerable<(T, T)> Permutate<T>(this IEnumerable<T> source)
        {
            var sourceArray = source.ToArray();
            var returnVal = sourceArray.SelectMany((sourceX, sourceI) => {
                var recordsToMerge = sourceArray.Where((mergeX, mergeI) => mergeI > sourceI && sourceX.NotEq(mergeX));
                return recordsToMerge.Select(x => (sourceX, x));
            });
            return returnVal;
        }

        public static IEnumerable<IEnumerable<T>> PermutateRoutes<T>(this IEnumerable<T> @this)
        {
            return permutateRoutes(Enumerable.Empty<T>(), @this);
            IEnumerable<IEnumerable<T>> permutateRoutes(IEnumerable<T> done, IEnumerable<T> toDo) =>
                !toDo.Any()
                    ? new[] { done }
                    : toDo.SelectMany(x =>
                        permutateRoutes(
                                        done.Append(x),
                                        toDo.Except(x)
                        )
                    );
        }
    }
}
