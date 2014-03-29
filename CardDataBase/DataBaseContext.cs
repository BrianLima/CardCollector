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
    class DataBaseContext : DataContext
    {
        public static string ConnectionString = "Data Source=isostore:/cards.sdf";

        private Table<Cards> _Cards;

        public Table<Cards> cards
        {
            get
            {
                if (_Cards == null)
                    _Cards = GetTable<Cards>();

                return _Cards;
            }
        }

        public DataBaseContext(string connectionString)
            : base(connectionString)
        {
            if (!this.DatabaseExists())
                this.CreateDatabase();
        }

    }
}
