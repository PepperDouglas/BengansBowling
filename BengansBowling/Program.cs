using BengansBowling.Controllers;
using BengansBowling.Repositories;
using BengansBowling.Views;
using Microsoft.EntityFrameworkCore;

namespace BengansBowling
{
    class Program
    {
        static async Task Main(string[] args) {

            // Configure DbContextOptions for BowlingAlleyContext
            var optionsBuilder = new DbContextOptionsBuilder<BowlingAlleyContext>();
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-HUERL9P;Database=BengansBowling;Trusted_Connection=True;TrustServerCertificate=True;");

            // Create an instance of BowlingAlleyContext
            using (var context = new BowlingAlleyContext(optionsBuilder.Options)) { 
            
            



                var memberRepository = new MemberRepository(context);
                var competitionRepository = new CompetitionRepository(context);
                var trackRepository = new TrackRepository(context);
                var matchRepository = new MatchRepository(context);
                var memberController = new MemberController(memberRepository);
                var competitionController = new CompetitionController(competitionRepository, memberRepository, trackRepository, matchRepository);
                var view = new ConsoleView();

                bool exit = false;
                while (!exit) {
                    int choice = view.ShowMainMenu();
                    switch (choice) {
                        case 1:
                        await ManageMembers(view, memberController);
                        break;
                        case 2:
                        await view.ShowCompetitionManagementMenu(competitionController);
                        break;
                        case 3:
                        view.ManageTracks(trackRepository);
                        break;
                        case 4:
                        exit = true;
                        break;
                        default:
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
                    }
                }
            }

            static async Task ManageMembers(ConsoleView view, MemberController memberController) {
                bool back = false;
                while (!back) {
                    int choice = view.ShowMemberMenu();
                    switch (choice) {
                        case 1:
                        var (Name, Address, Telephone) = view.GetMemberDetails();
                        memberController.AddMember(Name, Address, Telephone);
                        break;
                        case 2:
                        var (name, address, telephone) = view.GetMemberDetails();
                        int updateId = view.GetId("member");
                        memberController.UpdateMember(updateId, name, address, telephone);
                        break;
                        case 3:
                        int deleteId = view.GetId("member");
                        memberController.DeleteMember(deleteId);
                        break;
                        case 4:
                        await memberController.ListMembers();
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

            static void ManageCompetitions(ConsoleView view, CompetitionController competitionController) {
                bool back = false;
                while (!back) {
                    int choice = view.ShowCompetitionMenu();
                    switch (choice) {
                        case 1:
                        var (Name, Type) = view.GetCompetitionDetails();
                        competitionController.AddCompetitionAsync(Name, Type);
                        break;
                        case 2:
                        string updateName = view.GetCompetitionName();
                        int updateId = view.GetId("competition");
                        //competitionController.UpdateCompetition(updateId, updateName);
                        break;
                        case 3:
                        int deleteId = view.GetId("competition");
                        //competitionController.DeleteCompetition(deleteId);
                        break;
                        case 4:
                        competitionController.ListCompetitions();
                        break;
                        case 5: // New case for entering scores for competition matches
                        EnterScoresForCompetitionMatches(view, competitionController);
                        break;
                        case 6: // New case for adding matches to a competition
                        AddMatchesToCompetition(view, competitionController);
                        break;
                        case 7:
                        back = true;
                        break;
                        default:
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
                    }
                }
            }

            static void EnterScoresForCompetitionMatches(ConsoleView view, CompetitionController competitionController) {
                Console.Write("Enter the ID of the competition to enter scores for: ");
                int competitionId = Convert.ToInt32(Console.ReadLine());

                // Call the corresponding controller method to enter scores for matches
                competitionController.EnterScoresForCompetitionMatches(competitionId, view);
            }

            static void AddMatchesToCompetition(ConsoleView view, CompetitionController competitionController) {
                // Implement the method to add matches to a competition here
                // You can reuse the existing method from ConsoleView or create a new one
            }
        }
    }

}
