using System.Text;

namespace TreeDiag;

public abstract class TreeDiagnosticWriter<T> : TreeEnumerator<T>
{
    private StringBuilder _builder;

    public string Write(T node)
    {
        _builder = new StringBuilder();
        EnumerateRoot(node);
        return _builder.ToString();
    }

    protected override void ProcessNode(T node, int level, int indexForCurrentLevel)
    {
        var indent = new string(' ', level * 2);
        _builder.AppendLine($"{indent}{{{indexForCurrentLevel}}} {Format(node)}");
    }

    protected string GetShortTypeName(T node)
    {
        var type = node.GetType().ToString();
        return type.Substring(type.LastIndexOf('.') + 1);
    }

    protected abstract string Format(T node);
}
