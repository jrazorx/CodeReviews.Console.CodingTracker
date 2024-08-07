﻿namespace CodingTracker.jrazorx.Enums
{
    public static class MenuOptionExtensions
    {
        private static readonly Dictionary<MenuOption, string> DisplayTexts = new()
    {
        { MenuOption.StartLiveSession, "Start a live session" },
        { MenuOption.AddNewSession, "Add a new session" },
        { MenuOption.ViewAllSessions, "View all sessions" },
        { MenuOption.UpdateSession, "Update a session" },
        { MenuOption.DeleteSession, "Delete a session" },
        { MenuOption.ViewReports, "View Reports" },
        { MenuOption.ManageCodingGoals, "Manage Coding Goals" },
        { MenuOption.Exit, "Exit" }
    };

        public static string GetDisplayText(this MenuOption option)
        {
            return DisplayTexts.TryGetValue(option, out var text) ? text : option.ToString();
        }
    }
}