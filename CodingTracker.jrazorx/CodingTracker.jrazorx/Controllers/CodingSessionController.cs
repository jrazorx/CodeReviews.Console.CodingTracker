using System.Diagnostics;
using CodingTracker.jrazorx.Models;
using CodingTracker.jrazorx.Services;
using CodingTracker.jrazorx.UI;
using CodingTracker.jrazorx.Helpers;
using CodingTracker.jrazorx.Enums;

namespace CodingTracker.jrazorx.Controllers
{
    public class CodingSessionController
    {
        private readonly CodingSessionService _sessionService;
        private readonly UserInterface _userInterface;
        private readonly InputValidator _inputValidator;
        private readonly Stopwatch _stopwatch;

        public CodingSessionController(CodingSessionService sessionService, UserInterface userInterface, InputValidator inputValidator)
        {
            _sessionService = sessionService;
            _userInterface = userInterface;
            _inputValidator = inputValidator;
            _stopwatch = new Stopwatch();
        }

        public async Task RunAsync()
        {
            bool exitRequested = false;

            while (!exitRequested)
            {
                MenuOption menuChoice = _userInterface.GetMenuChoice();

                switch (menuChoice)
                {
                    case MenuOption.StartLiveSession:
                        await StartLiveSessionAsync();
                        break;
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
                        await DeleteSessionAsync();
                        break;
                    case MenuOption.ViewReports:
                        await ViewReportsAsync();
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

        private async Task StartLiveSessionAsync()
        {
            _userInterface.DisplayMessage("Stopwatch session started. Press any key to stop.");
            DateTime startTime = DateTime.Now;
            _stopwatch.Start();

            _userInterface.DisplayLiveSessionStopwatch(_stopwatch);

            Console.ReadKey(true);

            _stopwatch.Stop();
            DateTime endTime = DateTime.Now;

            TimeSpan duration = _stopwatch.Elapsed;
            _userInterface.DisplayMessage($"Session ended. Duration: {duration.ToString(@"hh\:mm\:ss")}");

            if (duration.TotalMinutes >= 1)
            {
                var session = new CodingSession { StartTime = startTime, EndTime = endTime };
                await _sessionService.InsertSessionAsync(session);
                _userInterface.DisplayMessage("Coding session [green]added successfully.[/]");
            }
            else
                _userInterface.DisplayMessage("Coding session [red]not recorded.[/] Minimum duration is 1 minute.");

            _stopwatch.Reset();
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

            await _sessionService.InsertSessionAsync(session);
            _userInterface.DisplayMessage("Coding session added successfully.");
        }

        private async Task<List<CodingSession>> FetchSessionsAsync(string period = "all", bool ascending = true)
        {
            if (period.ToLower() == "all")
            {
                return await _sessionService.GetSessionsAsync(null, null, ascending);
            }
            return await _sessionService.GetSessionsByPeriodAsync(period, ascending);
        }

        private void DisplaySessions(List<CodingSession> sessions, bool clearConsoleAtStart = true)
        {
            if (clearConsoleAtStart)
                Console.Clear();
            _userInterface.DisplaySessions(sessions);
        }

        private async Task<List<CodingSession>> FetchAndDisplaySessionsAsync(bool clearConsoleAtStart = true)
        {
            var sessions = await FetchSessionsAsync();
            DisplaySessions(sessions, clearConsoleAtStart);
            return sessions;
        }


        private async Task ViewAllSessionsAsync()
        {
            string period = "all";
            bool ascending = true;
            bool exit = false;

            while (!exit)
            {
                var sessions = await FetchSessionsAsync(period, ascending);
                _userInterface.DisplaySessions(sessions);

                var choice = _userInterface.GetSessionViewMenuChoice();
                switch (choice)
                {
                    case SessionViewOption.ChangeFilter:
                        period = _userInterface.GetFilterPeriod();
                        break;
                    case SessionViewOption.ChangeSortOrder:
                        ascending = _userInterface.GetSortOrder();
                        break;
                    case SessionViewOption.Back:
                        exit = true;
                        break;
                }
            }
        }

        private async Task UpdateSessionAsync()
        {
            var sessions = await FetchAndDisplaySessionsAsync();

            _userInterface.DisplayTitle("Update a session");
            int sessionId = _userInterface.GetInteger("Enter the ID of the session you want to update:");
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

            await _sessionService.UpdateSessionAsync(sessionToUpdate);
            _userInterface.DisplayMessage("Session updated successfully.");
        }

        private async Task DeleteSessionAsync()
        {
            var sessions = await FetchAndDisplaySessionsAsync();

            _userInterface.DisplayTitle("Delete a session");
            int sessionId = _userInterface.GetInteger("Enter the ID of the session you want to delete:");
            var sessionToDelete = sessions.FirstOrDefault(s => s.Id == sessionId);

            if (sessionToDelete == null)
            {
                _userInterface.DisplayError("Session not found.");
                return;
            }

            bool confirmDelete = _userInterface.GetConfirmation($"Are you sure you want to delete the session from [green]{sessionToDelete.StartTime}[/] to [green]{sessionToDelete.EndTime}[/]?");

            if (confirmDelete)
            {
                await _sessionService.DeleteSessionAsync(sessionId);
                _userInterface.DisplayMessage("Session deleted successfully.");
            }
            else
            {
                _userInterface.DisplayMessage("Deletion cancelled.");
            }
        }

        private async Task ViewReportsAsync()
        {
            string period = _userInterface.GetReportPeriod();
            var report = await _sessionService.GenerateReportAsync(period);
            _userInterface.DisplayReport(report, period);
        }
    }
}