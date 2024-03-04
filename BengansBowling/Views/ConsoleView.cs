using BengansBowling.Controllers;
using BengansBowling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BengansBowling.Views
{
    public class ConsoleView
    {
        public int ShowMainMenu() {
            Console.WriteLine("Bowling Alley Management System");
            Console.WriteLine("1. Manage Members");
            Console.WriteLine("2. Manage Competitions");
            Console.WriteLine("3. Manage Tracks");
            Console.WriteLine("4. Exit");
            Console.Write("Select an option: ");
            int choice = Convert.ToInt32(Console.ReadLine());
            return choice;
        }

        public int ShowMemberMenu() {
            Console.WriteLine("Manage Members");
            Console.WriteLine("1. Add Member");
            Console.WriteLine("2. Update Member");
            Console.WriteLine("3. Delete Member");
            Console.WriteLine("4. List Members");
            Console.WriteLine("5. Back to Main Menu");
            Console.Write("Select an option: ");
            int choice = Convert.ToInt32(Console.ReadLine());
            return choice;
        }

        public int ShowCompetitionMenu() {
            Console.WriteLine("Manage Competitions");
            Console.WriteLine("1. Add Competition");
            Console.WriteLine("2. List Competitions");
            Console.WriteLine("3. Add Matches to Competition");
            Console.WriteLine("4. Enter Scores For Competition Matches");
            Console.WriteLine("5. Back to Main Menu");
            Console.Write("Select an option: ");
            int choice = Convert.ToInt32(Console.ReadLine());
            return choice;
        }

        public int ShowTrackMenu() {
            Console.WriteLine("Manage Tracks");
            Console.WriteLine("1. Add Track");
            Console.WriteLine("2. Back to Main Menu");
            Console.Write("Select an option: ");
            int choice = Convert.ToInt32(Console.ReadLine());
            return choice;
        }

        public void ManageTracks(TrackRepository trackRepository) {
            bool back = false;
            while (!back) {
                int choice = ShowTrackMenu();
                switch (choice) {
                    case 1:
                    AddTrack(trackRepository);
                    break;
                    case 2:
                    back = true;
                    break;
                    default:
                    Console.WriteLine("Invalid choice, please try again.");
                    break;
                }
            }
        }

            public async void AddTrack(TrackRepository trackRepository) {
                Console.Write("Enter track name: ");
                string name = Console.ReadLine();

               
                var track = new Track(name);
                await trackRepository.AddAsync(track);

                Console.WriteLine("Track added successfully.");
            }

            public async Task ShowCompetitionManagementMenu(CompetitionController competitionController) {
            bool back = false;
            while (!back) {
                int choice = ShowCompetitionMenu();
                switch (choice) {
                    case 1:
                    CreateCompetition(competitionController);
                    break;
                    case 2:
                    await competitionController.ListCompetitions();
                    break;
                    case 3:
                    AddMatchesToCompetition(competitionController);
                    break;
                    case 4:
                    Console.Write("Enter the ID of the competition to enter scores for: ");
                    int competitionId = Convert.ToInt32(Console.ReadLine());
                    await competitionController.EnterScoresForCompetitionMatches(competitionId, this);
                    break;
                    case 5:
                    back = true;
                    break;
                    default:
                    Console.WriteLine("Invalid choice, please try again.");
                    break;
                }
            }
        }

        public void AddMatchesToCompetition(CompetitionController competitionController) {
            Console.Write("Enter the ID of the competition to add matches to: ");
            int competitionId = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter the number of players (even number): ");
            int numberOfPlayers = Convert.ToInt32(Console.ReadLine());
            if (numberOfPlayers % 2 != 0) {
                Console.WriteLine("The number of players must be even.");
                return;
            }

            List<int> memberIds = new List<int>();
            for (int i = 0; i < numberOfPlayers; i++) {
                Console.Write($"Enter player {i + 1} ID: ");
                int memberId = Convert.ToInt32(Console.ReadLine());
                memberIds.Add(memberId);
            }

            competitionController.AddMatchesToCompetition(competitionId, memberIds);
        }


        public (string Name, CompetitionType Type) GetCompetitionDetails() {
            Console.Write("Enter competition name: ");
            string name = Console.ReadLine();
            Console.WriteLine("Select competition type:");
            Console.WriteLine("1. Amateur");
            Console.WriteLine("2. Professional");
            Console.Write("Select an option: ");
            int typeChoice = Convert.ToInt32(Console.ReadLine());

            CompetitionType type = typeChoice == 1 ? CompetitionType.Amateur : CompetitionType.Professional;

            return (name, type);
        }

        public (int, int) GetSeriesScores(int seriesNumber, string playerOneName, string playerTwoName) {
            Console.WriteLine($"Series {seriesNumber}:");
            Console.Write($"Enter score for {playerOneName}: ");
            int scorePlayerOne = Convert.ToInt32(Console.ReadLine());
            Console.Write($"Enter score for {playerTwoName}: ");
            int scorePlayerTwo = Convert.ToInt32(Console.ReadLine());

            return (scorePlayerOne, scorePlayerTwo);
        }

        public void CreateCompetition(CompetitionController competitionController) {
            var (Name, Type) = GetCompetitionDetails();
            competitionController.AddCompetitionAsync(Name, Type);
        }

        public (string Name, string Address, string Telephone) GetMemberDetails() {
            Console.Write("Enter member name: ");
            string name = Console.ReadLine();

            Console.Write("Enter member address: ");
            string address = Console.ReadLine();

            Console.Write("Enter member telephone: ");
            string telephone = Console.ReadLine();

            return (name, address, telephone);
        }

        public string GetCompetitionName() {
            Console.Write("Enter competition name: ");
            return Console.ReadLine();
        }

        public int GetId(string entityType) {
            Console.Write($"Enter {entityType} ID: ");
            return Convert.ToInt32(Console.ReadLine());
        }
    }

}
