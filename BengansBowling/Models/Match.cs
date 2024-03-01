using BengansBowling.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BengansBowling.Models
{
    public class Match
    {
        public int Id { get; set; }
        public Member PlayerOne { get; set; }
        public Member PlayerTwo { get; set; }
        public List<Series> Series { get; set; }
        public Track Track { get; set; } // Reference to the Track
        public Member Winner { get; private set; } // The winner of the match
        public IScoringStrategy ScoringStrategy { get; set; }

        public Match(int id, Member playerOne, Member playerTwo, Track track) {
            Id = id;
            PlayerOne = playerOne;
            PlayerTwo = playerTwo;
            Track = track;
            Series = new List<Series>();
        }

        // Method to determine the winner based on series victories
        public void DetermineWinner() {
            Winner = ScoringStrategy.DetermineWinner(this);
        }
    }
}
