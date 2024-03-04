using BengansBowling.Models;
using BengansBowling.Observers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BengansBowling.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly BowlingAlleyContext _context;
        private readonly ApplicationEventPublisher _eventPublisher;

        public MatchRepository(BowlingAlleyContext context) {
            _context = context;

            _eventPublisher = new ApplicationEventPublisher();
            var loggerObserver = new LoggerObserver();
            _eventPublisher.Attach(loggerObserver);
        }

        public async Task<IEnumerable<Match>> GetAllAsync() {
            return await _context.Matches.Include(m => m.PlayerOne)
                                         .Include(m => m.PlayerTwo)
                                         .Include(m => m.Track)
                                         .ToListAsync();
        }

        public async Task<Match> GetByIdAsync(int id) {
            return await _context.Matches.Include(m => m.PlayerOne)
                                         .Include(m => m.PlayerTwo)
                                         .Include(m => m.Track)
                                         .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddAsync(Match match) {
            try {
                await _context.Matches.AddAsync(match);
                await _context.SaveChangesAsync();

            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task UpdateAsync(Match match) {
            var existingMatch = await _context.Matches.Include(m => m.Series)
                                             .FirstOrDefaultAsync(m => m.Id == match.Id);
            if (existingMatch != null) {
                existingMatch.PlayerOneId = match.PlayerOneId;
                existingMatch.PlayerTwoId = match.PlayerTwoId;
                existingMatch.TrackId = match.TrackId;
                await _context.SaveChangesAsync();
                _eventPublisher.Notify($"Match updated: {match.Id}");
            } else {
            }
        }
    }

}
