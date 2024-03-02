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
        Task<IEnumerable<Competition>> GetAllAsync();
        Task<Competition> GetByIdAsync(int id);
        Task AddAsync(Competition competition);
        Task UpdateAsync(Competition competition); // Reintroduced
    }

}
