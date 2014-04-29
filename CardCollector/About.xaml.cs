using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using CardDataBase;

namespace CardCollector
{
    public partial class About : PhoneApplicationPage
    {
        public About()
        {
            InitializeComponent();
        }

        private void appBarButtonReview_Click(object sender, EventArgs e)
        {
            MarketplaceReviewTask task = new MarketplaceReviewTask();
            task.Show();
            GetCard();
        }

        private void GetCard()
        {
            Random rand = new Random();
            int id = rand.Next(0, 2);
            Cards card = new Cards();
            card = card.getCard(id);
            card.Increase();
        }

        private void banner3_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask task = new WebBrowserTask();
            task.Uri = new Uri("http://www.windowsphone.com/pt-br/store/app/tecmundo/b38875aa-3a21-4c7a-a0c7-79e11058d7ce", UriKind.Absolute);
            task.Show();

            for (int i = 0; i < 5; i++)
            {
                GetCard();
            }
        }

        private void banner2_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask task = new WebBrowserTask();
            task.Uri = new Uri("http://www.windowsphone.com/pt-br/apps/8df4ca1e-8318-439b-b6c5-fa9a68a12f4b", UriKind.Absolute);
            task.Show();

            for (int i = 0; i < 5; i++)
            {
                GetCard();
            }
        }

        private void banner1_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask task = new WebBrowserTask();
            task.Uri = new Uri("http://www.windowsphone.com/pt-br/store/app/mundo-automotivo/dba88e3d-a519-49e5-9992-f79c88ada339", UriKind.Absolute);
            task.Show();

            for (int i = 0; i < 5; i++)
            {
                GetCard();
            }
        }

        private void banner4_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask task = new WebBrowserTask();
            task.Uri = new Uri("https://twitter.com/BrianoStorm", UriKind.Absolute);
            task.Show();

            for (int i = 0; i < 5; i++)
            {
                GetCard();
            }
        }
    }
}