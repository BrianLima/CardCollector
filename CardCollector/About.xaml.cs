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
            ApplicationBarIconButton appBarButtonReview = new ApplicationBarIconButton();
            appBarButtonReview.Text = "Avalie";
            appBarButtonReview.Click += appBarButtonReview_Click;
            ApplicationBar.Buttons.Add(appBarButtonReview);
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
    }
}