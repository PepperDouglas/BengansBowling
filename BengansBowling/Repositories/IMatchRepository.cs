using BengansBowling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BengansBowling.Repositories
{
    public interface IMatchRepository
    {
        Task<IEnumerable<Match>> GetAllAsync();
        Task<Match> GetByIdAsync(int id);
        Task AddAsync(Match match);
        Task UpdateAsync(Match match);
    }

}
