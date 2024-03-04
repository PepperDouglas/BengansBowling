using System.Collections.Generic;
using System.IO;
using System.Linq;
using BengansBowling.Models;
using BengansBowling.Observers;
using BengansBowling.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

public class TrackRepository : ITrackRepository
{

    private readonly BowlingAlleyContext _context;
    private readonly ApplicationEventPublisher _eventPublisher;

    public TrackRepository(BowlingAlleyContext context) {
        _context = context;
        _eventPublisher = new ApplicationEventPublisher();
        var loggerObserver = new LoggerObserver();
        _eventPublisher.Attach(loggerObserver);
    }

    public async Task<IEnumerable<Track>> GetAllAsync() {
        return await _context.Tracks.ToListAsync();
    }

    public async Task<Track> GetByIdAsync(int id) {
        return await _context.Tracks.FindAsync(id);
    }

    public async Task AddAsync(Track track) {
        await _context.Tracks.AddAsync(track);
        await _context.SaveChangesAsync();
        // Logging after successfully adding a track
        _eventPublisher.Notify($"Track added: {track.Name}");
    }
}
