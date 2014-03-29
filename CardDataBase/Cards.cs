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

        public IEnumerable ObtemCards()
        {
            DAOCards daoCards = new DAOCards();
            return daoCards.ObtemCards();
        }

        public bool Gravar()
        {
            DAOCards daoCards = new DAOCards();
            return daoCards.Gravar(this);
        }

        public bool Excluir()
        {
            DAOCards daoCards = new DAOCards();
            return daoCards.Excluir(this);
        }
    }
}
