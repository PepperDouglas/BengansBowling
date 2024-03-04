using BengansBowling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

//Factory pattern används för att abstrahera instansieringsprocessen för objekt, vilket tillåter skapandet av objekt
//utan att specificera den exakta klassen för objektet som ska skapas. Detta är särskilt användbart i scenarier som att skapa
//olika typer av tävlingar (amatör eller professionell) baserat på inmatningskriterier.

namespace BengansBowling.Factories
{

    public static class CompetitionFactory
    {
        public static Competition CreateCompetition(CompetitionType type, string name) {
            return new Competition(name, type);
        }
    }
}

