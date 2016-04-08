using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using LedighedsApp.Model.Handler;
using LedighedsApp.ViewModel;

namespace LedighedsApp.View
{
    public sealed partial class InfoCategoryView : Page
    {
        private InformationVm ViewModel;
        public InfoCategoryView()
        {
            this.InitializeComponent();
        }

        private void GoBack_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (((InformationVm)MainGrid.DataContext).SelectedInformation != null)
            {
                ((InformationVm)MainGrid.DataContext).SelectedInformation = null;
                Informations.SelectedItem = null;
                Informations.Visibility = Visibility.Visible;
                Information.Visibility = Visibility.Collapsed;
            } 
            else if (((InformationVm)MainGrid.DataContext).SelectedInfoType != null)
            {
                ((InformationVm)MainGrid.DataContext).SelectedInfoType = null;
                Categories.SelectedItem = null;
                Categories.Visibility = Visibility.Visible;
                Informations.Visibility = Visibility.Collapsed;
            }
            else
            {
                NavigationService.GoBack(); 
            }
            
        }

        private void ChooseCategory(object sender, SelectionChangedEventArgs e)
        {
            Categories.Visibility = Visibility.Collapsed;
            Informations.Visibility = Visibility.Visible;
        }

        private void ChooseInformation(object sender, SelectionChangedEventArgs e)
        {
            Informations.Visibility = Visibility.Collapsed;
            Information.Visibility = Visibility.Visible;
        }
    }
}
