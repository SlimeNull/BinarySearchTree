namespace System.Collections.Generic;

public class BinaryTreeNode<T> : IEnumerable<T>
    where T : IComparable<T>
{
    public BinaryTreeNode<T>? Parent { get; private set; }
    public BinaryTreeNode<T>? Left { get; private set; }
    public BinaryTreeNode<T>? Right { get; private set; }

    public T Value;

    public BinaryTreeNode(BinaryTreeNode<T>? left, BinaryTreeNode<T>? right, T current)
    {
        Left = left;
        Right = right;
        Value = current;
    }

    public BinaryTreeNode(T value)
    {
        Value = value;
    }

    public void SetLeft(BinaryTreeNode<T>? left)
    {
        if (left == null)
        {
            DetachRight();
            return;
        }

        if (left.Parent == this)
            return;
        if (left.Parent != null)
            throw new InvalidOperationException();
        if (left == this)
            throw new InvalidOperationException("你不能当你自己的儿子");
        left.Parent = this;
        Left = left;
    }

    public void SetRight(BinaryTreeNode<T>? right)
    {
        if (right == null)
        {
            DetachRight();
            return;
        }

        if (right.Parent == this)
            return;
        if (right.Parent != null)
            throw new InvalidOperationException();
        if (right == this)
            throw new InvalidOperationException("你不能当你自己的儿子");

        right.Parent = this;
        Right = right;
    }

    public BinaryTreeNode<T> GetMinNote()
    {
        BinaryTreeNode<T> result = this;
        while (result.Left != null)
            result = result.Left;
        return result;
    }

    public BinaryTreeNode<T> GetMaxNote()
    {
        BinaryTreeNode<T> result = this;
        while (result.Right != null)
            result = result.Right;
        return result;
    }

    public void DetachLeft()
    {
        if (Left == null)
            return;
        Left.Parent = null;
        Left = null;
    }

    public void DetachRight()
    {
        if (Right == null)
            return;
        Right.Parent = null;
        Right = null;
    }

    public BinaryTreeNode<T> RotateLeft()
    {
        if (Parent != null)
            throw new InvalidOperationException("不能直接旋转非根节点, 先脱离, 再旋转, 然后重新附加");
        if (Right == null)
            throw new InvalidOperationException("没有右节点");

        BinaryTreeNode<T> oldRoot = this;
        BinaryTreeNode<T> newRoot = Right;
        BinaryTreeNode<T>? move = newRoot.Left;
        oldRoot.DetachRight();
        newRoot.DetachLeft();
        oldRoot.SetRight(move);
        newRoot.SetLeft(oldRoot);

        return newRoot;
    }

    public BinaryTreeNode<T> RotateRight()
    {
        if (Parent != null)
            throw new InvalidOperationException("小逼崽子, 你不能直接旋转非根节点, 先脱离, 再旋转, 然后重新附加");
        if (Left == null)
            throw new InvalidOperationException("小逼崽子, 没有左节点");

        BinaryTreeNode<T> oldRoot = this;
        BinaryTreeNode<T> newRoot = Left;
        BinaryTreeNode<T>? move = newRoot.Right;
        oldRoot.DetachLeft();
        newRoot.DetachRight();
        oldRoot.SetLeft(move);
        newRoot.SetRight(oldRoot);

        return newRoot;
    }

    public int GetWeight()
    {
        int result = 1;
        if (Left != null)
            result += Left.GetWeight();
        if (Right != null)
            result += Right.GetWeight();
        return result;
    }

    public int GetHeight()
    {
        int subHeight = 0;
        if (Left != null)
            subHeight = Left.GetHeight();
        if (Right != null)
            subHeight = Math.Max(subHeight, Right.GetHeight());
        return subHeight + 1;
    }

    public void Insert(T value)
    {
        if (Value.CompareTo(value) > 0)
        {
            if (Left == null)
                SetLeft(new BinaryTreeNode<T>(value));
            else
                Left.Insert(value);
        }
        else
        {
            if (Right == null)
                SetRight(new BinaryTreeNode<T>(value));
            else
                Right.Insert(value);
        }
    }

    public bool Remove(T value)
    {
        if (Value.CompareTo(value) > 0)
        {
            if (Left != null)
            {
                if (!Left.Value.Equals(value))
                    return Left.Contains(value);

                return true;
            }
        }
        else
        {
            if (Right != null)
            {
                if (!Right.Value.Equals(value))
                    return Right.Contains(value);

                return true;
            }
        }

        return false;
    }

    public bool Contains(T value)
    {
        if (Value.CompareTo(value) > 0)
        {
            if (Left != null)
            {
                if (!Left.Value.Equals(value))
                    return Left.Contains(value);

                return true;
            }
        }
        else
        {
            if (Right != null)
            {
                if (!Right.Value.Equals(value))
                    return Right.Contains(value);

                return true;
            }
        }

        return false;
    }

    public static BinaryTreeNode<T> Build(IEnumerable<T> values)
    {
        IEnumerator<T> enumerator = values.GetEnumerator();
        if (!enumerator.MoveNext())
            throw new InvalidOperationException("There is no element");
        BinaryTreeNode<T> root = new BinaryTreeNode<T>(enumerator.Current);

        while (enumerator.MoveNext())
            root.Insert(enumerator.Current);

        return root;
    }

    public BinaryTreeNode<T> MakeBalanced()
    {
        if (Left != null)
        {
            BinaryTreeNode<T> left = Left;
            DetachLeft();

            SetLeft(left.MakeBalanced());
        }

        if (Right != null)
        {
            BinaryTreeNode<T> right = Right;
            DetachRight();

            SetRight(right.MakeBalanced());
        }

        int leftHeight = Left?.GetHeight() ?? 0;
        int rightHeight = Right?.GetHeight() ?? 0;

        if (leftHeight - rightHeight > 1)
            return RotateRight();
        else if (rightHeight - leftHeight > 1)
            return RotateLeft();
        else
            return this;
    }

    private IEnumerable<string> VisualizeInternal()
    {
        IEnumerable<string> SubNode(IEnumerable<string> node)
        {
            IEnumerator<string> enumerator = node.GetEnumerator();
            if (!enumerator.MoveNext())
                throw new InvalidOperationException();
            yield return $"|-{enumerator.Current}";

            while (enumerator.MoveNext())
                yield return $"| {enumerator.Current}";
        }

        yield return Value?.ToString() ?? "null";

        if (Left != null)
        {
            foreach (string line in SubNode(Left.VisualizeInternal()))
                yield return line;
        }
        if (Right != null)
            foreach (string line in SubNode(Right.VisualizeInternal()))
                yield return line;
    }

    public string Visualize()
    {
        return string.Join('\n', VisualizeInternal());
    }

    public IEnumerator<T> GetEnumerator()
    {
        if (Left != null)
            foreach (T left in Left)
                yield return left;

        yield return Value;

        if (Right != null)
            foreach (T right in Right)
                yield return right;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}