namespace CodingTracker
{
    public class CodingController
    {
        private readonly DatabaseManager _databaseManager;
        private readonly UserInterface _userInterface;
        private readonly InputValidator _inputValidator;

        public CodingController(DatabaseManager databaseManager, UserInterface userInterface, InputValidator inputValidator)
        {
            _databaseManager = databaseManager;
            _userInterface = userInterface;
            _inputValidator = inputValidator;
        }

        public async Task RunAsync()
        {
            while (true)
            {
                _userInterface.DisplayMenu();
                var choice = _userInterface.GetUserInput("Enter your choice: ");

                switch (choice)
                {
                    case "1":
                        //await ViewAllSessionsAsync();
                        break;
                    case "2":
                        //await AddNewSessionAsync();
                        break;
                    case "3":
                        //await UpdateSessionAsync();
                        break;
                    case "4":
                        //await DeleteSessionAsync();
                        break;
                    case "5":
                        //await ViewStatisticsAsync();
                        break;
                    case "0":
                        _userInterface.DisplayMessage("Thank you for using Coding Tracker. Goodbye!");
                        return;
                    default:
                        _userInterface.DisplayError("Invalid choice. Please try again.");
                        break;
                }

                _userInterface.GetUserInput("Press Enter to continue...");
            }
        }
    }
}