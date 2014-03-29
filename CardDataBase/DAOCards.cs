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
        public IEnumerable<Cards> ObtemCards()
            {
              List<Cards> dados = new List<Cards>();
              using (DataBaseContext db = new DataBaseContext(DataBaseContext.ConnectionString))
              {
                dados = (from cards in db.cards orderby cards.timeJogador select cards).ToList();
              }
              return dados;
            }
 
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
 
            public bool Realizado(Cards card)
            {
              try
              {
                using (DataBaseContext db = new DataBaseContext(DataBaseContext.ConnectionString))
                {
                  Cards update = (from tar in db.cards
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
