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
            AtualizarCards();
        }

        // Sample code for building a localized ApplicationBar
        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
            appBarButton.Text = "Comprar card";
            appBarButton.Click += appBarButton_Click;
            ApplicationBar.Buttons.Add(appBarButton);

            ApplicationBarIconButton appBarButton1 = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
            appBarButton1.Text = "Vender Card";
            appBarButton1.Click += appBarButton_Click;
            ApplicationBar.Buttons.Add(appBarButton1);

            // Create a new menu item with the localized string from AppResources.
            ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
            appBarMenuItem.Text = "todos os cards";
            appBarMenuItem.Click+=appBarMenuItem_Click;
            ApplicationBar.MenuItems.Add(appBarMenuItem);

            ApplicationBarMenuItem appBarMenuItemAbout = new ApplicationBarMenuItem();
            appBarMenuItemAbout.Text = "sobre";
            appBarMenuItemAbout.Click += appBarMenuItemAbout_Click;
            ApplicationBar.MenuItems.Add(appBarMenuItemAbout);
        }

        private void appBarMenuItemAbout_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }

        private void appBarMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AllCards.xaml", UriKind.Relative));
        }

        void appBarButton_Click(object sender, EventArgs e)
        {
            var t = sender as ApplicationBarIconButton;
            if (t != null)
            {
                criarCards();
                
                Cards card = new Cards();
                Random rand = new Random();
                int id = rand.Next(1,2);
                card = card.getCard(id);
                card.Increase();
                AtualizarCards();
            }
        }

        private void AtualizarCards()
        {
            Cards cards = new Cards();
            listCards.ItemsSource = cards.getMyCards();
        }

        private void criarCards()
        {
            Cards card = new Cards();
            card.nomeJogador = "briano";
            card.timeJogador = "time maroto";
            card.caminhoFoto = "/Assets/Cards/HueHueBr/JulioCesar.jpg";
            card.nivelRaridade = 1;
            card.quantidade = 1;
            card.Save();
        }

        private void trocar_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            //TODO:Implementar sistema de troca de cards
        }
    }
}