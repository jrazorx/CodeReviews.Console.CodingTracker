namespace CodingTracker
{
    public enum MenuOption
    {
        Exit = 0,
        AddNewSession,
        ViewAllSessions,
        UpdateSession,
        DeleteSession,
        ViewStatistics
    }

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
            bool exitRequested = false;

            while (!exitRequested)
            {
                _userInterface.DisplayMenu();

                MenuOption menuChoice = GetValidMenuChoice();

                switch (menuChoice)
                {
                    case MenuOption.AddNewSession:
                        await AddNewSessionAsync();
                        break;
                    case MenuOption.ViewAllSessions:
                        await ViewAllSessionsAsync();
                        break;
                    case MenuOption.UpdateSession:
                        //await UpdateSessionAsync();
                        break;
                    case MenuOption.DeleteSession:
                        //await DeleteSessionAsync();
                        break;
                    case MenuOption.ViewStatistics:
                        //await ViewStatisticsAsync();
                        break;
                    case MenuOption.Exit:
                        exitRequested = true;
                        break;
                }

                if (!exitRequested)
                {
                    _userInterface.WaitForKeyPress();
                }
            }
        }

        private MenuOption GetValidMenuChoice()
        {
            MenuOption choice;
            while (true)
            {
                Console.Write("\nEnter your choice: ");
                if (Enum.TryParse(Console.ReadLine(), true, out choice) &&
                    Enum.IsDefined(typeof(MenuOption), choice))
                {
                    return choice;
                }
                Console.WriteLine("Invalid input. Please enter a number between 0 and 5.");
            }
        }

        private async Task AddNewSessionAsync()
        {
            Console.Clear();
            DateTime startTime, endTime;

            while (true)
            {
                var startTimeInput = _userInterface.GetUserInput("Enter start time (yyyy-MM-dd HH:mm:ss): ");
                if (!_inputValidator.TryParseDateTime(startTimeInput, out startTime))
                {
                    _userInterface.DisplayError("Invalid date/time format. Please try again.");
                    continue;
                }

                var endTimeInput = _userInterface.GetUserInput("Enter end time (yyyy-MM-dd HH:mm:ss): ");
                if (!_inputValidator.TryParseDateTime(endTimeInput, out endTime))
                {
                    _userInterface.DisplayError("Invalid date/time format. Please try again.");
                    continue;
                }

                if (!_inputValidator.ValidateTimeRange(startTime, endTime))
                {
                    _userInterface.DisplayError("End time must be after start time. Please try again.");
                    continue;
                }

                break;
            }

            var session = new CodingSession { StartTime = startTime, EndTime = endTime };

            await _databaseManager.InsertSessionAsync(session);
            _userInterface.DisplayMessage("Coding session added successfully.");
        }

        private async Task ViewAllSessionsAsync(bool clearConsoleAtStart = true)
        {
            if (clearConsoleAtStart)
                Console.Clear();

            var sessions = await _databaseManager.GetAllSessionsAsync();
            _userInterface.DisplaySessions(sessions);
        }
    }
}