using Spectre.Console;

namespace CodingTracker
{
    public enum MenuOption
    {
        Exit = 0,
        CreateHabitType,
        ViewHabitTypes,
        UpdateHabitType,
        DeleteHabitType,
        CreateHabit,
        ViewHabits,
        UpdateHabit,
        DeleteHabit,
        GenerateYearlyReport
    }

    public class UserInterface
    {
        public void DisplayMenu()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new FigletText("Coding Tracker")
                    .Centered()
                    .Color(Color.Aqua));

            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine("1. View all coding sessions");
            AnsiConsole.WriteLine("2. Add new coding session");
            AnsiConsole.WriteLine("3. Update coding session");
            AnsiConsole.WriteLine("4. Delete coding session");
            AnsiConsole.WriteLine("5. View coding statistics");
            AnsiConsole.WriteLine("0. Exit");
            AnsiConsole.WriteLine();
        }

        public string GetUserInput(string prompt)
        {
            return AnsiConsole.Ask<string>(prompt);
        }

        public void DisplayMessage(string message)
        {
            AnsiConsole.WriteLine(message);
        }

        public void DisplayError(string message)
        {
            AnsiConsole.MarkupLine($"[red]Error: {message}[/]");
        }
    }
}