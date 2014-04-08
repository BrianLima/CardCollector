using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardDataBase;

namespace CardCollector
{
    public static class Players
    {
        public static String[,] players = new String[7,5]
        {
           {"Julio Cesar","Brasil","/Assets/Cards/HueHueBr/JulioCesar.jpg","1", "0"},
           {"Thiago Silva","Brasil", "/Assets/Cards/HueHueBr/ThiagoSilva.jpg","1", "0"},
           {"David Luiz","Brasil", "/Assets/Cards/HueHueBr/DavidLuiz.jpg","1", "0"},
           {"Dani Alves","Brasil", "/Assets/Cards/HueHueBr/DaniAlves.jpg","1", "0"},
           {"Marcelo","Brasil", "/Assets/Cards/HueHueBr/Marcelo.jpg","1", "0"},
           {"Dante","Brasil", "/Assets/Cards/HueHueBr/Dante.png","1", "0"},
           {"Ramires","Brasil", "/Assets/Cards/HueHueBr/Ramires.jpg","1", "0"},
        };

        public static void gerarCards()
        {
            Cards card = new Cards();
            for (int i = 0; i < 6; i++)
            {
                card.PlayerName = players[i, 0];
                card.PlayerTeam = players[i, 1];
                card.PlayerPath = players[i, 2];
                card.Rarity = Int32.Parse(players[i,3]);
                card.Amount = Int32.Parse(players[i,4]);
                card.Save();
            }
        }
    }
}
