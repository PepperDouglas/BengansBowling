using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BengansBowling.Models
{
    public class Track
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Track(int id, string name) {
            Id = id;
            Name = name;
        }
    }
}
