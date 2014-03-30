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

        private int _idCard;
        [Column(Name = "idCard", CanBeNull = false)]
        public int idCard{ get { return _idCard; } set { _idCard = value; } }

        private int _quantidade;
        [Column(Name="quantidade", CanBeNull = false)]
        public int quantidade { get { return _quantidade; } set { _quantidade = value; } }

        public IEnumerable ObtemCards()
        {
            DAOPlayerCards daoPlayerCards = new DAOPlayerCards();
            return daoPlayerCards.ObtemCards();
        }

        public bool Incluir()
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