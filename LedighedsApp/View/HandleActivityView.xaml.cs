using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using LedighedsApp.Model.Assets;
using LedighedsApp.Model.DataModel.Activity;
using LedighedsApp.Model.DataModel.Activity.DbSet;
using LedighedsApp.Model.DataModel.Enum;
using LedighedsApp.Model.DataModel.Interface;
using LedighedsApp.Model.Handler;
using LedighedsApp.View.Animation;
using LedighedsApp.View.UserControls;
using LedighedsApp.View.UserControls.BottomBar;
using LedighedsApp.ViewModel;

namespace LedighedsApp.View
{
    public sealed partial class HandleActivityView : Page
    {
        private ActivityVm ViewModel;
        private ActivityHandler handler;

        private UserActivity CurrentUserActivity;
        private Activity CurrentActivityType;
        private DateTime CurrentDate = DateTime.Now;

        public HandleActivityView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter.GetType() == typeof (UserActivity))
            {
                CurrentUserActivity = (UserActivity) e.Parameter;
                CurrentActivityType = CurrentUserActivity.Activity;

            } else if (e.Parameter.GetType() == typeof(List<object>)) { 
                foreach(object obj in (List<object>)e.Parameter)
                    if (obj.GetType() == typeof (Activity))
                        CurrentActivityType = (Activity)obj;
                    else if (obj.GetType() == typeof (DateTime))
                        CurrentDate = (DateTime) obj;

            } else if(e.Parameter.GetType() == typeof(Activity))
                CurrentActivityType = (Activity)e.Parameter;
        }

        private void Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel = ((ActivityVm)(MainGrid.DataContext));
            handler = ViewModel.handler;
            CreateView();
            BottomBarSaveItem.ButtonSaveEvent += new EventHandler(SaveActivity);
        }

        private void SaveActivity(object sender, EventArgs e)
        {
            foreach (var obj in PropertyGrid.Children)
            {
                if (obj.GetType() == typeof (TextBox))
                {
                    TextBox tempItem = (TextBox) obj;
                    if (CurrentActivityType.PropertyValues.ContainsKey(tempItem.Name))
                        CurrentActivityType.PropertyValues[tempItem.Name] = tempItem.Text;
                }
                if (obj.GetType() == typeof(ComboBox))
                {
                    ComboBox tempItem = (ComboBox)obj;
                    if (CurrentActivityType.PropertyValues.ContainsKey(tempItem.Name))
                        if(((ComboboxItem) tempItem.SelectedValue) != null)
                            CurrentActivityType.PropertyValues[tempItem.Name] = ((ActivityPropertyTypeValue)((ComboboxItem)tempItem.SelectedValue).Value).Id.ToString();
                }
            }
            DateTime tempTime = PickDate.Date.Date.Add(PickTime.Time);
            if (CurrentUserActivity != null)
            {
                CurrentUserActivity.Activity = CurrentActivityType;
                CurrentUserActivity.DateTime = tempTime;
                if (!handler.UpdateActivity(CurrentUserActivity))
                    ErrorHandler.ShowError(handler.ErrorMessage);
                else
                    ErrorHandler.DialogGoBack(ViewModel.Content["DIALOG_ACTIVITY_UPDATED"].ToString());
            }
            else
            {
                if (!handler.AddActivity(CurrentActivityType, tempTime))
                    ErrorHandler.ShowError(handler.ErrorMessage);
                else
                    ErrorHandler.DialogGoBack(ViewModel.Content["DIALOG_ACTIVITY_CREATED"].ToString());
            }
        }

        private void CreateView()
        {
            PickDate.Date = (CurrentUserActivity == null ? CurrentDate.Date : CurrentUserActivity.DateTime.Date);
            PickTime.Time = (CurrentUserActivity == null ? CurrentDate.TimeOfDay : CurrentUserActivity.DateTime.TimeOfDay);
            int row = 2;
            foreach (ActivityProperty obj in CurrentActivityType.Properties)
            {
                AddItemToGrid(obj, row);
                row = row+2;
            }
        }

        private void AddItemToGrid(ActivityProperty obj, int row)
        {
            string property = obj.Name;
            string propertyValue = CurrentActivityType.PropertyValues.FirstOrDefault(x => x.Key == obj.Property).Value ?? "";

            PropertyGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            TextBlock TextBlock = new TextBlock();
            TextBlock.Style = (Style)Application.Current.Resources["ActivityPropertyHeader"];
            TextBlock.Text = property + ":";
            TextBlock.SetValue(Grid.RowProperty, row);
            PropertyGrid.Children.Add(TextBlock);

            row++;

            PropertyGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            if (obj.Type == 0 || obj.Type < 2)
            {
                TextBox textBox = new TextBox();
                textBox.Style = (Style)Application.Current.Resources["ActivityPropertyTextBox"];
                textBox.Text = propertyValue;
                textBox.Name = obj.Property;
                textBox.SetValue(Grid.RowProperty, row);
                PropertyGrid.Children.Add(textBox);
            }
            else if (obj.Type == 2)
            {
                ComboBox ComboBox = new ComboBox();
                ComboBox.Style = (Style)Application.Current.Resources["ActivityPropertyComboBox"];
                ComboBox.Name = obj.Property;
                foreach(ActivityPropertyTypeValue objs in obj.StaticPropertyValues)
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = objs.Name;
                    item.Value = objs;
                    ComboBox.Items.Add(item);
                    if (objs.Name == propertyValue)
                        ComboBox.SelectedItem = item;     
                }
                ComboBox.SetValue(Grid.RowProperty, row);
                PropertyGrid.Children.Add(ComboBox);
            }
            Border border = new Border()
            {
                BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)),
                BorderThickness = new Thickness(0, 0, 0, 1),
            };
            border.SetValue(Grid.RowProperty, row);
            PropertyGrid.Children.Add(border);
            
        }

        private void GoBack_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
