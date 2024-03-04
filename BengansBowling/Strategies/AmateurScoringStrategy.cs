using BengansBowling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Strategy pattern används för att definiera en familj av algoritmer, kapsla in var och en och göra dem utbytbara,
//vilket gör att algoritmen kan variera oberoende av klienter som använder den. I samband med poängtävlingar gör det
//det möjligt för applikationen att växla mellan olika poängalgoritmer (t.ex. ProfessionalScoringStrategy vs. AmateurScoringStrategy)
//baserat på tävlingstypen.


namespace BengansBowling.Strategies
{
    public class AmateurScoringStrategy : IScoringStrategy
    {
        public Member DetermineWinner(Match match) {
            var lastSeries = match.Series.Last();
            return lastSeries.ScorePlayerOne > lastSeries.ScorePlayerTwo ? match.PlayerOne : match.PlayerTwo;
        }
    }

}
