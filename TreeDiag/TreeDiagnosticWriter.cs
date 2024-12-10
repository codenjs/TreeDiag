using System;
using System.Collections.Generic;
using System.Text;

namespace TreeDiag
{
    public abstract class TreeDiagnosticWriter<T>
    {
        public string Write(T node)
        {
            var builder = new StringBuilder();
            AppendNodeWithChildren(builder, node, 0);
            return builder.ToString();
        }

        private void AppendNodeWithChildren(StringBuilder builder, T node, int level)
        {
            var indent = new string(' ', level * 2);
            builder.AppendLine(indent + Format(node));

            foreach (var child in GetChildren(node))
            {
                AppendNodeWithChildren(builder, child, level + 1);
            }
        }

        protected string GetShortTypeName(T node)
        {
            var type = node.GetType().ToString();
            return type.Substring(type.LastIndexOf('.') + 1);
        }

        protected abstract string Format(T node);
        protected abstract IEnumerable<T> GetChildren(T node);
    }
}
