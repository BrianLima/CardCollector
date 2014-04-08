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
        /// Get's every card on the database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Cards> getAllCards()
        {
            List<Cards> data = new List<Cards>();
            using (DataBaseContext db = new DataBaseContext(DataBaseContext.ConnectionString))
            {
                data = (from cards in db.cards 
                         orderby cards.PlayerTeam 
                         select cards).ToList();
            }
            return data;
        }

        /// <summary>
        /// Get all the player cards
        /// </summary>
        /// <returns></returns>
        internal IEnumerable getMyCards()
        {
            List<Cards> data = new List<Cards>();
            using(DataBaseContext db = new DataBaseContext(DataBaseContext.ConnectionString))
            {
                data = (from cards in db.cards
                         where cards.Amount > 0
                         orderby cards.PlayerTeam
                         select cards).ToList();
            }
            return data;
        }

        /// <summary>
        /// Get a specific card
        /// </summary>
        /// <param name="id">card's id</param>
        /// <returns>card</returns>
        public Cards getCard(int id)
        {
            List<Cards> data = new List<Cards>();
            using(DataBaseContext db = new DataBaseContext(DataBaseContext.ConnectionString))
            {
                data = (from e in db.cards 
                        where e.id == id 
                        select e).ToList();
            }
            return data[0];
        }

        /// <summary>
        /// Salva um card para o banco de dados
        /// </summary>
        /// <param name="card"></param>
        /// <returns>bool</returns>
        public bool Save(Cards card)
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
        /// <returns>bool</returns>
        public bool Destroy(Cards card)
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

        /// <summary>
        /// Increase the card amount
        /// </summary>
        /// <param name="card"></param>
        /// <returns>bool</returns>
        public bool Increase(Cards card)
        {
            try
            {
                using (DataBaseContext db = new DataBaseContext(DataBaseContext.ConnectionString))
                {
                    Cards update = (from tar in db.cards
                                    where tar.id == card.id
                                    select tar).First();
                    update.Amount += 1;
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
        /// Decrease the card amount
        /// </summary>
        /// <param name="card"></param>
        /// <returns>bool</returns>
        public bool Decrease(Cards card)
        {
            try
            {
                using (DataBaseContext db = new DataBaseContext(DataBaseContext.ConnectionString))
                {
                    Cards update = (from tar in db.cards
                                    where tar.id == card.id
                                    select tar).First();
                    update.Amount -= 1;
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