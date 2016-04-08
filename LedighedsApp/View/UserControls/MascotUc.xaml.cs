using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LedighedsApp.View.UserControls
{
    public sealed partial class MascotUc : UserControl
    {
        private static EventHandler _mascotTapped;

        public static event EventHandler MascotTapped 
        { 
            add 
            { 
                _mascotTapped = null;
                _mascotTapped = value;
            }
            remove
            {
                _mascotTapped -= value;
            }
        }

        public MascotUc()
        {
            this.InitializeComponent();
        }

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_mascotTapped != null)
            {
                _mascotTapped(this, EventArgs.Empty);
            }
        }
    }
}
