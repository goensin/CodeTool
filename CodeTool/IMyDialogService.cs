using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTool
{
    public interface IMyDialogService
    {
        Task ShowMessage(string message, string identifier);
        Task<bool> ShowConfirm(string message, string identifier);
        Task ShowLoading(string identifier, DialogOpenedEventHandler openedEventHandler);
    }
}
