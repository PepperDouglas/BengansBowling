﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using BengansBowling.Models;
using BengansBowling.Observers;
using BengansBowling.Repositories;
using Newtonsoft.Json;

public class TrackRepository : ITrackRepository
{
    private readonly string _filePath = "Tracks.json";
    private List<Track> _tracks;
    private readonly ApplicationEventPublisher _eventPublisher;

    public TrackRepository() {
        _tracks = LoadTracks() ?? new List<Track>();

        _eventPublisher = new ApplicationEventPublisher();
        var loggerObserver = new LoggerObserver();
        _eventPublisher.Attach(loggerObserver);
    }

    private List<Track> LoadTracks() {
        if (!File.Exists(_filePath)) {
            return new List<Track>();
        }

        var json = File.ReadAllText(_filePath);
        return JsonConvert.DeserializeObject<List<Track>>(json);
    }

    private void SaveChanges() {
        var json = JsonConvert.SerializeObject(_tracks, Formatting.Indented);
        File.WriteAllText(_filePath, json);
    }

    public IEnumerable<Track> GetAll() {
        return _tracks;
    }

    public Track GetById(int id) {
        return _tracks.FirstOrDefault(t => t.Id == id);
    }

    public void Add(Track track) {
        _tracks.Add(track);
        SaveChanges();
        _eventPublisher.Notify($"Track added: {track.Name}");
    }

    public void Update(Track track) {
        var index = _tracks.FindIndex(t => t.Id == track.Id);
        if (index != -1) {
            _tracks[index] = track;
            SaveChanges();
            _eventPublisher.Notify($"Track added: {track.Name}");
        }
    }

    public void Delete(int id) {
        var track = GetById(id);
        if (track != null) {
            _tracks.Remove(track);
            SaveChanges();
            _eventPublisher.Notify($"Track added: {track.Name}");
        }
    }
}
