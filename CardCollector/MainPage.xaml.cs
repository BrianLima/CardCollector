using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using CardCollector.Resources;
using CardDataBase;
using Microsoft.Phone.Tasks;

namespace CardCollector
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            // Sample code to localize the ApplicationBar
            BuildLocalizedApplicationBar();
            ReloadCards();
        }

        // Sample code for building a localized ApplicationBar
        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton appBarButtonReview = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
            appBarButtonReview.Text = "Classificar";
            appBarButtonReview.Click += appBarButtonReview_Click;
            ApplicationBar.Buttons.Add(appBarButtonReview);

            ApplicationBarIconButton appBarButtonShare = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
            appBarButtonShare.Text = "Compartilhar";
            appBarButtonShare.Click  += appBarButtonShare_Click;
            ApplicationBar.Buttons.Add(appBarButtonShare);

            // Create a new menu item with the localized string from AppResources.
            ApplicationBarMenuItem appBarMenuItemAllCards = new ApplicationBarMenuItem();
            appBarMenuItemAllCards.Text = "todos os cards";
            appBarMenuItemAllCards.Click+=appBarMenuItem_Click;
            ApplicationBar.MenuItems.Add(appBarMenuItemAllCards);

            ApplicationBarMenuItem appBarMenuItemAbout = new ApplicationBarMenuItem();
            appBarMenuItemAbout.Text = "sobre";
            appBarMenuItemAbout.Click += appBarMenuItemAbout_Click;
            ApplicationBar.MenuItems.Add(appBarMenuItemAbout);
        }

        private void appBarButtonShare_Click(object sender, EventArgs e)
        {
            ShareLinkTask share = new ShareLinkTask();
            share.Message = "Estou colecionando cards da Copa do Mundo Fifa 2014 no meu Windows Phone! Baixe o App e ganhe 20 cards você também!";
            share.Title = "Álbum de Cards Copa do Mundo Fifa 2014!";
            share.LinkUri = new Uri("", UriKind.Absolute);

            GenerateRandomCard();
        }

        private void appBarMenuItemAbout_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }

        private void appBarMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AllCards.xaml", UriKind.Relative));
        }

        void appBarButtonReview_Click(object sender, EventArgs e)
        {
            MarketplaceReviewTask task = new MarketplaceReviewTask();
            task.Show();

            GenerateRandomCard();
        }

        private void GenerateRandomCard()
        {
            Random rand = new Random();
            int id = rand.Next(1, 16);

            Cards card = new Cards();
            card = card.getCard(id);
            card.Increase();
        }

        private void ReloadCards()
        {
            Cards cards = new Cards();
            listCards.ItemsSource = cards.getMyCards();
        }

        private void Swap_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            //TODO:Implementar sistema de troca de cards
        }

        private void SwapCard_Click(object sender, RoutedEventArgs e)
        {
            var t = sender as PlayerCardControl;
            if (t != null)
            {
                Cards card = t.DataContext as Cards;
                NavigationService.Navigate(new Uri("/SwapCard.xaml?parameter=" + card.id, UriKind.Relative));
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ReloadCards();
        }
    }
}