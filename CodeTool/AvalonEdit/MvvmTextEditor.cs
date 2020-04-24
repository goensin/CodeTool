using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CodeTool
{
    public class MvvmTextEditor : TextEditor, INotifyPropertyChanged
    {

        public static DependencyProperty TextProperty =
           DependencyProperty.Register("Text", typeof(string), typeof(MvvmTextEditor),
           // binding changed callback: set value of underlying property
           new PropertyMetadata((obj, args) =>
           {
               MvvmTextEditor target = (MvvmTextEditor)obj;
               if (target.baseText != (string)args.NewValue)    //avoid undo stack overflow
                   target.baseText = (string)args.NewValue;
           })
       );

        public new string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        internal string baseText { get { return base.Text; } set { base.Text = value; } }


        public string Syntax
        {
            set { base.SyntaxHighlighting= HighlightingManager.Instance.GetDefinitionByExtension($".{value}"); }
        }

        // Using a DependencyProperty as the backing store for Syntax.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SyntaxProperty =
            DependencyProperty.Register("Syntax", typeof(string), typeof(MvvmTextEditor), 
                new PropertyMetadata((obj, args) =>
            {
                MvvmTextEditor target = (MvvmTextEditor)obj;
                    target.Syntax = (string)args.NewValue;
            }));



        protected override void OnTextChanged(EventArgs e)
        {
            SetCurrentValue(TextProperty, baseText);
            RaisePropertyChanged("Text");

            RaisePropertyChanged("Syntax");

            base.OnTextChanged(e);

        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}
