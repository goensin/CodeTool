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
            //                Remarks="��ע"
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

            TreeNodes.Add(new TreeNodeInfo      //��Ӹ��ļ���
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



        #region �ֶΡ�����
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


        #region ����

        #region �������
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

        //����������
        private RelayCommand<TreeView> loaded;
        public RelayCommand<TreeView> Loaded => loaded ?? (loaded = new RelayCommand<TreeView>((treeView) =>
        {
            TreeViewItem treeViewItem = GetTreeViewItem(treeView, treeView.Items[0]);   //Ĭ��չ�����ڵ�
            treeViewItem.IsExpanded = true;
        }));

        private RelayCommand<MouseButtonEventArgs> treeViewMouseRightDown;
        public RelayCommand<MouseButtonEventArgs> TreeViewMouseRightDown => treeViewMouseRightDown ?? (treeViewMouseRightDown = new RelayCommand<MouseButtonEventArgs>(TreeViewMouseRightDownExecute));
        // ����Ҽ�ѡ��treeview item
        private void TreeViewMouseRightDownExecute(MouseButtonEventArgs e)
        {
            TreeView treeView = e.Source as TreeView;
            TreeNodeInfo nodeInfo = ((FrameworkElement)e.OriginalSource).DataContext as TreeNodeInfo;
            if (treeView != null && nodeInfo != null)   // ѡ�� �ڵ�
            {
                TreeViewItem treeViewItem = GetTreeViewItem(treeView, nodeInfo);

                if (treeViewItem != null)
                {
                    treeViewItem.IsSelected = true;
                }
            }

            //if (treeView != null && nodeInfo == null)   // δѡ�� �ڵ�
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

        // ����ļ�
        private async void AddFileExecute(TreeView treeView)
        {
            try
            {
                if (treeView != null)
                {
                    TreeNodeInfo nodeInfo = treeView.SelectedItem as TreeNodeInfo;
                    string folderPath = ""; //��ӵ��ļ���·��

                    if (nodeInfo != null)   //ѡ�нڵ�
                    {
                        if (nodeInfo.IconKind != PackIconKind.Code)   //ѡ�Ǵ����ļ�
                        {
                            folderPath = nodeInfo.FilePath;

                        }
                        else     //ѡ���ļ����ļ����޷�����ļ�
                        {
                            return;
                        }
                    }
                    else
                    {
                        folderPath = codeFolder; //ѡ�пհף���ӵ���Ŀ¼
                    }



                    string[] result = (await DialogHost.Show(new AddInfo { DataContext = new AddInfoViewModel("����ļ�", true) }, identifier)).ToString().Split('.');
                    string fileName = result[0];
                    string extension = string.IsNullOrEmpty(result[1]) ? "cs" : result[1];

                    if (File.Exists(Path.Combine(folderPath, fileName + ".xml")))
                    {
                        await dialogService.ShowMessage("���ļ����Ѵ��ڣ�", identifier);
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
                                nodeInfo.Childs.Add(newNode);   //�����нڵ����
                                GetTreeViewItem(treeView, nodeInfo).IsExpanded = true;   //չ���ո���ӵ��ļ���
                            }
                            else
                            {
                                TreeNodes.Add(newNode);         //�ڸ����
                            }

                            CurrentNodes.Add(newNode);
                            TabControlIndex = CurrentNodes.IndexOf(newNode);
                            //TreeNodes[0].Childs.Clear();
                            //LoadTreeNodes(codeFolder, TreeNodes[0].Childs);
                        }
                        else
                        {
                            await dialogService.ShowMessage("����ļ�ʧ�ܣ�", identifier);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage($"ϵͳ����:{ex.Message}", identifier);
            }
        }

        private RelayCommand<TreeView> addFolder;
        public RelayCommand<TreeView> AddFolder => addFolder ?? (addFolder = new RelayCommand<TreeView>(AddFolderExecute));
        //����ļ���
        private async void AddFolderExecute(TreeView treeView)
        {
            try
            {
                if (treeView != null)
                {
                    TreeNodeInfo nodeInfo = treeView.SelectedItem as TreeNodeInfo;
                    string folderPath = ""; //��ӵ��ļ���·��

                    if (nodeInfo != null)   //ѡ�нڵ�
                    {
                        if (nodeInfo.IconKind != PackIconKind.Code)   //ѡ�Ǵ����ļ�
                        {
                            folderPath = nodeInfo.FilePath;

                        }
                        else     //ѡ���ļ����ļ����޷�����ļ�
                        {
                            return;
                        }
                    }
                    else
                    {
                        folderPath = codeFolder; //ѡ�пհף���ӵ���Ŀ¼
                    }

                    string[] result = (await DialogHost.Show(new AddInfo { DataContext = new AddInfoViewModel("����ļ���", false) }, identifier)).ToString().Split('.');
                    string fileName = result[0];
                    string extension = result[1];

                    if (fileName != "")
                    {
                        if (Directory.Exists(Path.Combine(folderPath, fileName)))
                        {
                            await dialogService.ShowMessage("���ļ����Ѵ��ڣ�", identifier);
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
                            nodeInfo.Childs.Add(newNode);   //�����нڵ����
                            GetTreeViewItem(treeView, nodeInfo).IsExpanded = true;   //չ���ո���ӵ��ļ���
                        }
                        else
                        {
                            TreeNodes.Add(newNode);         //�ڸ����
                        }

                        //TreeNodes[0].Childs.Clear();
                        //LoadTreeNodes(codeFolder, TreeNodes[0].Childs);
                    }
                }
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage($"ϵͳ����:{ex.Message}", identifier);
            }
        }

        private RelayCommand<TreeView> rename;
        public RelayCommand<TreeView> Rename => rename ?? (rename = new RelayCommand<TreeView>(RenameExecute));
        //������
        private void RenameExecute(TreeView treeView)
        {
            //try
            //{
            //    if (treeView != null)
            //    {
            //        TreeNodeInfo nodeInfo = treeView.SelectedItem as TreeNodeInfo;
            //        string dirName = Path.GetDirectoryName(nodeInfo.FilePath);

            //        string newName = (await DialogHost.Show(new ChangeInfo("������", nodeInfo.Name), identifier)).ToString();
            //        if (newName != "cancel")
            //        {
            //            if (nodeInfo.IconKind == PackIconKind.Code) //�ļ�
            //            {
            //                if (File.Exists(Path.Combine(dirName, newName + ".xml")))
            //                {
            //                    await dialogService.ShowMessage("���ļ��������Ѵ��ڣ�", identifier);
            //                    return;
            //                }
            //                string newFilePath = Path.Combine(dirName, newName + ".xml");
            //                string oldFilePath = nodeInfo.FilePath;
            //                File.Move(oldFilePath, newFilePath);
            //            }
            //            else    //�ļ���
            //            {
            //                if (Directory.Exists(Path.Combine(dirName, newName)))
            //                {
            //                    await dialogService.ShowMessage("���ļ��������Ѵ��ڣ�", identifier);
            //                    return;
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    await dialogService.ShowMessage($"ϵͳ����:{ex.Message}", identifier); ;
            //}

        }

        private RelayCommand<TreeView> delete;
        public RelayCommand<TreeView> Delete => delete ?? (delete = new RelayCommand<TreeView>(DeleteExecute));
        //ɾ��
        private async void DeleteExecute(TreeView treeView)
        {
            try
            {
                if (treeView != null)
                {
                    TreeNodeInfo nodeInfo = treeView.SelectedItem as TreeNodeInfo;


                    if (nodeInfo.IconKind == PackIconKind.Folder) //ɾ���ļ���
                    {
                        if (await dialogService.ShowConfirm($"�Ƿ�ȷ��ɾ���ļ���'{ nodeInfo.Name}'�Լ��ļ����������ļ���", identifier))
                        {
                            DeleteAllFileAndFloder(nodeInfo.FilePath);
                            GetParentModels(nodeInfo, TreeNodes).Remove(nodeInfo);      //��ȡ��������ɾ���ڵ�
                            DeleteCurrentNodefromFolder(nodeInfo.Childs);      //�����ļ��� ɾ����ǰ�򿪵��ļ���
                        }
                    }
                    else                                        //ɾ���ļ�
                    {
                        if (await dialogService.ShowConfirm($"�Ƿ�ȷ��ɾ���ļ�'{ nodeInfo.Name}'��", identifier))
                        {
                            File.Delete(nodeInfo.FilePath);
                            GetParentModels(nodeInfo, TreeNodes).Remove(nodeInfo);      //��ȡ��������ɾ���ڵ�
                            CurrentNodes.Remove(nodeInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage($"ϵͳ����:{ex.Message}", identifier);
            }
        }

        private RelayCommand<TreeView> copy;
        public RelayCommand<TreeView> Copy => copy ?? (copy = new RelayCommand<TreeView>(CopyExecute));
        //����
        private void CopyExecute(TreeView treeView)
        {
            if (treeView != null)
            {
                TempNode = new Tuple<TreeNodeInfo, bool>((treeView.SelectedItem as TreeNodeInfo), false);
            }
        }

        private RelayCommand<TreeView> cut;
        public RelayCommand<TreeView> Cut => cut ?? (cut = new RelayCommand<TreeView>(CutExecute));
        //����
        private void CutExecute(TreeView treeView)
        {
            if (treeView != null)
            {
                TempNode = new Tuple<TreeNodeInfo, bool>((treeView.SelectedItem as TreeNodeInfo), true);
            }
        }

        private RelayCommand<TreeView> paste;
        public RelayCommand<TreeView> Paste => paste ?? (paste = new RelayCommand<TreeView>(PasteExecute));
        //ճ��
        private async void PasteExecute(TreeView treeView)
        {
            try
            {
                if (treeView != null)
                {
                    TreeNodeInfo nodeInfo = treeView.SelectedItem as TreeNodeInfo;
                    string folderPath = ""; //��ӵ��ļ���·��

                    if (nodeInfo != null)   //ѡ�нڵ�
                    {
                        if (nodeInfo.IconKind != PackIconKind.Code)   //ѡ�Ǵ����ļ�
                        {
                            folderPath = nodeInfo.FilePath;

                        }
                        else     //ѡ���ļ����ļ����޷�����ļ�
                        {
                            return;
                        }
                    }
                    else
                    {
                        folderPath = codeFolder; //ѡ�пհף���ӵ���Ŀ¼
                    }

                    if (TempNode.Item1.IconKind == PackIconKind.Code)       //ճ���ļ�
                    {
                        string fileName = TempNode.Item1.Name;

                        if (File.Exists(Path.Combine(folderPath, fileName + ".xml")))
                        {
                            await dialogService.ShowMessage("���ļ����Ѵ��ڣ�", identifier);
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
                            if (TempNode.Item2) //����
                            {
                                GetParentModels(TempNode.Item1, TreeNodes).Remove(TempNode.Item1);   //�Ƴ�ԭ��Item
                                CurrentNodes.Remove(TempNode.Item1);        //�Ƴ���ǰ��item
                                File.Delete(TempNode.Item1.FilePath);       //ɾ���ļ�
                            }

                            if (nodeInfo != null)
                            {
                                nodeInfo.Childs.Add(newNode);   //�����нڵ����
                                GetTreeViewItem(treeView, nodeInfo).IsExpanded = true;   //չ���ո���ӵ��ļ���
                            }
                            else
                            {
                                TreeNodes.Add(newNode);         //�ڸ����
                            }
                        }
                        else
                        {
                            await dialogService.ShowMessage("ճ���ļ�ʧ�ܣ�", identifier);
                        }
                    }
                    else  //ճ���ļ���
                    {
                        string dirName = TempNode.Item1.Name;

                        if (Directory.Exists(Path.Combine(folderPath, dirName)))
                        {
                            await dialogService.ShowMessage("���ļ����Ѵ��ڣ�", identifier);
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

                        ChangeNodeChildFilePath(newDirNode);         //�޸�����������ļ�·��

                        Directory.CreateDirectory(newDirNode.FilePath);    //����ļ��м��������ļ�
                        AddFolderAndFile(newDirNode.Childs, newDirNode.FilePath);

                        if (TempNode.Item2) //����
                        {
                            GetParentModels(TempNode.Item1, TreeNodes).Remove(TempNode.Item1);   //�Ƴ�ԭ��Item
                            DeleteCurrentNodefromFolder(TempNode.Item1.Childs);      //�����ļ��� ɾ����ǰ�򿪵��ļ���
                            DeleteAllFileAndFloder(tempNode.Item1.FilePath);    //�ݹ�ɾ�������ļ��м��ļ�
                        }

                        if (nodeInfo != null)
                        {
                            nodeInfo.Childs.Add(newDirNode);   //�����нڵ����
                            GetTreeViewItem(treeView, nodeInfo).IsExpanded = true;   //չ���ո���ӵ��ļ���
                        }
                        else
                        {
                            TreeNodes.Add(newDirNode);         //�ڸ����
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage($"ϵͳ����:{ex.Message}", identifier);
            }
        }

        /// <summary>
        /// �ı��ļ����ڵ�  �����ļ����ļ��е�·��
        /// </summary>
        /// <param name="rootNode"></param>
        private void ChangeNodeChildFilePath(TreeNodeInfo parentNode)
        {
            foreach (var node in parentNode.Childs)
            {
                if (node.IconKind == PackIconKind.Code) //�ļ�
                {
                    node.FilePath = Path.Combine(parentNode.FilePath, node.Name + ".xml");
                }
                else  //�ļ���
                {
                    node.FilePath = Path.Combine(parentNode.FilePath, node.Name);
                    ChangeNodeChildFilePath(node);
                }
            }
        }

        private RelayCommand saveFile;
        public RelayCommand SaveFile => saveFile ?? (saveFile = new RelayCommand(SaveFileExecute));
        //�����ļ�
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
        //���浱ǰ��ȫ���ļ�
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
        //�ļ��������ݸı�
        private void ContentChangeExecute()
        {
            CurrentNodes[TabControlIndex].NeedSave = true;
        }

        private RelayCommand<TreeView> openTreeViewItem;
        public RelayCommand<TreeView> OpenTreeViewItem => openTreeViewItem ?? (openTreeViewItem = new RelayCommand<TreeView>(OpenTreeViewItemExecute));
        //���ļ�
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
        /// �ݹ� �����ļ��� ɾ����ǰ�򿪵��ļ���
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
        /// �ݹ�������нڵ�
        /// </summary>
        /// <param name="dir">�ļ���</param>
        private void LoadTreeNodes(string dir, ObservableCollection<TreeNodeInfo> nodes)
        {
            string[] directories = Directory.GetDirectories(dir);
            string[] files = Directory.GetFiles(dir).Where(d => Path.GetExtension(d) == ".xml").ToArray();

            //�����ļ���
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

            //�����ļ�
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
        /// �ݹ�����ļ��к��ļ�
        /// </summary>
        /// <param name="node"></param>
        private void AddFolderAndFile(ObservableCollection<TreeNodeInfo> childs, string dirPath)
        {
            List<TreeNodeInfo> directorieNodes = childs.Where(c => c.IconKind != PackIconKind.Code).ToList();
            List<TreeNodeInfo> fileNodes = childs.Where(c => c.IconKind == PackIconKind.Code).ToList();

            //����ļ���
            foreach (var node in directorieNodes)
            {
                Directory.CreateDirectory(node.FilePath);
                AddFolderAndFile(node.Childs, node.FilePath);
            }

            //�����ļ�
            foreach (var node in fileNodes)
            {
                XmlHelper.Save2File(node.FilePath, node);
            }
        }

        /// <summary>
        /// �ݹ�ɾ�������ļ��м��ļ�
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
        /// �������Ҹ�����
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


        #region ΢���ṩ���ٶ�λ��TreeViewItem����
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