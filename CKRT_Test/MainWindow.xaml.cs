using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CKRT_Test
{
    public partial class MainWindow : Window
    {
        public string? JsonFile { get; set; }
        static FileNode? rootFileNode;
        internal FolderCollection FolderNodes;

        const string constDdirectory = "directory";
        const string constType = "type";
        const string constName = "name";
        const string constContents = "contents";
        const string constParent = "parent";
        const string constFile = "file";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_JsonFile_KeyUp(object sender, KeyEventArgs e)
        {
            if (e != null && e.Source is TextBox && sender is TextBox)
            {
                if (e.KeyboardDevice.Modifiers == ModifierKeys.None && e.Key == Key.Enter)
                {
                    if (FolderNodes == null) return;
                    try
                    {
                        string jsonString = File.ReadAllText(TextBox_JsonFile.Text);
                        JsonNode jRootNode = JsonNode.Parse(jsonString)!;
                        if (jRootNode != null && jRootNode is JsonArray)
                        {
                            JsonArray jArrayNodes = jRootNode.AsArray();
                            JsonObject jObjectNodes = jArrayNodes[0].AsObject();

                            string? type = (string)jObjectNodes[constType]!;
                            string? name = (string)jObjectNodes[constName]!;

                            if (type == constDdirectory)
                            {
                                JsonNode jNextNode = jObjectNodes[constContents];
                                rootFileNode = new FileNode(type, name);
                                GoingByNodes(jNextNode, rootFileNode);

                                // Заполнить начальную коллекцию папок
                                CreateFoldersCollectionFrom(rootFileNode);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageWindow messageWindow = new MessageWindow(ex.Message)
                        {
                            Title = "Чтение Json-файла"
                        };
                        messageWindow.ShowDialog();
                    }
                    e.Handled = true;
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Получить ссылку на источник привязки ListBox_Folders
            BindingExpression be = ListBox_Folders.GetBindingExpression(ItemsControl.ItemsSourceProperty);
            if (be != null)
            {
                FolderNodes = (FolderCollection)be.DataItem;
            }
            TextBox_JsonFile.Focus();
            //TextBox_JsonFile.Text = "C:\\Users\\root\\Documents\\dumpster.json";
        }

        private void ListBox_Folders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int iSelected = ListBox_Folders.SelectedIndex;

            if (iSelected < 0 || FolderNodes == null) { return; }

            // Получить данные из списка узла
            if (FolderNodes.Count <= iSelected) { return; }
            FileNode selectedNode = FolderNodes[iSelected];

            // Движение вверх
            if (selectedNode.Type == constParent)
            {
                FileNode? fileNode = (selectedNode.Parent == null) ? rootFileNode : selectedNode.Parent.Parent;
                CreateFoldersCollectionFrom(fileNode);
            }
            else if (selectedNode.Type == constDdirectory)
            {
                // Движение вниз
                CreateFoldersCollectionFrom(selectedNode);
            }
            else if (selectedNode.Type == constFile)
            {
                MessageWindow messageWindow = new MessageWindow(selectedNode.ToString())
                {
                    Title = "Выбран файл"
                };
                messageWindow.ShowDialog();
            }
        }

        private void ListBox_Folders_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Source is ListBox && sender is ListBox)
            {
                if (e.KeyboardDevice.Modifiers == ModifierKeys.None && e.Key == Key.Enter)  
                {
                    ListBox_Folders_MouseDoubleClick(sender, null);
                }
            }
        }
    }
}
