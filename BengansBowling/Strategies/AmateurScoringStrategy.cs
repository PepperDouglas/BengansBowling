using BengansBowling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BengansBowling.Strategies
{
    public class AmateurScoringStrategy : IScoringStrategy
    {
        public Member DetermineWinner(Match match) {
            // Assuming there are always exactly 2 series for amateur matches
            var lastSeries = match.Series.Last();
            return lastSeries.ScorePlayerOne > lastSeries.ScorePlayerTwo ? match.PlayerOne : match.PlayerTwo;
        }
    }

}
