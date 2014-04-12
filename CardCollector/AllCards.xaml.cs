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

        private void StackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var listItem = FindParent(sender as StackPanel, typeof(PlayerCardControl)) as PlayerCardControl;
        }

        private DependencyObject FindParent(DependencyObject child, Type type)
        {
            var parent = VisualTreeHelper.GetParent(child);

            if (parent != null && !type.IsInstanceOfType(parent))
            {
                return FindParent(parent, type);
            }
            else
            {
                return parent;
            }
        }
    }
}