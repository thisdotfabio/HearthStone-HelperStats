using HearthStone_HelperStats.Libs;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.System.Profile;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HearthStone_HelperStats
{
    public partial class ExtendedSplashScreen : Page
    {
        internal Rect splashImageRect;
        private SplashScreen splash;
        internal bool dismissed = false;
        internal Frame rootFrame;

        public ExtendedSplashScreen(SplashScreen splashScreen, bool loadState)
        {
            this.InitializeComponent();

            Window.Current.SizeChanged += ExtendedSplashScreen_SizeChanged;

            splash = splashScreen;
            if (splash!=null)
            {
                splash.Dismissed += Splash_Dismissed;

                splashImageRect = splash.ImageLocation;

                PositionImage();
                PositionRing();
            }

            rootFrame = new Frame();

            DoActions();
        }

        private void PositionImage()
        {
            switch (AnalyticsInfo.VersionInfo.DeviceFamily)
            {
                case "Windows.Mobile":
                    extendedSplashImage.Height = 600 / (1240 / Window.Current.Bounds.Width);
                    extendedSplashImage.Width = Window.Current.Bounds.Width;
                    extendedSplashImage.SetValue(Canvas.LeftProperty, 0);
                    extendedSplashImage.SetValue(Canvas.TopProperty, (Window.Current.Bounds.Height / 2) - (extendedSplashImage.Height / 2));

                    break;

                case "Windows.Desktop":
                    extendedSplashImage.SetValue(Canvas.LeftProperty, splashImageRect.X);
                    extendedSplashImage.SetValue(Canvas.TopProperty, splashImageRect.Y);
                    extendedSplashImage.Height = splashImageRect.Height;
                    extendedSplashImage.Width = splashImageRect.Width;

                    break;

                default:
                    // Other things like HoloLens?
                    break;
            }
        }

        void PositionRing()
        {
            splashProgressRing.SetValue(Canvas.LeftProperty, splashImageRect.X + (splashImageRect.Width * 0.5) - (splashProgressRing.Width * 0.5));
            splashProgressRing.SetValue(Canvas.TopProperty, (splashImageRect.Y + splashImageRect.Height + splashImageRect.Height * 0.1));
        }

        private void Splash_Dismissed(SplashScreen sender, object args)
        {
            dismissed = true;
        }

        private void ExtendedSplashScreen_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            if (splash != null)
            {
                splashImageRect = splash.ImageLocation;

                PositionImage();
                PositionRing();
            }
        }

        async void DoActions()
        {
            loadingTextBlock.Text = "Verificando la conexión a inertnet";
            await Task.Delay(1000);
            if (Libs.Network.CheckInternet())
            {
                loadingTextBlock.Text = "Buscando actualizaciones de cartas";
                await Task.Delay(1000);

                JSONCardDownloadStatus checkDownload = JSONCard.UpdateCardList();

                switch (checkDownload)
                {
                    case JSONCardDownloadStatus.NotDownloaded:
                        loadingTextBlock.Text = "No se ha podido actualizar las cartas";
                        break;
                    case JSONCardDownloadStatus.Downloaded:
                        loadingTextBlock.Text = "Cartas actualizadas";
                        break;
                    case JSONCardDownloadStatus.AlreadyUpdated:
                        loadingTextBlock.Text = "No hay nuevas actualizaciones";
                        break;
                }
            }
            else
                loadingTextBlock.Text = "No se ha podido establecer conexión a internet";

            await Task.Delay(1000);

            rootFrame.Navigate(typeof(MainPage));
            Window.Current.Content = rootFrame;
        }
    }
}