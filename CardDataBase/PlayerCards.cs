using System.Collections;
using System.Data.Linq.Mapping;

namespace CardDataBase
{
    /*
     * Classe base para o banco de dados
     * Tabela: PlayerCards
     */
    [Table(Name = "PlayerCards")]
    public class PlayerCards
    {
        private int _id;
        [Column(Name = "id", IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
        public int id { get { return _id; } set { _id = value; } }

        private string _idCard;
        [Column(Name = "idCard", CanBeNull = false)]
        public string idCard{ get { return _idCard; } set { _idCard = value; } }

        private string _quantidade;
        [Column(Name="quantidade", CanBeNull = false)]
        public string timeJogador { get { return _quantidade; } set { _quantidade = value; } }

        public IEnumerable ObtemCards()
        {
            DAOPlayerCards daoPlayerCards = new DAOPlayerCards();
            return daoPlayerCards.ObtemCards();
        }

        public bool Gravar()
        {
            DAOPlayerCards daoPlayerCards = new DAOPlayerCards();
            return daoPlayerCards.Gravar(this);
        }

        public bool Excluir()
        {
            DAOPlayerCards daoPlayerCards = new DAOPlayerCards();
            return daoPlayerCards.Excluir(this);
        }
    }
}