using System.Collections;
using System.Data.Linq.Mapping;

namespace CardDataBase
{
    /*
     * Classe base para o banco de dados
     * Tabela: Cards
     */
    [Table(Name = "Cards")]
    public class Cards
    {
        private int _id;
        [Column(Name = "id", IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
        public int id { get { return _id; } set { _id = value; } }

        private string _PlayerName;
        [Column(Name = "PlayerName", CanBeNull = false)]
        public string PlayerName { get { return _PlayerName; } set { _PlayerName = value; } }

        private string _PlayerTeam;
        [Column(Name="PlayerTeam", CanBeNull = false)]
        public string PlayerTeam { get { return _PlayerTeam; } set { _PlayerTeam = value; } }

        private string _PlayerPath;
        [Column(Name = "PlayerPath", CanBeNull = false)]
        public string PlayerPath { get { return _PlayerPath; } set { _PlayerPath = value; } }

        private int _Rarity;
        [Column(Name = "Rarity", CanBeNull=false)]
        public int Rarity { get { return _Rarity; } set { _Rarity = value; } }

        private int _Amount;
        [Column(Name = "Amount", CanBeNull = true)]
        public int Amount { get { return _Amount; } set { _Amount = value; } }

        public IEnumerable getAllCards()
        {
            DAOCards daoCards = new DAOCards();
            return daoCards.getAllCards();
        }

        public IEnumerable getMyCards()
        {
            DAOCards daoCards = new DAOCards();
            return daoCards.getMyCards();
        }

        public Cards getCard(int id)
        {
            DAOCards daorCards = new DAOCards();
            return daorCards.getCard(id);
        }

        public bool Save()
        {
            DAOCards daoCards = new DAOCards();
            return daoCards.Save(this);
        }

        public bool Destroy()
        {
            DAOCards daoCards = new DAOCards();
            return daoCards.Destroy(this);
        }

        public bool Increase()
        {
            DAOCards daoCards = new DAOCards();
            return daoCards.Increase(this);
        }

        public bool Decrease()
        {
            DAOCards daoCards = new DAOCards();
            return daoCards.Decrease(this);
        }
    }
}
