using BengansBowling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BengansBowling.Repositories
{
    public interface ICompetitionRepository
    {
        IEnumerable<Competition> GetAll();
        Competition GetById(int id);
        void Add(Competition competition);
        void Update(Competition competition);
        void Delete(int id);
    }
}
