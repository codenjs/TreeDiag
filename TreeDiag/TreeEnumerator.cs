using System.Collections.Generic;

namespace TreeDiag;

public abstract class TreeEnumerator<T>
{
    protected void EnumerateRoot(T node)
    {
        EnumerateNodeWithChildren(node, 0);
    }

    private void EnumerateNodeWithChildren(T node, int level)
    {
        ProcessNode(node, level);

        foreach (var child in GetChildren(node))
        {
            EnumerateNodeWithChildren(child, level + 1);
        }
    }

    protected abstract void ProcessNode(T node, int level);
    protected abstract IEnumerable<T> GetChildren(T node);
}
