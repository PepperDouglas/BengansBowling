using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BengansBowling.Models
{
    public enum CompetitionType
    {
        Amateur,
        Professional
    }

    public class Competition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Match> Matches { get; set; }
        public CompetitionType Type { get; set; }

        public Competition(string name, CompetitionType type) {
            //Id = id;
            Name = name;
            Type = type;
            Matches = new List<Match>();
        }
    }
}

