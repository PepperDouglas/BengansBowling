using BengansBowling.Strategies;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public int CompetitionId { get; set; }
        public Competition Competition { get; set; }
        public int PlayerOneId { get; set; }
        public int PlayerTwoId { get; set; }
        public int TrackId { get; set; }
        public List<Series> Series { get; set; }
        public Track Track { get; set; }
        public Member? Winner { get; private set; }
        
        [NotMapped]
        public IScoringStrategy ScoringStrategy { get; set; }

        public Match() { }

        public Match(int id, Member playerOne, Member playerTwo, Track track) {
            Id = id;
            PlayerOne = playerOne;
            PlayerTwo = playerTwo;
            Track = track;
            Series = new List<Series>();
        }

        public void DetermineWinner() {
            Winner = ScoringStrategy.DetermineWinner(this);
        }
    }
}
