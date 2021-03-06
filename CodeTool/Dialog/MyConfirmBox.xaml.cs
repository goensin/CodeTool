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
    /// MyConfirmBox.xaml 的交互逻辑
    /// </summary>
    public partial class MyConfirmBox : UserControl
    {
        public MyConfirmBox(string message)
        {
            InitializeComponent();

            Title = "确认";
            Message = message;
            DataContext = this;
        }

        public string Title { get; set; }
        public string Message { get; set; }
    }
}
