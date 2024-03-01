using BengansBowling.Factories;
using BengansBowling.Models;
using BengansBowling.Observers;
using BengansBowling.Repositories;
using BengansBowling.Strategies;
using BengansBowling.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BengansBowling.Controllers
{
    public class CompetitionController
    {
        private readonly ICompetitionRepository _competitionRepository;
        private readonly IMemberRepository _memberRepository; // Assume you have this
        // Assuming you have an alley repository or a way to manage alleys
        private readonly ITrackRepository _trackRepository;
        private readonly ApplicationEventPublisher _eventPublisher;

        public CompetitionController(ICompetitionRepository competitionRepository, IMemberRepository memberRepository, ITrackRepository trackRepository) {
            _competitionRepository = competitionRepository;
            _memberRepository = memberRepository;
            _trackRepository = trackRepository; // Initialize the track repository

            _eventPublisher = new ApplicationEventPublisher();
            var loggerObserver = new LoggerObserver();
            _eventPublisher.Attach(loggerObserver);
        }

        public void AddCompetition(string name, CompetitionType type) {
            // Check if the repository is empty to determine the new ID
            int newId = 1;
            if (_competitionRepository.GetAll().Any()) {
                newId = _competitionRepository.GetAll().Max(c => c.Id) + 1;
            }

            // Use the CompetitionFactory to create a competition of the specified type
            var competition = CompetitionFactory.CreateCompetition(type, name);
            competition.Id = newId; // Set the newly generated ID

            _competitionRepository.Add(competition);
            Console.WriteLine($"{type} competition added successfully: {name}");
        }

        public void UpdateCompetition(int id, string name) {
            var competition = _competitionRepository.GetById(id);
            if (competition != null) {
                competition.Name = name;
                _competitionRepository.Update(competition);

                Console.WriteLine("Competition updated successfully.");
            } else {
                Console.WriteLine("Competition not found.");
            }
        }

        public void DeleteCompetition(int id) {
            _competitionRepository.Delete(id);
            Console.WriteLine("Competition deleted successfully.");
        }

        public void ListCompetitions() {
            var competitions = _competitionRepository.GetAll();
            foreach (var competition in competitions) {
                Console.WriteLine($"ID: {competition.Id}, Name: {competition.Name}");
            }
            Console.Write("Enter the ID of the competition to view details: ");
            int selectedId = Convert.ToInt32(Console.ReadLine());
            DisplayCompetitionDetails(selectedId);
        }

        public void DisplayCompetitionDetails(int competitionId) {
            var competition = _competitionRepository.GetById(competitionId);
            if (competition == null) {
                Console.WriteLine("Competition not found.");
                return;
            }

            Console.WriteLine($"Competition: {competition.Name}");
            foreach (var match in competition.Matches) {
                string result = match.Winner != null ? $"Winner: {match.Winner.Name}" : "No winner yet (or tie)";
                Console.WriteLine($"Match {match.Id} between {match.PlayerOne.Name} and {match.PlayerTwo.Name}: {result}");
            }
        }

        public void AddMatchesToCompetition(int competitionId, List<int> memberIds) {
            var competition = _competitionRepository.GetById(competitionId);
            if (competition == null) {
                Console.WriteLine("Competition not found.");
                _eventPublisher.Notify($"Failed adding matches: Competition not found");
                return;
            }

            if (memberIds.Count % 2 != 0) {
                Console.WriteLine("The number of players must be even.");
                _eventPublisher.Notify($"Failed adding matches: All players could not be paired");
                return;
            }

            var availableTracks = _trackRepository.GetAll().ToList();
            if (availableTracks.Count < memberIds.Count / 2) {
                Console.WriteLine("Not enough available tracks.");
                _eventPublisher.Notify($"Failed adding matches: Not enough available tracks");
                return;
            }

            int nextMatchId = competition.Matches.Any() ? competition.Matches.Max(m => m.Id) + 1 : 1;
            int seriesCount = competition.Type == CompetitionType.Amateur ? 2 : 3; // Determine series count based on competition type

            for (int i = 0; i < memberIds.Count; i += 2) {
                var playerOne = _memberRepository.GetById(memberIds[i]);
                var playerTwo = _memberRepository.GetById(memberIds[i + 1]);
                var track = availableTracks[i / 2];

                // Create the Match with an empty Series list
                var match = new Match(nextMatchId++, playerOne, playerTwo, track);

                // Reset seriesId for each match
                int seriesId = 1;

                // Initialize the Series list based on the determined series count
                for (int seriesIndex = 0; seriesIndex < seriesCount; seriesIndex++) {
                    // Initialize scores to 0
                    match.Series.Add(new Series(seriesId++, 0, 0)); // Adjusted to use Series constructor
                }

                // Assign a scoring strategy based on competition type
                match.ScoringStrategy = competition.Type == CompetitionType.Professional ? new ProfessionalScoringStrategy() : new AmateurScoringStrategy();
                competition.Matches.Add(match);

                _eventPublisher.Notify($"Match pairing added for {playerOne.Name} and {playerTwo.Name}");
            }

            _competitionRepository.Update(competition);
            Console.WriteLine("Matches added to competition successfully.");
        }


        public void EnterScoresForCompetitionMatches(int competitionId, ConsoleView view) {
            var competition = _competitionRepository.GetById(competitionId);
            if (competition == null) {
                Console.WriteLine("Competition not found.");
                _eventPublisher.Notify($"Failed adding scores: No such competition registered");
                return;
            }

            foreach (var match in competition.Matches) {
                Console.WriteLine($"Entering scores for Match {match.Id} between Player {match.PlayerOne.Name} and Player {match.PlayerTwo.Name}.");

                // Assuming each match has a predetermined number of series to play
                for (int i = 0; i < match.Series.Count; i++) {
                    // Call ConsoleView methods to get series scores for each player
                    var scores = view.GetSeriesScores(i + 1, match.PlayerOne.Name, match.PlayerTwo.Name);
                    match.Series[i].ScorePlayerOne = scores.Item1;
                    match.Series[i].ScorePlayerTwo = scores.Item2;
                }

                _eventPublisher.Notify($"Scores fully entered for match");
                // After updating series scores, determine the winner
                match.DetermineWinner();
                Console.WriteLine($"Winner: {match.Winner?.Name ?? "Tie"}");
            }

            _eventPublisher.Notify($"Matches fully scored for {competition.Name}");
            // Update the competition to reflect the changes
            _competitionRepository.Update(competition);
        }
    }

}
