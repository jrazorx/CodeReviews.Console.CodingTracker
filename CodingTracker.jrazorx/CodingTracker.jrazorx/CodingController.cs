using System.Diagnostics.Metrics;

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
                        await UpdateSessionAsync();
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
            DateTime startTime, endTime;

            while (true)
            {
                Console.Clear();
                startTime = _userInterface.GetDateTime("Enter start time");

                endTime = _userInterface.GetDateTime("Enter end time");

                if (!_inputValidator.IsSessionTimeRangeValid(startTime, endTime))
                {
                    _userInterface.DisplayError("End time must be after start time. Please try again.");
                    _userInterface.WaitForKeyPress();
                    continue;
                }

                if (!_inputValidator.IsSessionDurationValid(startTime, endTime))
                {
                    _userInterface.DisplayError("Get some sleep! Session duration cannot exceed 23 hours and 59 minutes. Please try again.");
                    _userInterface.WaitForKeyPress();
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

        private async Task UpdateSessionAsync()
        {
            await ViewAllSessionsAsync();

            _userInterface.DisplayTitle("Update a session");
            int sessionId = _userInterface.GetInteger("Enter the ID of the session you want to update:");
            var sessions = await _databaseManager.GetAllSessionsAsync();
            var sessionToUpdate = sessions.FirstOrDefault(s => s.Id == sessionId);

            if (sessionToUpdate == null)
            {
                _userInterface.DisplayError("Session not found.");
                return;
            }

            DateTime startTime, endTime;

            while (true)
            {
                startTime = _userInterface.GetDateTime("Enter new start time", sessionToUpdate.StartTime);
                endTime = _userInterface.GetDateTime("Enter new end time", sessionToUpdate.EndTime);

                if (!_inputValidator.IsSessionTimeRangeValid(startTime, endTime))
                {
                    _userInterface.DisplayError("End time must be after start time. Please try again.");
                    continue;
                }

                if (!_inputValidator.IsSessionDurationValid(startTime, endTime))
                {
                    _userInterface.DisplayError("Session duration cannot exceed 23 hours and 59 minutes. Please try again.");
                    continue;
                }

                break;
            }

            sessionToUpdate.StartTime = startTime;
            sessionToUpdate.EndTime = endTime;

            await _databaseManager.UpdateSessionAsync(sessionToUpdate);
            _userInterface.DisplayMessage("Session updated successfully.");
        }
    }
}