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
using System.Windows.Media;

namespace CardCollector
{
    public partial class PlayerCardControl : UserControl
    {

        private string _color;
        public string color { get { return _color; } set { _color = value; } }

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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var t = sender as PlayerCardControl;
            if (t != null)
            {
                Cards card = t.DataContext as Cards;
                if (card.PlayerTeam == "Brasil")
                {
                    color1.Color = Colors.Green;
                    color2.Color = Colors.Yellow;
                }
            }
        }
    }
}
