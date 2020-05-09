using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CodeTool.ViewModel
{
    public class AddInfoViewModel : ViewModelBase
    {
        public AddInfoViewModel(string title, bool isFile)
        {
            Title = title;
            ExtensionVisibility = isFile ? Visibility.Visible : Visibility.Collapsed;
            ExtensionList = new List<string>
            {
                "cs","xaml","xml","h","cpp","c","jsp","json",".aspx",".asp","php","css", "py", "go", "java" ,"lua"
            };
        }

        public Visibility ExtensionVisibility { get; set; }
        public string Title { get; set; }
        public List<string> ExtensionList { get; set; }

        private string extension;
        public string Extension
        {
            get => extension;
            set { Set(ref extension, value); }
        }

        private string fileName;
        public string FileName
        {
            get => fileName;
            set { Set(ref fileName, value); }
        }


        private RelayCommand cancel;
        public RelayCommand Cancel => cancel ?? (cancel = new RelayCommand(CancelExecute));

        private void CancelExecute()
        {
            DialogHost.CloseDialogCommand.Execute($".", null);
        }


        private RelayCommand confirm;
        public RelayCommand Confirm => confirm ?? (confirm = new RelayCommand(ConfirmExecute));

        private void ConfirmExecute()
        {
            DialogHost.CloseDialogCommand.Execute($"{FileName}|{Extension}", null);
        }
    }
}
