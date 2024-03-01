using BengansBowling.Models;
using BengansBowling.Observers;
using BengansBowling.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BengansBowling.Repositories
{
    public class CompetitionRepository : ICompetitionRepository
    {
        private const string FilePath = "Competitions.json";
        private List<Competition> _competitions;
        private readonly ApplicationEventPublisher _eventPublisher;

        public CompetitionRepository() {
            _competitions = FileStorageHelper.ReadFromJsonFile<List<Competition>>(FilePath) ?? new List<Competition>();

            _eventPublisher = new ApplicationEventPublisher();
            var loggerObserver = new LoggerObserver();
            _eventPublisher.Attach(loggerObserver);
        }

        public IEnumerable<Competition> GetAll() {
            return _competitions;
        }

        public Competition GetById(int id) {
            return _competitions.FirstOrDefault(c => c.Id == id);
        }

        public void Add(Competition competition) {
            _competitions.Add(competition);
            SaveChanges();
            _eventPublisher.Notify($"Competition added: {competition.Name}");
        }

        public void Update(Competition competition) {
            var index = _competitions.FindIndex(c => c.Id == competition.Id);
            if (index != -1) {
                _competitions[index] = competition;
                SaveChanges();
                _eventPublisher.Notify($"Competition updated: {competition.Name}");
            }
        }

        public void Delete(int id) {
            var competition = GetById(id);
            if (competition != null) {
                _competitions.Remove(competition);
                SaveChanges();
                _eventPublisher.Notify($"Competition removed: {competition.Name}");
            }
        }

        private void SaveChanges() {
            FileStorageHelper.WriteToJsonFile(FilePath, _competitions);
        }
    }

}
