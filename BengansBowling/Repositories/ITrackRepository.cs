using BengansBowling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BengansBowling.Repositories
{
    public interface ITrackRepository
    {
        IEnumerable<Track> GetAll();
        Track GetById(int id);
        void Add(Track track);
        void Update(Track track);
        void Delete(int id);
    }

}
