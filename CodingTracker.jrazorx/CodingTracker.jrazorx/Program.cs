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
            var repository = new CodingSessionRepository();
            var sessionService = new CodingSessionService(repository);
            var userInterface = new UserInterface();
            var inputValidator = new InputValidator();
            var codingController = new CodingSessionController(sessionService, userInterface, inputValidator);

            await sessionService.InitializeDatabaseAsync();
            await codingController.RunAsync();
        }
    }
}