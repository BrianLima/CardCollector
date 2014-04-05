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
    }
}