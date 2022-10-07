namespace System.Collections.Generic;

public class BinaryTree<T> : ICollection<T>, IEnumerable<T>
    where T : IComparable<T>
{
    public BinaryTreeNode<T>? RootNode { get; private set; }

    public int Count => RootNode?.GetWeight() ?? 0;

    public bool IsReadOnly => false;

    public void Insert(T value)
    {
        if (RootNode == null)
            RootNode = new BinaryTreeNode<T>(value);
        else
            RootNode.Insert(value);
    }

    public void InsertRange(IEnumerable<T> values)
    {
        IEnumerator<T> enumerator = values.GetEnumerator();
        if (!enumerator.MoveNext())
            return;

        BinaryTreeNode<T>? root = RootNode;

        if (root == null)
            root = RootNode = new BinaryTreeNode<T>(enumerator.Current);
        else
            root.Insert(enumerator.Current);

        while (enumerator.MoveNext())
            root.Insert(enumerator.Current);
    }

    public bool Remove(T value)
    {
        if (RootNode == null)
            return false;

        if (!RootNode.Value.Equals(value))
            return RootNode.Remove(value);

        RootNode = null;
        return true;
    }
    public void Clear()
    {
        RootNode = null;
    }
    public bool Contains(T item)
    {
        if (RootNode == null)
            return false;
        if (!RootNode.Value.Equals(item))
            return RootNode.Contains(item);
        return true;
    }

    public void MakeBalanced()
    {
        if (RootNode == null)
            throw new InvalidOperationException("No element");
        RootNode = RootNode.MakeBalanced();
    }

    public string Visualize()
    {
        return RootNode == null ? string.Empty : RootNode.Visualize();
    }

    IEnumerator<T> EmptyEnumerator() { yield break; }

    public IEnumerator<T> GetEnumerator()
    {

        if (RootNode == null)
            return EmptyEnumerator();
        else
            return RootNode.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    void ICollection<T>.Add(T item) => Insert(item);
    public void CopyTo(T[] array, int arrayIndex)
    {
        int i = arrayIndex;
        foreach (var item in this)
            array[i++] = item;
    }
}
