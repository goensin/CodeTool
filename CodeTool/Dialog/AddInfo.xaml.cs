﻿using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace CodeTool.Dialog
{
    /// <summary>
    /// AddInfo.xaml 的交互逻辑
    /// </summary>
    public partial class AddInfo : UserControl
    {
        public AddInfo(string title,string extension)
        {
            InitializeComponent();

            Title = title;
            Extension = extension;
            DataContext = this;
        }

        public string Title { get; set; }
        public string Extension { get; set; }
    }
}