using System.Runtime.CompilerServices;

[CollectionBuilder(typeof(MyCollectionBuilder), "Build")]
public class MyCollection
{
    public readonly int[] Values;
    public MyCollection(int[] arr) => Values = arr;
    public IEnumerator<int> GetEnumerator() => Values.AsEnumerable().GetEnumerator();

    internal class MyCollectionBuilder
    {
        internal static MyCollection Build(ReadOnlySpan<int> arr) => new MyCollection(arr.ToArray());
    }
}

