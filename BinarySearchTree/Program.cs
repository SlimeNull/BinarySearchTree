using System.Diagnostics;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        int[] values = Enumerable
            .Range(0, 30)
            .Select(i => Random.Shared.Next(0, 100))
            .ToArray();

        BinaryTree<int> tree = new BinaryTree<int>();
        tree.InsertRange(values);
        Console.WriteLine(tree.Visualize());

        tree.MakeBalanced();
        Console.WriteLine(tree.Visualize());
    }
}