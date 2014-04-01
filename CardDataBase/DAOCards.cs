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
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace CardDataBase
{
    public class DAOCards
    {
        /// <summary>
        /// Pesquisa todos os cards cadastrados
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Cards> ObtemCards()
        {
            List<Cards> dados = new List<Cards>();
            using (DataBaseContext db = new DataBaseContext(DataBaseContext.ConnectionString))
            {
                dados = (from cards in db.cards 
                         orderby cards.timeJogador 
                         select cards).ToList();
            }
            return dados;
        }

        /// <summary>
        /// Pesquisa um card especifico
        /// </summary>
        /// <param name="id">id do card</param>
        /// <returns>card</returns>
        public Cards obterCard(int id)
        {
            List<Cards> dado = new List<Cards>();
            using(DataBaseContext db = new DataBaseContext(DataBaseContext.ConnectionString))
            {
                dado = (from e in db.cards 
                        where e.id == id 
                        select e).ToList();
            }
            return dado[0];
        }

        /// <summary>
        /// Salva um card para o banco de dados
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public bool Gravar(Cards card)
        {
            try
            {
                using (DataBaseContext db = new DataBaseContext(DataBaseContext.ConnectionString))
                {
                    db.cards.InsertOnSubmit(card);
                    db.SubmitChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Exclui o card do banco de dados
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public bool Excluir(Cards card)
        {
            try
            {
                using (DataBaseContext db = new DataBaseContext(DataBaseContext.ConnectionString))
                {
                    var excluir = db.cards.Where(t => t.id == card.id).First();
                    db.cards.DeleteOnSubmit(excluir);
                    db.SubmitChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Incrementa(Cards card)
        {
            try
            {
                using (DataBaseContext db = new DataBaseContext(DataBaseContext.ConnectionString))
                {
                    Cards update = (from tar in db.cards
                                    where tar.id == card.id
                                    select tar).First();
                    update.quantidade += 1;
                    db.SubmitChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Decrementa(Cards card)
        {
            try
            {
                using (DataBaseContext db = new DataBaseContext(DataBaseContext.ConnectionString))
                {
                    Cards update = (from tar in db.cards
                                    where tar.id == card.id
                                    select tar).First();
                    update.quantidade -= 1;
                    db.SubmitChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}