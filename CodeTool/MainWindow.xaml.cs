using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace CodeTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //string currentFileName = @"D:\ensin\个人\CodeTool\CodeTool\bin\Debug\code\WPF UI\Material Design.xml";
            //textEditor.Text = "using ICSharpCode.AvalonEdit;\r\nusing System;\r\nusing System.Collections.Generic; ";
            //textEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinitionByExtension(".cs");
            //textEditor.Load(currentFileName);
            //textEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinitionByExtension(Path.GetExtension(currentFileName));
        }
    }
}
