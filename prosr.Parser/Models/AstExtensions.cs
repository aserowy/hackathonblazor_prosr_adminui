using System.Collections.Generic;

namespace prosr.Parser.Models
{
    internal static class AstExtensions
    {
        public static void Pack(this Ast ast)
        {
            Package? package = null;

            var nodes = new List<INode>();
            foreach (var node in ast.Nodes)
            {
                if (node is Package pkg)
                {
                    package = pkg;
                    nodes.Add(package);
                }
                else
                {
                    if (package is null)
                    {
                        nodes.Add(node);
                    }
                    else
                    {
                        package.Nodes.Add(node);
                    }
                }
            }

            ast.Nodes = nodes;
        }
    }
}
