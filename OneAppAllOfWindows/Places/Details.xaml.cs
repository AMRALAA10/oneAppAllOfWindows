using OneAppAllOfWindows.Common;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

using Windows.UI.StartScreen;

namespace OneAppAllOfWindows.Places
{


    public sealed partial class Details : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public Details()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            Application.Current.Suspending += Current_Suspending;

            ToggleAppBarButton(!SecondaryTile.Exists("AMR"));
            this.PinUnPinCommandButton.Click += PinUnPinCommandButton_Click;


        }


        /*string displayName = "Secondary tile pinned through app bar";
        string tileActivationArguments = MainPage.appbarTileId + " was pinned at=" + DateTime.Now.ToLocalTime().ToString();
        Uri square150x150Logo = new Windows.Foundation.Uri("ms-appx:///Assets/square150x150Tile-sdk.png");
        TileSize newTileDesiredSize = TileSize.Square150x150;
        */


        private async void PinUnPinCommandButton_Click(object sender, RoutedEventArgs e)
        {

            this.SecondaryTileCommandBar.IsSticky = true;
            if (SecondaryTile.Exists("Amr"))
            {
                //UnPin
                SecondaryTile secTile = new SecondaryTile("Amr");
                Windows.Foundation.Rect rect = new Rect(20, 20, 2000, 2000);
                var placment = Windows.UI.Popups.Placement.Above;
                bool IsUnPinned = await secTile.RequestDeleteForSelectionAsync(rect, placment);
                ToggleAppBarButton(IsUnPinned);
            }
            else
            {
                //Pin
                SecondaryTile secTile = new SecondaryTile("Amr", "Amr Alaa", "AAA", new Uri("ms-appx:///Assets/Wide310x150Logo.scale-100.png"), TileSize.Square150x150);
                secTile.VisualElements.Square30x30Logo = new Uri("ms-appx:///Assets/Wide310x150Logo.scale-100.png");
                secTile.VisualElements.ShowNameOnSquare150x150Logo = true;
                secTile.VisualElements.ShowNameOnSquare310x310Logo = true;

                secTile.VisualElements.ForegroundText = ForegroundText.Dark;
                
                Windows.Foundation.Rect rect = new Rect(20, 20, 2000, 2000);
                var placment = Windows.UI.Popups.Placement.Above;
                bool isPinned = await secTile.RequestCreateForSelectionAsync(rect,placment);
                ToggleAppBarButton(isPinned);

            }

            this.BottomAppBar.IsSticky = false;
            
        }

        private void Current_Suspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer f = Windows.Storage.ApplicationData.Current.LocalSettings;
            f.Values[PassedImage.Name] = textBoxToSave.Text;


        }

        
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {

            Windows.Storage.ApplicationDataContainer f = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (f.Values[PassedImage.Name] == null) return;
            textBoxToSave.Text = 
                f.Values[PassedImage.Name] as string;
        }

        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer f = Windows.Storage.ApplicationData.Current.LocalSettings;
            f.Values[PassedImage.Name] = PassedImage.Source;
        }

        #region NavigationHelper registration

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            PassedImage.Source = e.Parameter as BitmapImage;
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void textBoxToSave_TextChanged(object sender, TextChangedEventArgs e)
        {
            imageNameTextBlock.Text = textBoxToSave.Text;
        }


        private void ToggleAppBarButton(bool showPinButton)
        {
            if (showPinButton)
            {
                this.PinUnPinCommandButton.Label = "Pin to Start";
                this.PinUnPinCommandButton.Icon = new SymbolIcon(Symbol.Pin);
            }

            else
            {
                this.PinUnPinCommandButton.Label = "Unpin From Start";
                this.PinUnPinCommandButton.Icon = new SymbolIcon(Symbol.UnPin);
            }

            this.PinUnPinCommandButton.UpdateLayout();

        }


    }
}
