using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using CardCollector;
using CardDataBase;
using System.Windows.Media;

namespace CardCollector
{
    public partial class AllCards : PhoneApplicationPage
    {
        public AllCards()
        {
            InitializeComponent();
            AtualizaCards();
        }

        private void AtualizaCards()
        {
            Cards cards = new Cards();
            listCards.ItemsSource = cards.getAllCards();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var t = sender as MenuItem;
            if (t != null)
            {     
                Cards card = t.DataContext as Cards;

                string stringVariable = card.id.ToString();
                NavigationService.Navigate(new Uri("/SwapCard.xaml", UriKind.Relative));

                //NavigationService.Navigate(new Uri("/SwapCard.xaml?parameter=" + card.id.ToString(), UriKind.Relative));
            }
        }
    }
}