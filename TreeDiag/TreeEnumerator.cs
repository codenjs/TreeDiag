using System.Collections.Generic;

namespace TreeDiag;

public abstract class TreeEnumerator<T>
{
    protected void EnumerateRoot(T node)
    {
        EnumerateNodeWithChildren(node, 0, 0);
    }

    private void EnumerateNodeWithChildren(T node, int level, int indexForCurrentLevel)
    {
        ProcessNode(node, level, indexForCurrentLevel);

        var indexForChildren = 0;
        foreach (var child in GetChildren(node))
        {
            EnumerateNodeWithChildren(child, level + 1, indexForChildren++);
        }
    }

    protected abstract void ProcessNode(T node, int level, int indexForCurrentLevel);
    protected abstract IEnumerable<T> GetChildren(T node);
}
