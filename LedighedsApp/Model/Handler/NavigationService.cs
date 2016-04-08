using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using LedighedsApp.Model.DataModel;
using LedighedsApp.Model.DataModel.Enum;
using LedighedsApp.Model.DBMS;
using Microsoft.Xaml.Interactions.Core;

namespace LedighedsApp.Model.Handler
{
    public static class NavigationService
    {
        private static int CurrentPageId { get; set; }
        private static int NextPageId { get; set; }
        private static Page currentPage = (Page)((App)Application.Current).rootFrame.Content;

        public static void NavigateToPage(Type nextPage, object param = null)
        {
            //CurrentPageId = (int)Enum.Parse(typeof(PageName), ReturnPageName(currentPage.GetType()));
            //NextPageId = (int)Enum.Parse(typeof(PageName), ReturnPageName(nextPage.GetType()));

            //Conn.Insert(new NavigationStep(CurrentPageId, NextPageId));

            currentPage.Frame.Navigate(nextPage, param);
        }

        public static void GoBack()
        {
            if (currentPage.Frame.CanGoBack)
            {
                //Last eller first? Mener last burde være korrekt
                var nextPage = currentPage.Frame.BackStack.Last();

                //CurrentPageId = (int)Enum.Parse(typeof(PageName), ReturnPageName(currentPage.GetType()));
                //NextPageId = (int)Enum.Parse(typeof(PageName), ReturnPageName(nextPage.GetType()));

                //Conn.Insert(new NavigationStep(CurrentPageId, NextPageId));

                currentPage.Frame.GoBack();
            }
        }

        private static string ReturnPageName(Type item)
        {
            string[] itemArray = item.ToString().Split('.');
            return itemArray[itemArray.Count() - 1].ToString();
        }
    }
}
