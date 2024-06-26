using Spectre.Console;

namespace CodingTracker
{
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
            AnsiConsole.WriteLine("1. Add new coding session");
            AnsiConsole.WriteLine("2. View all coding sessions");
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

        public void DisplaySessions(List<CodingSession> sessions)
        {
            var table = new Table();
            table.AddColumn("ID");
            table.AddColumn("Start Time");
            table.AddColumn("End Time");
            table.AddColumn("Duration");

            foreach (var session in sessions)
            {
                table.AddRow(
                    session.Id.ToString(),
                    session.StartTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    session.EndTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    session.GetFormattedDuration());
            }

            AnsiConsole.Write(table);
        }

        public void DisplayMessage(string message)
        {
            AnsiConsole.WriteLine(message);
        }

        public void DisplayError(string message)
        {
            AnsiConsole.MarkupLine($"[red]Error: {message}[/]");
        }

        public void WaitForKeyPress()
        {
            AnsiConsole.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
        }

    }
}