using HearthStone_HelperStats.Libs;
using System.Collections.Generic;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

// La plantilla de elemento Página en blanco está documentada en http://go.microsoft.com/fwlink/?LinkId=234238

namespace HearthStone_HelperStats.CollectionUI
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class CollectionPage : Page
    {
        List<CollectibleCard> _cards;
        public CollectionPage()
        {
            this.InitializeComponent();
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += (sender, e) =>
            {
                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                    e.Handled = true;
                }
            };


            //carga el array de cartas json
            string path = ApplicationData.Current.LocalFolder.Path;

            _cards = JSONHelper.JsonDeserialize<List<CollectibleCard>>(path + "\\cards.collectible.json");

            CardsListView.ItemsSource = _cards;
        }
    }
}
