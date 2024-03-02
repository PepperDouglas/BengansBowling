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
        private readonly IMatchRepository _matchRepository;
        private readonly ApplicationEventPublisher _eventPublisher;

        public CompetitionController(ICompetitionRepository competitionRepository, IMemberRepository memberRepository, ITrackRepository trackRepository, IMatchRepository matchRepository) {
            _competitionRepository = competitionRepository;
            _memberRepository = memberRepository;
            _trackRepository = trackRepository; // Initialize the track repository
            _matchRepository = matchRepository;

            _eventPublisher = new ApplicationEventPublisher();
            var loggerObserver = new LoggerObserver();
            _eventPublisher.Attach(loggerObserver);
        }

        public async Task AddCompetitionAsync(string name, CompetitionType type) {
            // Use the CompetitionFactory to create a competition of the specified type
            var competition = CompetitionFactory.CreateCompetition(type, name);

            // No need to set the ID manually; the database will generate it
            await _competitionRepository.AddAsync(competition); // Ensure AddAsync method exists and is awaited
            Console.WriteLine($"{type} competition added successfully: {name}");
        }


        public async Task UpdateCompetitionAsync(int id, string name) {
            var competition = await _competitionRepository.GetByIdAsync(id);
            if (competition != null) {
                competition.Name = name;
                await _competitionRepository.UpdateAsync(competition);

                Console.WriteLine("Competition updated successfully.");
            } else {
                Console.WriteLine("Competition not found.");
            }
        }


        //public void DeleteCompetition(int id) {
        //    _competitionRepository.Delete(id);
        //    Console.WriteLine("Competition deleted successfully.");
        //}

        public async Task ListCompetitions() {
            var competitions = await _competitionRepository.GetAllAsync();
            foreach (var competition in competitions) {
                Console.WriteLine($"ID: {competition.Id}, Name: {competition.Name}");
            }
            Console.Write("Enter the ID of the competition to view details: ");
            int selectedId = Convert.ToInt32(Console.ReadLine());
            await DisplayCompetitionDetails(selectedId);
        }

        public async Task DisplayCompetitionDetails(int competitionId) {
            var competition = await _competitionRepository.GetByIdAsync(competitionId);
            if (competition == null) {
                Console.WriteLine("Competition not found.");
                return;
            }

            Console.WriteLine($"Competition: {competition.Name}");
            foreach (var match in competition.Matches) {
                string result = match.Winner != null ? $"Winner: {match.Winner.Name}" : "No winner yet (or tie)";
                //Console.WriteLine($"Match {match.Id} between {match.PlayerOne.Name} and {match.PlayerTwo.Name}: {result}");
                Console.WriteLine($"Match {match.Id}: {result}");
            }
        }

        //This one adds matches to competitions
        public async Task AddMatchesToCompetition(int competitionId, List<int> memberIds) {
            var competition = await _competitionRepository.GetByIdAsync(competitionId);
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

            var availableTracks = await _trackRepository.GetAllAsync();
            var trackList = availableTracks.ToList();
            if (trackList.Count < memberIds.Count / 2) {
                Console.WriteLine("Not enough available tracks.");
                _eventPublisher.Notify($"Failed adding matches: Not enough available tracks");
                return;
            }

            int seriesCount = competition.Type == CompetitionType.Amateur ? 2 : 3; // Determine series count based on competition type

            for (int i = 0; i < memberIds.Count; i += 2) {
                var playerOne = await _memberRepository.GetByIdAsync(memberIds[i]);
                var playerTwo = await _memberRepository.GetByIdAsync(memberIds[i + 1]);
                var track = trackList[i / 2];

                // Create the Match without manually setting an ID
                var match = new Match
                {
                    PlayerOne = playerOne,
                    PlayerTwo = playerTwo,
                    Track = track,
                    Series = new List<Series>(),
                    // Optionally set the competition ID if the Match entity has a foreign key for Competition
                    CompetitionId = competitionId
                };

                // Initialize the Series list based on the determined series count, ID will be auto-generated
                for (int seriesIndex = 0; seriesIndex < seriesCount; seriesIndex++) {
                    match.Series.Add(new Series(0, 0 ));
                }

                // Assign a scoring strategy based on competition type
                match.ScoringStrategy = competition.Type == CompetitionType.Professional ? new ProfessionalScoringStrategy() : new AmateurScoringStrategy();

                // Use _matchRepository to add the match
                await _matchRepository.AddAsync(match);

                _eventPublisher.Notify($"Match pairing added for {playerOne.Name} and {playerTwo.Name}");
            }

            Console.WriteLine("Matches added to competition successfully.");
        }



        public async Task EnterScoresForCompetitionMatches(int competitionId, ConsoleView view) {
            //this one needs to include series
            var competition = await _competitionRepository.GetByIdAsync(competitionId);
            if (competition == null) {
                Console.WriteLine("Competition not found.");
                _eventPublisher.Notify($"Failed adding scores: No such competition registered");
                return;
            }

            // Select the scoring strategy based on the competition type
            IScoringStrategy scoringStrategy = competition.Type == CompetitionType.Professional
                ? new ProfessionalScoringStrategy()
                : new AmateurScoringStrategy();

            foreach (var match in competition.Matches) {
                Console.WriteLine($"Entering scores for Match {match.Id} between Player {match.PlayerOne.Name} and Player {match.PlayerTwo.Name}.");
                //For only ID is got
                //Console.WriteLine($"Entering scores for Match {match.Id} between PlayerID {match.PlayerOneId} and PlayerID {match.PlayerTwoId}.");

                // Assuming each match has a predetermined number of series to play
                for (int i = 0; i < match.Series.Count; i++) {
                    // Call ConsoleView methods to get series scores for each player
                    //Console.WriteLine(match.PlayerOne.Name);
                    var scores = view.GetSeriesScores(i + 1, match.PlayerOne.Name, match.PlayerTwo.Name);
                    match.Series[i].ScorePlayerOne = scores.Item1;
                    match.Series[i].ScorePlayerTwo = scores.Item2;
                }

                _eventPublisher.Notify($"Scores fully entered for match");
                // After updating series scores, determine the winner
                // Apply the scoring strategy to determine the winner
                match.ScoringStrategy = scoringStrategy;
                match.DetermineWinner();
                Console.WriteLine($"Winner: {match.Winner?.Name ?? "Tie"}");
                
            }
            // or should this be one up?
            await _competitionRepository.UpdateAsync(competition);
            _eventPublisher.Notify($"Matches fully scored for {competition.Name}");
            // Update the competition to reflect the changes
        }
    }

}
