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

        public static int Blah<T>(this IEnumerable<T> @this) => 3;
        public static int Blah2<T>(this T @this) => 3;

        public static IEnumerable<T> Replace<T>(this IEnumerable<T> @this, Func<T, bool> predicate, T replacement) =>
            @this.Select(x =>
                predicate(x)
                    ? replacement
                    : x);
    }
}
