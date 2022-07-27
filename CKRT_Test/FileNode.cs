using System.Collections.Generic;

namespace CKRT_Test
{
    public class FileNode
    {
        public FileNode(string? type, string? name)
        {
            Type = type;
            Name = name;
            NodesAvailable = new List<FileNode>();
            Parent = null;
        }
        public FileNode(string? type, string? name, FileNode parentNode)
        {
            Type = type;
            Name = name;
            NodesAvailable = new List<FileNode>();
            Parent = parentNode;
        }

        public FileNode(FileNode node)
        {
            Type = node.Type;
            Name = node.Name;
            NodesAvailable = new List<FileNode>(node.NodesAvailable);
            Parent = node.Parent;
        }
        public string? Type { get; set; }
        public string? Name { get; set; }
        public List<FileNode>? NodesAvailable { get; set; }
        public FileNode? Parent { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }

}
