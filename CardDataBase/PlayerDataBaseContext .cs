using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Data.Linq;

namespace CardDataBase
{
    class PlayerDataBaseContext : DataContext
    {
        public static string ConnectionString = "Data Source=isostore:/PlayerCards.sdf";

        private Table<PlayerCards> _PlayerCards;

        public Table<PlayerCards> playerCards
        {
            get
            {
                if (_PlayerCards == null)
                    _PlayerCards = GetTable<PlayerCards>();

                return _PlayerCards;
            }
        }

        public PlayerDataBaseContext(string connectionString)
            : base(connectionString)
        {
            if (!this.DatabaseExists())
                this.CreateDatabase();
        }

    }
}