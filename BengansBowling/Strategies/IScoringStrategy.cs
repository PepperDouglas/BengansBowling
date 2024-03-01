using BengansBowling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BengansBowling.Strategies
{
    public interface IScoringStrategy
    {
        Member DetermineWinner(Match match);
    }
}
