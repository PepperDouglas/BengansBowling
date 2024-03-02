using BengansBowling.Models;
using BengansBowling.Observers;
using BengansBowling.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class CompetitionRepository : ICompetitionRepository
{
    private readonly BowlingAlleyContext _context;
    private readonly ApplicationEventPublisher _eventPublisher;

    public CompetitionRepository(BowlingAlleyContext context) {
        _context = context;
        _eventPublisher = new ApplicationEventPublisher();
        var loggerObserver = new LoggerObserver();
        _eventPublisher.Attach(loggerObserver);
    }

    public async Task<IEnumerable<Competition>> GetAllAsync() {
        return await _context.Competitions.Include(c => c.Matches).ToListAsync();
    }

    public async Task<Competition> GetByIdAsync(int competitionId) {
        return await _context.Competitions
                             .Include(c => c.Matches)
                                 .ThenInclude(m => m.PlayerOne) // Eagerly load PlayerOne
                             .Include(c => c.Matches)
                                 .ThenInclude(m => m.PlayerTwo) // Eagerly load PlayerTwo
                             .Include(c => c.Matches)
                                 .ThenInclude(m => m.Series) // Include this if you need Series too
                             .FirstOrDefaultAsync(c => c.Id == competitionId);
    }

    public async Task AddAsync(Competition competition) {
        try {
            await _context.Competitions.AddAsync(competition);
            await _context.SaveChangesAsync();
            _eventPublisher.Notify($"Competition added: {competition.Name}");

        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task UpdateAsync(Competition competition) {
        var existingCompetition = await _context.Competitions.FindAsync(competition.Id);
        if (existingCompetition != null) {
            // Map the properties you wish to update
            existingCompetition.Name = competition.Name;
            // Add other properties as needed
            //notneeded maybe
            _context.Competitions.Update(competition);
            await _context.SaveChangesAsync();
            _eventPublisher.Notify($"Competition updated: {competition.Name}");
        }
    }

    // Removed Update and Delete methods
}
