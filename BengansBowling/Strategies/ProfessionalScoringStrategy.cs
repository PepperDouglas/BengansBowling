using BengansBowling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BengansBowling.Strategies
{
    public class ProfessionalScoringStrategy : IScoringStrategy
    {
        public Member DetermineWinner(Match match) {
            int winsPlayerOne = 0;
            int winsPlayerTwo = 0;

            foreach (var series in match.Series) {
                if (series.ScorePlayerOne > series.ScorePlayerTwo) winsPlayerOne++;
                else if (series.ScorePlayerTwo > series.ScorePlayerOne) winsPlayerTwo++;
            }

            if (winsPlayerOne > winsPlayerTwo) return match.PlayerOne;
            else if (winsPlayerTwo > winsPlayerOne) return match.PlayerTwo;
            else return null; // It's a tie
        }
    }

}
