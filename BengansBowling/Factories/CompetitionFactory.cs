using BengansBowling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BengansBowling.Factories
{

    public static class CompetitionFactory
    {
        public static Competition CreateCompetition(CompetitionType type, string name) {
            int newId = GenerateNewId(); // Assume this method generates a unique ID for the competition
            return new Competition(newId, name, type);
        }

        // Placeholder for a method to generate a unique ID for a new competition
        private static int GenerateNewId() {
            // Implement ID generation logic here
            return new Random().Next(1, 1000); // Simplified example, adjust as necessary
        }
    }
}

