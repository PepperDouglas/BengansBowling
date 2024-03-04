using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BengansBowling.Models
{
    public class Series
    {
        public int Id { get; set; }
        public int ScorePlayerOne { get; set; }
        public int ScorePlayerTwo { get; set; }

        public Series(int scorePlayerOne, int scorePlayerTwo) {
            ScorePlayerOne = scorePlayerOne;
            ScorePlayerTwo = scorePlayerTwo;
        }
    }
}
