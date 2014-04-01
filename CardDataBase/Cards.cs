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

        private string _nomeJogador;
        [Column(Name = "nomeJogador", CanBeNull = false)]
        public string nomeJogador { get { return _nomeJogador; } set { _nomeJogador = value; } }

        private string _timeJogador;
        [Column(Name="timeJogador", CanBeNull = false)]
        public string timeJogador { get { return _timeJogador; } set { _timeJogador = value; } }

        private string _caminhoFoto;
        [Column(Name = "caminhoFoto", CanBeNull = false)]
        public string caminhoFoto { get { return _caminhoFoto; } set { _caminhoFoto = value; } }

        private int _nivelRaridade;
        [Column(Name = "nivelRaridade", CanBeNull=false)]
        public int nivelRaridade { get { return _nivelRaridade; } set { _nivelRaridade = value; } }

        private int _quantidade;
        [Column(Name = "quantidade", CanBeNull = true)]
        public int quantidade { get { return _quantidade; } set { _quantidade = value; } }

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
