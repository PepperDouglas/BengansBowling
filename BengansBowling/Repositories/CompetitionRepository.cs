using BengansBowling.Models;
using BengansBowling.Observers;
using BengansBowling.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//Repository pattern används för att kapsla in den logik som krävs för att komma åt datakällor,
//vilket ger ett mer abstrakt gränssnitt till datalagret. Denna separation av problem gör dataåtkomstlagret mer hanterbart
//och det förenklar dataoperationer som CRUD, vilket gör applikationen mer underhållbar och testbar.


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
                                 .ThenInclude(m => m.PlayerOne)
                             .Include(c => c.Matches)
                                 .ThenInclude(m => m.PlayerTwo)
                             .Include(c => c.Matches)
                                 .ThenInclude(m => m.Series)
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
            existingCompetition.Name = competition.Name;
            _context.Competitions.Update(competition);
            await _context.SaveChangesAsync();
            _eventPublisher.Notify($"Competition updated: {competition.Name}");
        }
    }
}
