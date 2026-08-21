using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace IL2AVR;

public sealed class Tree<T>(T value, ICollection<Tree<T>>? children = null, IEqualityComparer<T>? comparer = null) : IEnumerable<T>
{
    private interface ITreeEnumeratorKey<K> where K : ITreeEnumeratorKey<K>
    {
        Tree<T>? Tree { get; }
        static abstract K Create(Tree<T>? tree);
    }
    public readonly record struct TreeEnumeratorKey(Tree<T>? Tree) : ITreeEnumeratorKey<TreeEnumeratorKey>
    {
        public static TreeEnumeratorKey Create(Tree<T>? tree) => new(tree);
    }
    private class TreeEnumerator<K>(Tree<T> tree) : IEnumerator<T> where K : ITreeEnumeratorKey<K>
    {
        private readonly HashSet<K> marked = [];
        protected Stack<K>? Stack = null;
        public T Current => Stack!.Peek().Tree!.Value;
        object? IEnumerator.Current => Current;
        public void Dispose()
        {
        }
        protected virtual K Process(K current) => current;
        protected virtual K Create(K? rootTree, Tree<T> childTree) => K.Create(childTree);
        public bool MoveNext()
        {
            if (Stack is null)
            {
                (Stack = new()).Push((K)K.Create(tree));
                return true;
            }
            while (true)
            {
                if (Stack.TryPop(out K? currentTree) && marked.Add(currentTree) && (currentTree = Process(currentTree!)).Tree is not null)
                {
                    foreach (Tree<T> tree in currentTree.Tree.Children)
                        Stack.Push(Create(currentTree, tree));
                    return true;
                }
                return false;
            }
        }
        public void Reset() => Stack = null;
    }
    private readonly struct WhereTreeEnumerable(Tree<T> tree, Func<Tree<T>, bool> predicate) : IEnumerable<T>
    {
        private sealed class WhereTreeEnumerator(Tree<T> tree, Func<Tree<T>, bool> predicate) : TreeEnumerator<TreeEnumeratorKey>(tree)
        {
            protected override TreeEnumeratorKey Process(TreeEnumeratorKey current) => predicate(current.Tree!) ? current : new(null);
        }
        public IEnumerator<T> GetEnumerator() => new WhereTreeEnumerator(tree, predicate);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    private readonly record struct DepthTreeEnumeratorKey(int Depth, Tree<T> Tree) : ITreeEnumeratorKey<DepthTreeEnumeratorKey>
    {
        public static DepthTreeEnumeratorKey Create(Tree<T>? tree) => new(0, tree!);
    }
    private readonly struct DepthTreeEnumerable(Tree<T> tree) : IEnumerable<(int, T)>
    {
        private sealed class DepthTreeEnumerator(Tree<T> tree) : TreeEnumerator<DepthTreeEnumeratorKey>(tree), IEnumerator<(int, T)>
        {
            public new (int, T) Current
            {
                get
                {
                    DepthTreeEnumeratorKey current = Stack!.Peek();
                    return (current.Depth, current.Tree.Value);
                }
            }
            protected override DepthTreeEnumeratorKey Create(DepthTreeEnumeratorKey rootTree, Tree<T> childTree) => new(rootTree.Depth + 1, childTree);
        }
        public IEnumerator<(int, T)> GetEnumerator() => new DepthTreeEnumerator(tree);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    private readonly IEqualityComparer<T> comparer = comparer ?? EqualityComparer<T>.Default;
    public readonly ICollection<Tree<T>> Children = children ?? [];
    public readonly T Value = value;
    public Tree<T>? this[T value] => Find(value);
    public Tree<T>? Find(T value) => Children.FirstOrDefault(child => comparer.Equals(child.Value, value));
    public Tree<T>? Find(Func<T, bool> predicate) => Children.FirstOrDefault(child => predicate(Value));
    public Tree<T>? FindRecursively(Func<T, bool> predicate)
    {
        if (predicate(Value))
            return this;
        Tree<T>? tree = Children.FirstOrDefault(tree => predicate(tree.Value));
        if (tree is null)
            foreach (Tree<T> t in Children)
                if ((tree = t.FindRecursively(predicate)!) is not null)
                    break;
        return tree;
    }
    public Tree<T>? FindRecursively(T value) => FindRecursively(v => comparer.Equals(value, v));
    public IEnumerator<T> GetEnumerator() => new TreeEnumerator<TreeEnumeratorKey>(this);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerable<T> RootWhere(Func<Tree<T>, bool> predicate) => new WhereTreeEnumerable(this, predicate);
    public IEnumerable<(int, T)> WithDepth() => new DepthTreeEnumerable(this);
}