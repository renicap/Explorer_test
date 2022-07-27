using System.Text.Json.Nodes;
using System.Windows;

namespace CKRT_Test
{
    public partial class MainWindow : Window
    {
        static void GoingByNodes(JsonNode? jNode, FileNode? fileNode)
        {
            if (jNode == null || fileNode == null) { return; }

            FileNode newNode;
            string type;
            if (jNode is JsonArray)
            {
                JsonArray jArrayNodes = jNode.AsArray();

                // Добавляем родительскую папку
                foreach (var jn in jArrayNodes)
                {
                    type = (string)jn[constType];
                    if (type != constParent && fileNode != rootFileNode)      
                    {
                        newNode = new FileNode(constParent, "..", fileNode);
                        fileNode.NodesAvailable.Add(newNode);
                        break;
                    }
                }

                // Добавить папки
                foreach (var jn in jArrayNodes)
                {
                    type = (string)jn[constType];
                    newNode = new FileNode(type, (string)jn[constName], fileNode);
                    fileNode.NodesAvailable.Add(newNode);
                    if (type == constDdirectory)
                    {
                        JsonNode jNextNode = jn[constContents];
                        GoingByNodes(jNextNode, newNode);
                    }
                }
            }
        }
    }
}