namespace ThomasonAlgorithm.obj;

public static class ListExtensions
{
    public static int? FirstPossibleNeighborOrNull(this List<int> source, Func<int, bool> predicate)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (predicate == null)
        {
            throw new ArgumentNullException(nameof(predicate));
        }

        foreach (int element in source)
        {
            if (predicate(element))
            {
                return element;
            }
        }

        return null;
    }
}