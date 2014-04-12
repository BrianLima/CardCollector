using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using Windows.Networking.Sockets;
using CardDataBase;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Microsoft.Phone.Tasks;
using Windows.Networking.Proximity;
using CardCollector.Resources;

namespace CardCollector
{
    public partial class SwapCard : PhoneApplicationPage
    {
        public SwapCard()
        {
            InitializeComponent();
        }

    }
}