using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using CardDataBase;

namespace CardCollector
{
    public partial class PlayerCardControl : UserControl
    {
        public PlayerCardControl()
        {
            InitializeComponent();
        }

        private void fotoJogador_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var t = sender as Image;
            if (t != null)
            {
                Cards card = t.DataContext as Cards;
                card.Increase();
            }
        }
    }
}
