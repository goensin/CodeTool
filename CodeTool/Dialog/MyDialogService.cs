using CodeTool.Dialog;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTool
{
    public class MyDialogService : IMyDialogService
    {
        public async Task<bool> ShowConfirm(string message, string identifier)
        {
            return (bool)await DialogHost.Show(new MyConfirmBox(message), identifier);
        }

        public async Task ShowLoading(string identifier, DialogOpenedEventHandler openedEventHandler)
        {
            await DialogHost.Show(new MyLoading(), identifier, openedEventHandler);
        }

        public async Task ShowMessage(string message, string identifier)
        {
            await DialogHost.Show(new MyMessageBox(message), identifier);
        }
    }
}
