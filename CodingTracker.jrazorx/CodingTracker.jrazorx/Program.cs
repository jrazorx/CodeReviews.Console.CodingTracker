namespace CodingTracker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var databaseManager = new DatabaseManager();
            var userInterface = new UserInterface();
            var inputValidator = new InputValidator();
            var codingController = new CodingController(databaseManager, userInterface, inputValidator);

            await databaseManager.InitializeDatabaseAsync();
            await codingController.RunAsync();
        }
    }
}