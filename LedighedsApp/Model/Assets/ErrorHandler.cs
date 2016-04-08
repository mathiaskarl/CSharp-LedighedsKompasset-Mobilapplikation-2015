using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Popups;
using LedighedsApp.Common;
using LedighedsApp.Model.Handler;

namespace LedighedsApp.Model.Assets
{
    static class ErrorHandler
    {
        private static IAsyncOperation<IUICommand> asyncCommand = null;

        private static string _returnMessage;

        public static string ReturnErrorMessage(string statusKey, ObservableDictionary dictionary = null)
        {
            dictionary = dictionary ?? App.Content;
            return RetrieveErrorMessage(statusKey, dictionary);
        }

        public async static void ShowError(string errorMessage)
        {
            if (asyncCommand != null)
                return;

            MessageDialog msgbox = new MessageDialog(errorMessage);
            asyncCommand = msgbox.ShowAsync();
            await asyncCommand;
            asyncCommand = null;
        }

        public async static void DialogGoBack(string message)
        {
            if (asyncCommand != null)
                return;

            MessageDialog msgbox = new MessageDialog(message);
            UICommand okBtn = new UICommand("OK");
            okBtn.Invoked = OkBtnClick;
            msgbox.Commands.Add(okBtn);
            asyncCommand = msgbox.ShowAsync();
            await asyncCommand;
            asyncCommand = null;
        }

        private static void OkBtnClick(IUICommand command)
        {
            NavigationService.GoBack();
        }

        private static string RetrieveErrorMessage(string statusKey, ObservableDictionary dictionary)
        {
            if (dictionary.Count < 1)
                return "An unknown error has occurred.";

            object item;
            if (!dictionary.TryGetValue(statusKey, out item))
                return dictionary["ERROR_UNKNOWN"].ToString();

            return dictionary[statusKey].ToString();
        }
    }
}
