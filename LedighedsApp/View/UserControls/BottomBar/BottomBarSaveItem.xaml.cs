using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using LedighedsApp.Model.Assets;
using LedighedsApp.ViewModel;

namespace LedighedsApp.View.UserControls.BottomBar
{
    public sealed partial class BottomBarSaveItem : UserControl
    {
        private static EventHandler _buttonSaveEvent;
        public static event EventHandler ButtonSaveEvent
        {
            add
            {
                _buttonSaveEvent = null;
                _buttonSaveEvent += value;
            }
            remove
            {
                _buttonSaveEvent -= value;
            }
        }

        public BottomBarSaveItem()
        {
            this.InitializeComponent();
        }

        private void ButtonSave_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_buttonSaveEvent != null)
                _buttonSaveEvent(this, EventArgs.Empty);
        }
    }
}
