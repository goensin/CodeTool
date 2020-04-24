using CodeTool.Common;
using CodeTool.Dialog;
using CodeTool.Model;
using Dragablz;
using Dragablz.Dockablz;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CodeTool.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IMyDialogService myDialogService)
        {
            dialogService = myDialogService;

            //TreeNodes = new ObservableCollection<TreeNodeInfo>
            //{
            //    new TreeNodeInfo
            //    {
            //        Name="WPF UI",
            //        IconKind=PackIconKind.Folder,
            //        Foreground = new SolidColorBrush(Color.FromRgb(0xf9,0xa8,0x25)),
            //        Content="folder",
            //        Remarks="",
            //        Childs=new ObservableCollection<TreeNodeInfo>
            //        {
            //            new TreeNodeInfo
            //            {
            //                Name="Material Design",
            //                IconKind=PackIconKind.Code,
            //                Foreground = new SolidColorBrush(Color.FromRgb(0x02,0x88,0xd1)),
            //                Content=" <ResourceDictionary.MergedDictionaries>"+
            //                        "<ResourceDictionary Source='pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Light.xaml'/>"+
            //                        "<ResourceDictionary Source='pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Defaults.xaml'/>"+
            //                        "<ResourceDictionary Source='pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.indigo.xaml'/>"+
            //                        "<ResourceDictionary Source='pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.pink.xaml'/>"+
            //                        "<ResourceDictionary Source='pack://application:,,,/Dragablz;component/Themes/materialdesign.xaml' />"+
            //                        "</ResourceDictionary.MergedDictionaries>",
            //                Remarks="备注"
            //            }
            //        }
            //    },
            //    new TreeNodeInfo
            //    {
            //        Name="Common",
            //        IconKind=PackIconKind.Folder,
            //        Foreground = new SolidColorBrush(Color.FromRgb(0xf9,0xa8,0x25)),
            //        Content="Common",
            //        Remarks="",
            //        Childs=new ObservableCollection<TreeNodeInfo>
            //        {
            //        }
            //    },
            //    new TreeNodeInfo
            //    {
            //        Name="Winform",
            //        IconKind=PackIconKind.Folder,
            //        Foreground = new SolidColorBrush(Color.FromRgb(0xf9,0xa8,0x25)),
            //        Content="winform",
            //        Remarks="",
            //        Childs=new ObservableCollection<TreeNodeInfo>
            //        {
            //        }
            //    }
            //};
            TreeNodes = new ObservableCollection<TreeNodeInfo>();
            CurrentNodes = new ObservableCollection<TreeNodeInfo>();

            TreeNodes.Add(new TreeNodeInfo      //添加根文件夹
            {
                Name = "code",
                IconKind = PackIconKind.MicrosoftVisualStudio,
                Foreground = new SolidColorBrush(Color.FromRgb(0x39, 0x49, 0xab)),
                Content = "folder",
                Remarks = "",
                Childs = new ObservableCollection<TreeNodeInfo>(),
                FilePath = codeFolder
            });
            LoadTreeNodes(codeFolder, TreeNodes[0].Childs);

            //CurrentNodes = new ObservableCollection<TreeNodeInfo>(TreeNodes);

            if (!IsInDesignModeStatic)
            {
                App.Current.MainWindow.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
                App.Current.MainWindow.MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            }


            MaxVisibility = Visibility.Visible;
            RestoreVisibility = Visibility.Collapsed;
        }



        #region 字段、属性
        const string identifier = "RootDialog";
        IMyDialogService dialogService;
        string codeFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "code");

        private Visibility maxVisibility;
        public Visibility MaxVisibility
        {
            get => maxVisibility;
            set { Set(ref maxVisibility, value); }
        }

        private Visibility restoreVisibility;
        public Visibility RestoreVisibility
        {
            get => restoreVisibility;
            set { Set(ref restoreVisibility, value); }
        }

        private ObservableCollection<TreeNodeInfo> treeNodes;
        public ObservableCollection<TreeNodeInfo> TreeNodes
        {
            get => treeNodes;
            set { Set(ref treeNodes, value); }
        }

        private TreeNodeInfo selectedNode;
        public TreeNodeInfo SelectedNode
        {
            get => selectedNode;
            set { Set(ref selectedNode, value); }
        }

        private ObservableCollection<TreeNodeInfo> currentNodes;
        public ObservableCollection<TreeNodeInfo> CurrentNodes
        {
            get => currentNodes;
            set { Set(ref currentNodes, value); }
        }

        private int tabControlIndex;
        public int TabControlIndex
        {
            get => tabControlIndex;
            set { Set(ref tabControlIndex, value); }
        }

        private Tuple<TreeNodeInfo, bool> tempNode;
        public Tuple<TreeNodeInfo, bool> TempNode
        {
            get => tempNode;
            set { Set(ref tempNode, value); }
        }

        #endregion


        #region 命令

        #region 界面操作
        private RelayCommand close;
        public RelayCommand Close => close ?? (close = new RelayCommand(() =>
        {
            App.Current.MainWindow.Close();
        }));

        private RelayCommand restore;
        public RelayCommand Restore => restore ?? (restore = new RelayCommand(() =>
        {
            App.Current.MainWindow.WindowState = WindowState.Normal;
            MaxVisibility = Visibility.Visible;
            RestoreVisibility = Visibility.Collapsed;
        }));

        private RelayCommand maximize;
        public RelayCommand Maximize => maximize ?? (maximize = new RelayCommand(() =>
        {
            App.Current.MainWindow.WindowState = WindowState.Maximized;
            MaxVisibility = Visibility.Collapsed;
            RestoreVisibility = Visibility.Visible;
        }));

        private RelayCommand minimize;
        public RelayCommand Minimize => minimize ?? (minimize = new RelayCommand(() =>
        {
            App.Current.MainWindow.WindowState = WindowState.Minimized;
        }));

        private RelayCommand dragMove;
        public RelayCommand DragMove => dragMove ?? (dragMove = new RelayCommand(() =>
        {
            App.Current.MainWindow.DragMove();
        }));

        private RelayCommand transformWindowsStatus;
        public RelayCommand TransformWindowsStatus => transformWindowsStatus ?? (transformWindowsStatus = new RelayCommand(() =>
        {
            if (App.Current.MainWindow.WindowState == WindowState.Normal)
            {
                App.Current.MainWindow.WindowState = WindowState.Maximized;
                MaxVisibility = Visibility.Collapsed;
                RestoreVisibility = Visibility.Visible;
            }
            else
            {
                App.Current.MainWindow.WindowState = WindowState.Normal;
                MaxVisibility = Visibility.Visible;
                RestoreVisibility = Visibility.Collapsed;
            }
        }));
        #endregion

        //界面加载完成
        private RelayCommand<TreeView> loaded;
        public RelayCommand<TreeView> Loaded => loaded ?? (loaded = new RelayCommand<TreeView>((treeView) =>
        {
            TreeViewItem treeViewItem = GetTreeViewItem(treeView, treeView.Items[0]);   //默认展开根节点
            treeViewItem.IsExpanded = true;
        }));

        private RelayCommand<MouseButtonEventArgs> treeViewMouseRightDown;
        public RelayCommand<MouseButtonEventArgs> TreeViewMouseRightDown => treeViewMouseRightDown ?? (treeViewMouseRightDown = new RelayCommand<MouseButtonEventArgs>(TreeViewMouseRightDownExecute));
        // 鼠标右键选中treeview item
        private void TreeViewMouseRightDownExecute(MouseButtonEventArgs e)
        {
            TreeView treeView = e.Source as TreeView;
            TreeNodeInfo nodeInfo = ((FrameworkElement)e.OriginalSource).DataContext as TreeNodeInfo;
            if (treeView != null && nodeInfo != null)   // 选中 节点
            {
                TreeViewItem treeViewItem = GetTreeViewItem(treeView, nodeInfo);

                if (treeViewItem != null)
                {
                    treeViewItem.IsSelected = true;
                }
            }

            //if (treeView != null && nodeInfo == null)   // 未选中 节点
            //{
            //    TreeNodeInfo selectedItem = treeView.SelectedItem as TreeNodeInfo;
            //    if (selectedItem != null)
            //    {
            //        TreeViewItem treeViewItem = GetTreeViewItem(treeView, nodeInfo);
            //       // TreeViewItem treeViewItem = treeView.ItemContainerGenerator.ContainerFromItem(selectedItem) as TreeViewItem;
            //        if (treeViewItem != null)
            //        {
            //            treeViewItem.IsSelected = false;
            //        }
            //    }
            //}
        }

        private RelayCommand<TreeView> addFile;
        public RelayCommand<TreeView> AddFile => addFile ?? (addFile = new RelayCommand<TreeView>(AddFileExecute));

        // 添加文件
        private async void AddFileExecute(TreeView treeView)
        {
            try
            {
                if (treeView != null)
                {
                    TreeNodeInfo nodeInfo = treeView.SelectedItem as TreeNodeInfo;
                    string folderPath = ""; //添加的文件夹路径

                    if (nodeInfo != null)   //选中节点
                    {
                        if (nodeInfo.IconKind != PackIconKind.Code)   //选非代码文件
                        {
                            folderPath = nodeInfo.FilePath;

                        }
                        else     //选中文件，文件内无法添加文件
                        {
                            return;
                        }
                    }
                    else
                    {
                        folderPath = codeFolder; //选中空白，添加到根目录
                    }



                    string[] result = (await DialogHost.Show(new AddInfo { DataContext = new AddInfoViewModel("添加文件", true) }, identifier)).ToString().Split('.');
                    string fileName = result[0];
                    string extension = string.IsNullOrEmpty(result[1]) ? "cs" : result[1];

                    if (File.Exists(Path.Combine(folderPath, fileName + ".xml")))
                    {
                        await dialogService.ShowMessage("改文件名已存在！", identifier);
                        return;
                    }

                    if (fileName != "")
                    {
                        var newNode = new TreeNodeInfo
                        {
                            Name = fileName,
                            IconKind = PackIconKind.Code,
                            Foreground = new SolidColorBrush(Color.FromRgb(0x02, 0x88, 0xd1)),
                            Content = "",
                            Remarks = "",
                            FilePath = Path.Combine(folderPath, fileName + ".xml"),
                            Childs = new ObservableCollection<TreeNodeInfo>(),
                            Syntax = extension
                        };


                        if (XmlHelper.Save2File(newNode.FilePath, newNode))
                        {
                            if (nodeInfo != null)
                            {
                                nodeInfo.Childs.Add(newNode);   //在已有节点添加
                                GetTreeViewItem(treeView, nodeInfo).IsExpanded = true;   //展开刚刚添加的文件夹
                            }
                            else
                            {
                                TreeNodes.Add(newNode);         //在根添加
                            }

                            CurrentNodes.Add(newNode);
                            TabControlIndex = CurrentNodes.IndexOf(newNode);
                            //TreeNodes[0].Childs.Clear();
                            //LoadTreeNodes(codeFolder, TreeNodes[0].Childs);
                        }
                        else
                        {
                            await dialogService.ShowMessage("添加文件失败！", identifier);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage($"系统错误:{ex.Message}", identifier);
            }
        }

        private RelayCommand<TreeView> addFolder;
        public RelayCommand<TreeView> AddFolder => addFolder ?? (addFolder = new RelayCommand<TreeView>(AddFolderExecute));
        //添加文件夹
        private async void AddFolderExecute(TreeView treeView)
        {
            try
            {
                if (treeView != null)
                {
                    TreeNodeInfo nodeInfo = treeView.SelectedItem as TreeNodeInfo;
                    string folderPath = ""; //添加的文件夹路径

                    if (nodeInfo != null)   //选中节点
                    {
                        if (nodeInfo.IconKind != PackIconKind.Code)   //选非代码文件
                        {
                            folderPath = nodeInfo.FilePath;

                        }
                        else     //选中文件，文件内无法添加文件
                        {
                            return;
                        }
                    }
                    else
                    {
                        folderPath = codeFolder; //选中空白，添加到根目录
                    }

                    string[] result = (await DialogHost.Show(new AddInfo { DataContext = new AddInfoViewModel("添加文件夹", false) }, identifier)).ToString().Split('.');
                    string fileName = result[0];
                    string extension = result[1];

                    if (fileName != "")
                    {
                        if (Directory.Exists(Path.Combine(folderPath, fileName)))
                        {
                            await dialogService.ShowMessage("改文件夹已存在！", identifier);
                            return;
                        }

                        var newNode = new TreeNodeInfo
                        {
                            Name = fileName,
                            IconKind = PackIconKind.Folder,
                            Foreground = new SolidColorBrush(Color.FromRgb(0xf9, 0xa8, 0x25)),
                            Content = "folder",
                            Remarks = "",
                            FilePath = Path.Combine(folderPath, fileName),
                            Childs = new ObservableCollection<TreeNodeInfo>()
                        };

                        Directory.CreateDirectory(newNode.FilePath);

                        if (nodeInfo != null)
                        {
                            nodeInfo.Childs.Add(newNode);   //在已有节点添加
                            GetTreeViewItem(treeView, nodeInfo).IsExpanded = true;   //展开刚刚添加的文件夹
                        }
                        else
                        {
                            TreeNodes.Add(newNode);         //在根添加
                        }

                        //TreeNodes[0].Childs.Clear();
                        //LoadTreeNodes(codeFolder, TreeNodes[0].Childs);
                    }
                }
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage($"系统错误:{ex.Message}", identifier);
            }
        }

        private RelayCommand<TreeView> rename;
        public RelayCommand<TreeView> Rename => rename ?? (rename = new RelayCommand<TreeView>(RenameExecute));
        //重命名
        private void RenameExecute(TreeView treeView)
        {
            //try
            //{
            //    if (treeView != null)
            //    {
            //        TreeNodeInfo nodeInfo = treeView.SelectedItem as TreeNodeInfo;
            //        string dirName = Path.GetDirectoryName(nodeInfo.FilePath);

            //        string newName = (await DialogHost.Show(new ChangeInfo("重命名", nodeInfo.Name), identifier)).ToString();
            //        if (newName != "cancel")
            //        {
            //            if (nodeInfo.IconKind == PackIconKind.Code) //文件
            //            {
            //                if (File.Exists(Path.Combine(dirName, newName + ".xml")))
            //                {
            //                    await dialogService.ShowMessage("改文件夹名称已存在！", identifier);
            //                    return;
            //                }
            //                string newFilePath = Path.Combine(dirName, newName + ".xml");
            //                string oldFilePath = nodeInfo.FilePath;
            //                File.Move(oldFilePath, newFilePath);
            //            }
            //            else    //文件夹
            //            {
            //                if (Directory.Exists(Path.Combine(dirName, newName)))
            //                {
            //                    await dialogService.ShowMessage("改文件夹名称已存在！", identifier);
            //                    return;
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    await dialogService.ShowMessage($"系统错误:{ex.Message}", identifier); ;
            //}

        }

        private RelayCommand<TreeView> delete;
        public RelayCommand<TreeView> Delete => delete ?? (delete = new RelayCommand<TreeView>(DeleteExecute));
        //删除
        private async void DeleteExecute(TreeView treeView)
        {
            try
            {
                if (treeView != null)
                {
                    TreeNodeInfo nodeInfo = treeView.SelectedItem as TreeNodeInfo;


                    if (nodeInfo.IconKind == PackIconKind.Folder) //删除文件夹
                    {
                        if (await dialogService.ShowConfirm($"是否确定删除文件夹'{ nodeInfo.Name}'以及文件夹内所有文件？", identifier))
                        {
                            DeleteAllFileAndFloder(nodeInfo.FilePath);
                            GetParentModels(nodeInfo, TreeNodes).Remove(nodeInfo);      //获取父集合来删除节点
                            DeleteCurrentNodefromFolder(nodeInfo.Childs);      //根据文件夹 删除当前打开的文件，
                        }
                    }
                    else                                        //删除文件
                    {
                        if (await dialogService.ShowConfirm($"是否确定删除文件'{ nodeInfo.Name}'？", identifier))
                        {
                            File.Delete(nodeInfo.FilePath);
                            GetParentModels(nodeInfo, TreeNodes).Remove(nodeInfo);      //获取父集合来删除节点
                            CurrentNodes.Remove(nodeInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage($"系统错误:{ex.Message}", identifier);
            }
        }

        private RelayCommand<TreeView> copy;
        public RelayCommand<TreeView> Copy => copy ?? (copy = new RelayCommand<TreeView>(CopyExecute));
        //复制
        private void CopyExecute(TreeView treeView)
        {
            if (treeView != null)
            {
                TempNode = new Tuple<TreeNodeInfo, bool>((treeView.SelectedItem as TreeNodeInfo), false);
            }
        }

        private RelayCommand<TreeView> cut;
        public RelayCommand<TreeView> Cut => cut ?? (cut = new RelayCommand<TreeView>(CutExecute));
        //剪切
        private void CutExecute(TreeView treeView)
        {
            if (treeView != null)
            {
                TempNode = new Tuple<TreeNodeInfo, bool>((treeView.SelectedItem as TreeNodeInfo), true);
            }
        }

        private RelayCommand<TreeView> paste;
        public RelayCommand<TreeView> Paste => paste ?? (paste = new RelayCommand<TreeView>(PasteExecute));
        //粘贴
        private async void PasteExecute(TreeView treeView)
        {
            try
            {
                if (treeView != null)
                {
                    TreeNodeInfo nodeInfo = treeView.SelectedItem as TreeNodeInfo;
                    string folderPath = ""; //添加的文件夹路径

                    if (nodeInfo != null)   //选中节点
                    {
                        if (nodeInfo.IconKind != PackIconKind.Code)   //选非代码文件
                        {
                            folderPath = nodeInfo.FilePath;

                        }
                        else     //选中文件，文件内无法添加文件
                        {
                            return;
                        }
                    }
                    else
                    {
                        folderPath = codeFolder; //选中空白，添加到根目录
                    }

                    if (TempNode.Item1.IconKind == PackIconKind.Code)       //粘贴文件
                    {
                        string fileName = TempNode.Item1.Name;

                        if (File.Exists(Path.Combine(folderPath, fileName + ".xml")))
                        {
                            await dialogService.ShowMessage("改文件名已存在！", identifier);
                            return;
                        }

                        var newNode = new TreeNodeInfo
                        {
                            Name = TempNode.Item1.Name,
                            IconKind = TempNode.Item1.IconKind,
                            Foreground = TempNode.Item1.Foreground,
                            Content = TempNode.Item1.Content,
                            Remarks = TempNode.Item1.Remarks,
                            FilePath = Path.Combine(folderPath, fileName + ".xml"),
                            Childs = TempNode.Item1.Childs,
                            NeedSave = TempNode.Item1.NeedSave
                        };

                        if (XmlHelper.Save2File(newNode.FilePath, newNode))
                        {
                            if (TempNode.Item2) //剪切
                            {
                                GetParentModels(TempNode.Item1, TreeNodes).Remove(TempNode.Item1);   //移除原有Item
                                CurrentNodes.Remove(TempNode.Item1);        //移除当前打开item
                                File.Delete(TempNode.Item1.FilePath);       //删除文件
                            }

                            if (nodeInfo != null)
                            {
                                nodeInfo.Childs.Add(newNode);   //在已有节点添加
                                GetTreeViewItem(treeView, nodeInfo).IsExpanded = true;   //展开刚刚添加的文件夹
                            }
                            else
                            {
                                TreeNodes.Add(newNode);         //在根添加
                            }
                        }
                        else
                        {
                            await dialogService.ShowMessage("粘贴文件失败！", identifier);
                        }
                    }
                    else  //粘贴文件夹
                    {
                        string dirName = TempNode.Item1.Name;

                        if (Directory.Exists(Path.Combine(folderPath, dirName)))
                        {
                            await dialogService.ShowMessage("改文件夹已存在！", identifier);
                            return;
                        }

                        var newDirNode = new TreeNodeInfo
                        {
                            Name = TempNode.Item1.Name,
                            IconKind = TempNode.Item1.IconKind,
                            Foreground = TempNode.Item1.Foreground,
                            Content = TempNode.Item1.Content,
                            Remarks = TempNode.Item1.Remarks,
                            FilePath = Path.Combine(folderPath, dirName),
                            Childs = TempNode.Item1.Childs,
                            NeedSave = TempNode.Item1.NeedSave
                        };

                        ChangeNodeChildFilePath(newDirNode);         //修改子项的所有文件路径

                        Directory.CreateDirectory(newDirNode.FilePath);    //添加文件夹及内所有文件
                        AddFolderAndFile(newDirNode.Childs, newDirNode.FilePath);

                        if (TempNode.Item2) //剪切
                        {
                            GetParentModels(TempNode.Item1, TreeNodes).Remove(TempNode.Item1);   //移除原有Item
                            DeleteCurrentNodefromFolder(TempNode.Item1.Childs);      //根据文件夹 删除当前打开的文件，
                            DeleteAllFileAndFloder(tempNode.Item1.FilePath);    //递归删除所有文件夹及文件
                        }

                        if (nodeInfo != null)
                        {
                            nodeInfo.Childs.Add(newDirNode);   //在已有节点添加
                            GetTreeViewItem(treeView, nodeInfo).IsExpanded = true;   //展开刚刚添加的文件夹
                        }
                        else
                        {
                            TreeNodes.Add(newDirNode);         //在根添加
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage($"系统错误:{ex.Message}", identifier);
            }
        }

        /// <summary>
        /// 改变文件夹内的  所有文件和文件夹的路径
        /// </summary>
        /// <param name="rootNode"></param>
        private void ChangeNodeChildFilePath(TreeNodeInfo parentNode)
        {
            foreach (var node in parentNode.Childs)
            {
                if (node.IconKind == PackIconKind.Code) //文件
                {
                    node.FilePath = Path.Combine(parentNode.FilePath, node.Name + ".xml");
                }
                else  //文件夹
                {
                    node.FilePath = Path.Combine(parentNode.FilePath, node.Name);
                    ChangeNodeChildFilePath(node);
                }
            }
        }

        private RelayCommand saveFile;
        public RelayCommand SaveFile => saveFile ?? (saveFile = new RelayCommand(SaveFileExecute));
        //保存文件
        private void SaveFileExecute()
        {
            var node = CurrentNodes[TabControlIndex];
            if (XmlHelper.Save2File(node.FilePath, node))
            {
                node.NeedSave = false;
            }
        }

        private RelayCommand saveAllFile;
        public RelayCommand SaveAllFile => saveAllFile ?? (saveAllFile = new RelayCommand(SaveAllFileExecute));
        //保存当前打开全部文件
        private void SaveAllFileExecute()
        {
            foreach (var node in CurrentNodes)
            {
                if (node.NeedSave)
                {
                    if (XmlHelper.Save2File(node.FilePath, node))
                    {
                        node.NeedSave = false;
                    }
                }
            }
        }

        private RelayCommand contentChange;
        public RelayCommand ContentChange => contentChange ?? (contentChange = new RelayCommand(ContentChangeExecute));
        //文件发生内容改变
        private void ContentChangeExecute()
        {
            CurrentNodes[TabControlIndex].NeedSave = true;
        }

        private RelayCommand<TreeView> openTreeViewItem;
        public RelayCommand<TreeView> OpenTreeViewItem => openTreeViewItem ?? (openTreeViewItem = new RelayCommand<TreeView>(OpenTreeViewItemExecute));
        //打开文件
        private void OpenTreeViewItemExecute(TreeView treeView)
        {
            TreeNodeInfo nodeInfo = treeView.SelectedItem as TreeNodeInfo;
            if (nodeInfo != null)
            {
                if (nodeInfo.IconKind == PackIconKind.Code)
                {
                    if (CurrentNodes.Where(n => n.FilePath == nodeInfo.FilePath).Count() == 0)
                    {
                        CurrentNodes.Add(nodeInfo);
                    }
                    TabControlIndex = CurrentNodes.IndexOf(nodeInfo);
                }
            }
        }


        #endregion

        /// <summary>
        /// 递归 根据文件夹 删除当前打开的文件，
        /// </summary>
        /// <param name="childs"></param>
        private void DeleteCurrentNodefromFolder(ObservableCollection<TreeNodeInfo> childs)
        {
            foreach (var node in childs)
            {
                DeleteCurrentNodefromFolder(node.Childs);
                if (CurrentNodes.Contains(node))
                {
                    CurrentNodes.Remove(node);
                }
            }
        }

        /// <summary>
        /// 递归加载所有节点
        /// </summary>
        /// <param name="dir">文件夹</param>
        private void LoadTreeNodes(string dir, ObservableCollection<TreeNodeInfo> nodes)
        {
            string[] directories = Directory.GetDirectories(dir);
            string[] files = Directory.GetFiles(dir).Where(d => Path.GetExtension(d) == ".xml").ToArray();

            //加载文件夹
            for (int j = 0; j < directories.Length; j++)
            {
                TreeNodeInfo dirInfo = new TreeNodeInfo
                {
                    Name = Path.GetFileName(directories[j]),
                    IconKind = PackIconKind.Folder,
                    Foreground = new SolidColorBrush(Color.FromRgb(0xf9, 0xa8, 0x25)),
                    Content = "folder",
                    Remarks = "",
                    Childs = new ObservableCollection<TreeNodeInfo>(),
                    FilePath = directories[j],
                };
                LoadTreeNodes(directories[j], dirInfo.Childs);
                nodes.Add(dirInfo);
            }

            //加载文件
            for (int i = 0; i < files.Length; i++)
            {
                var node = XmlHelper.Load2Object<TreeNodeInfo>(files[i]);
                if (node != null)
                {
                    node.IconKind = PackIconKind.Code;
                    node.Foreground = new SolidColorBrush(Color.FromRgb(0x02, 0x88, 0xd1));
                    nodes.Add(node);
                }
            }
        }

        /// <summary>
        /// 递归添加文件夹和文件
        /// </summary>
        /// <param name="node"></param>
        private void AddFolderAndFile(ObservableCollection<TreeNodeInfo> childs, string dirPath)
        {
            List<TreeNodeInfo> directorieNodes = childs.Where(c => c.IconKind != PackIconKind.Code).ToList();
            List<TreeNodeInfo> fileNodes = childs.Where(c => c.IconKind == PackIconKind.Code).ToList();

            //添加文件夹
            foreach (var node in directorieNodes)
            {
                Directory.CreateDirectory(node.FilePath);
                AddFolderAndFile(node.Childs, node.FilePath);
            }

            //加载文件
            foreach (var node in fileNodes)
            {
                XmlHelper.Save2File(node.FilePath, node);
            }
        }

        /// <summary>
        /// 递归删除所有文件夹及文件
        /// </summary>
        /// <param name="dir"></param>
        private void DeleteAllFileAndFloder(string dirPath)
        {
            string[] files = Directory.GetFiles(dirPath);
            string[] directories = Directory.GetDirectories(dirPath);

            for (int i = 0; i < files.Length; i++)
            {
                File.Delete(files[i]);
            }

            for (int j = 0; j < directories.Length; j++)
            {
                DeleteAllFileAndFloder(directories[j]);
            }
            Directory.Delete(dirPath);
        }

        /// <summary>
        /// 遍历查找父集合
        /// </summary>
        /// <param name="nodeInfo"></param>
        /// <param name="treeNodes"></param>
        /// <returns></returns>
        private ObservableCollection<TreeNodeInfo> GetParentModels(TreeNodeInfo nodeInfo, ObservableCollection<TreeNodeInfo> treeNodes)
        {
            if (treeNodes.Contains(nodeInfo))
            {
                return treeNodes;
            }
            else
            {
                foreach (var node in treeNodes)
                {
                    ObservableCollection<TreeNodeInfo> nodeInfos = GetParentModels(nodeInfo, node.Childs);
                    if (nodeInfos != null)
                    {
                        return nodeInfos;
                    }
                }
                return null;
            }
        }


        #region 微软提供快速定位到TreeViewItem方法
        private TreeViewItem GetTreeViewItem(ItemsControl container, object item)
        {
            if (container != null)
            {
                if (container.DataContext == item)
                {
                    return container as TreeViewItem;
                }

                // Expand the current container
                if (container is TreeViewItem && !((TreeViewItem)container).IsExpanded)
                {
                    container.SetValue(TreeViewItem.IsExpandedProperty, true);
                }

                // Try to generate the ItemsPresenter and the ItemsPanel.
                // by calling ApplyTemplate.  Note that in the
                // virtualizing case even if the item is marked
                // expanded we still need to do this step in order to
                // regenerate the visuals because they may have been virtualized away.

                container.ApplyTemplate();
                ItemsPresenter itemsPresenter =
                    (ItemsPresenter)container.Template.FindName("ItemsHost", container);
                if (itemsPresenter != null)
                {
                    itemsPresenter.ApplyTemplate();
                }
                else
                {
                    // The Tree template has not named the ItemsPresenter,
                    // so walk the descendents and find the child.
                    itemsPresenter = FindVisualChild<ItemsPresenter>(container);
                    if (itemsPresenter == null)
                    {
                        container.UpdateLayout();

                        itemsPresenter = FindVisualChild<ItemsPresenter>(container);
                    }
                }

                Panel itemsHostPanel = (Panel)VisualTreeHelper.GetChild(itemsPresenter, 0);

                // Ensure that the generator for this panel has been created.
                UIElementCollection children = itemsHostPanel.Children;

                MyVirtualizingStackPanel virtualizingPanel =
                    itemsHostPanel as MyVirtualizingStackPanel;

                for (int i = 0, count = container.Items.Count; i < count; i++)
                {
                    TreeViewItem subContainer;
                    if (virtualizingPanel != null)
                    {
                        // Bring the item into view so
                        // that the container will be generated.
                        virtualizingPanel.BringIntoView(i);

                        subContainer =
                            (TreeViewItem)container.ItemContainerGenerator.
                            ContainerFromIndex(i);
                    }
                    else
                    {
                        subContainer =
                            (TreeViewItem)container.ItemContainerGenerator.
                            ContainerFromIndex(i);

                        // Bring the item into view to maintain the
                        // same behavior as with a virtualizing panel.
                        subContainer.BringIntoView();
                    }

                    if (subContainer != null)
                    {
                        // Search the next level for the object.
                        TreeViewItem resultContainer = GetTreeViewItem(subContainer, item);
                        if (resultContainer != null)
                        {
                            return resultContainer;
                        }
                        else
                        {
                            // The object is not under this TreeViewItem
                            // so collapse it.
                            subContainer.IsExpanded = false;
                        }
                    }
                }
            }

            return null;
        }

        private T FindVisualChild<T>(Visual visual) where T : Visual
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(visual); i++)
            {
                Visual child = (Visual)VisualTreeHelper.GetChild(visual, i);
                if (child != null)
                {
                    T correctlyTyped = child as T;
                    if (correctlyTyped != null)
                    {
                        return correctlyTyped;
                    }

                    T descendent = FindVisualChild<T>(child);
                    if (descendent != null)
                    {
                        return descendent;
                    }
                }
            }

            return null;
        }
        #endregion

    }
}