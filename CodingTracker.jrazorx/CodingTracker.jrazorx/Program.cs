using CodingTracker.jrazorx.Controllers;
using CodingTracker.jrazorx.Repositories;
using CodingTracker.jrazorx.Services;
using CodingTracker.jrazorx.UI;
using CodingTracker.jrazorx.Helpers;

namespace CodingTracker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var codingSessionRepository = new CodingSessionRepository();
            var codingGoalRepository = new CodingGoalRepository();
            var codingSessionService = new CodingSessionService(codingSessionRepository);
            var goalService = new CodingGoalService(codingGoalRepository, codingSessionService);
            var userInterface = new UserInterface();
            var inputValidator = new InputValidator();
            var codingController = new CodingSessionController(codingSessionService, goalService, userInterface, inputValidator);

            await codingSessionService.InitializeDatabaseAsync();
            await goalService.InitializeDatabaseAsync();
            await codingController.RunAsync();
        }
    }
}