using System.Windows;

namespace CKRT_Test
{
    public partial class MainWindow : Window
    {
        void CreateFoldersCollectionFrom(FileNode? sourceNode)
        {
            if (sourceNode == null) { return; }
            FolderNodes.Clear();
            // Сначало папки 
            foreach (var node in sourceNode.NodesAvailable)
            {
                if (node.Type == constDdirectory || node.Type == constParent)
                {
                    FolderNodes.Add(node);
                }
            }
            // Потом файлы
            foreach (var node in sourceNode.NodesAvailable)
            {
                if (node.Type == constFile)
                {
                    FolderNodes.Add(node);
                }
            }
        }
    }
}