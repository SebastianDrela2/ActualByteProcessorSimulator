namespace ActualProcessorSim.Collections;

public interface IReadOnlySubView<T> : IReadOnlyList<T>
{
    IReadOnlyList<T> Outer { get; }
    int Offset { get; }
}
public static class ReadOnlySubView
{
    public static ReadOnlySubView<T> From<T>(IReadOnlyList<T> items, Range range)
    {
        var (i, n) = range.GetOffsetAndLength(items.Count);
        return new(items, i, n);
    }
    public static ReadOnlySubView<T> SubList<T>(this IReadOnlyList<T> list, Range range) => From(list, range);
    public static ReadOnlySubView<T> SubList<T>(this IReadOnlyList<T> list, int offset, int length) => new(list, offset, length);
}
public class ReadOnlySubView<T> : IReadOnlySubView<T>
{
    public IReadOnlyList<T> Outer { get; }
    public int Offset { get; }
    public int Count { get; }

    public ReadOnlySubView(IReadOnlyList<T> outer, int offset, int length)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(length);
        ArgumentOutOfRangeException.ThrowIfNegative(offset);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(length, outer.Count);
        Outer = outer;
        Offset = offset;
        Count = length;
    }

    public T this[int index] => Outer[index + Offset];
    public ReadOnlySubView<T> this[Range range] => SubList(range);

    public ReadOnlySubView<T> SubList(Range range)
    {
        var (i, n) = range.GetOffsetAndLength(Count);
        return Slice(i, n);
    }
    public ReadOnlySubView<T> SubList(int offset, int length) => new(Outer, Offset + offset, length);
    public ReadOnlySubView<T> Slice(int offset, int length) => SubList(offset, length);

    public IEnumerable<T> AsEnumerable() => Outer.Skip(Offset).Take(Count);
    public IEnumerator<T> GetEnumerator() => AsEnumerable().GetEnumerator();
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
}
