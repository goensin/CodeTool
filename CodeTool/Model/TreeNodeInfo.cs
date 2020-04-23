using GalaSoft.MvvmLight;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;

namespace CodeTool.Model
{
    public class TreeNodeInfo : ObservableObject
    {
        private string name;
        public string Name
        {
            get => name;
            set { Set(ref name, value); }
        }

        private Brush foreground;
        [XmlIgnore]
        public Brush Foreground
        {
            get => foreground;
            set { Set(ref foreground, value); }
        }

        private PackIconKind iconKind;
        [XmlIgnore]
        public PackIconKind IconKind
        {
            get => iconKind;
            set { Set(ref iconKind, value); }
        }

        private string content;
        public string Content
        {
            get => content;
            set { Set(ref content, value); }
        }

        private string remarks;
        public string Remarks
        {
            get => remarks;
            set { Set(ref remarks, value); }
        }

        private ObservableCollection<TreeNodeInfo> childs;
        public ObservableCollection<TreeNodeInfo> Childs
        {
            get => childs;
            set { Set(ref childs, value); }
        }

        private string filePath;
        public string FilePath
        {
            get => filePath;
            set { Set(ref filePath, value); }
        }

        private bool needSave;
        [XmlIgnore]
        public bool NeedSave
        {
            get => needSave;
            set { Set(ref needSave, value); }
        }

    }
}
