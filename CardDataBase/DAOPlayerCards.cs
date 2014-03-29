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
    public class DAOPlayerCards
    {
        public IEnumerable<PlayerCards> ObtemCards()
        {
            List<PlayerCards> dados = new List<PlayerCards>();
            using (PlayerDataBaseContext db = new PlayerDataBaseContext(DataBaseContext.ConnectionString))
            {
                dados = (from playerCards in db.playerCards orderby playerCards.timeJogador select playerCards).ToList();
            }
            return dados;
        }

        public bool Gravar(PlayerCards playerCard)
        {
            try
            {
                using (PlayerDataBaseContext db = new PlayerDataBaseContext(PlayerDataBaseContext.ConnectionString))
                {
                    db.playerCards.InsertOnSubmit(playerCard);
                    db.SubmitChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Excluir(PlayerCards playerCard)
        {
            try
            {
                using (PlayerDataBaseContext db = new PlayerDataBaseContext(PlayerDataBaseContext.ConnectionString))
                {
                    var excluir = db.playerCards.Where(t => t.id == playerCard.id).First();
                    db.playerCards.DeleteOnSubmit(excluir);
                    db.SubmitChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Realizado(PlayerCards card)
        {
            try
            {
                using (PlayerDataBaseContext db = new PlayerDataBaseContext(PlayerDataBaseContext.ConnectionString))
                {
                    PlayerCards update = (from tar in db.playerCards
                                          where tar.id == card.id
                                          select tar).First();
                    //update.Realizada = !update.Realizada;
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