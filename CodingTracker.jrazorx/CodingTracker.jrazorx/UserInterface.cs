using Spectre.Console;
using System.Linq;
using System.Globalization;

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

        public int GetInteger(string prompt)
        {
            return AnsiConsole.Prompt(
                new TextPrompt<int>(prompt)
                    .PromptStyle("green")
                    .ValidationErrorMessage("[red]Please enter a valid integer.[/]")
                    .Validate(value =>
                    {
                        return value > 0 ? ValidationResult.Success() : ValidationResult.Error("Value must be greater than 0.");
                    }));
        }

        public DateTime GetDateTime(string prompt, DateTime? defaultDateTime = null)
        {
            DisplayTitle(prompt);

            var referenceDateTime = defaultDateTime ?? DateTime.Now;

            // Year selection
            var year = AnsiConsole.Prompt(
                new TextPrompt<int>("Enter the [green]year[/] [blue][[0-9999]][/] ")
                    .DefaultValue(referenceDateTime.Year)
                    .PromptStyle("yellow")
                    .ValidationErrorMessage("[red]That's not a valid year[/]")
                    .Validate(year =>
                    {
                        return year switch
                        {
                            < 0 => ValidationResult.Error("[red]The year must be at least 0[/]"),
                            > 9999 => ValidationResult.Error("[red]The year must be less than or equal to 9999[/]"),
                            _ => ValidationResult.Success(),
                        };
                    })
            );

            // Month selection
            var monthPrompt = new TextPrompt<int>("Enter the [green]month[/] [blue][[1-12]][/] ")
                .PromptStyle("yellow")
                .ValidationErrorMessage("[red]That's not a valid month[/]")
                .Validate(month =>
                {
                    return month switch
                    {
                        < 1 => ValidationResult.Error("[red]The month must be 1 or greater[/]"),
                        > 12 => ValidationResult.Error("[red]The month must be 12 or less[/]"),
                        _ => ValidationResult.Success(),
                    };
                });

            if (year == referenceDateTime.Year)
            {
                monthPrompt = monthPrompt.DefaultValue(referenceDateTime.Month);
            }

            var month = AnsiConsole.Prompt(monthPrompt);

            // Day selection
            int maxDays = DateTime.DaysInMonth(year, month);
            var dayPrompt = new TextPrompt<int>($"Enter the [green]day[/] [blue][[1-{maxDays}]][/] ")
                .PromptStyle("yellow")
                .ValidationErrorMessage("[red]That's not a valid day[/]")
                .Validate(day =>
                {
                    if (day < 1)
                    {
                        return ValidationResult.Error("[red]The day must be 1 or greater[/]");
                    }
                    else if (day > maxDays)
                    {
                        return ValidationResult.Error($"[red]The day must be {maxDays} or less for {CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month)} {year}[/]");
                    }
                    return ValidationResult.Success();
                });

            if (year == referenceDateTime.Year && month == referenceDateTime.Month)
            {
                dayPrompt = dayPrompt.DefaultValue(referenceDateTime.Day);
            }

            var day = AnsiConsole.Prompt(dayPrompt);

            // Hours selection
            var hoursPrompt = new TextPrompt<int>("Enter the [green]hour[/] [blue][[0-23]][/] ")
                .PromptStyle("yellow")
                .ValidationErrorMessage("[red]That's not a valid hour[/]")
                .Validate(hours =>
                {
                    return hours switch
                    {
                        < 0 => ValidationResult.Error("[red]The hour must be 0 or greater[/]"),
                        > 23 => ValidationResult.Error("[red]The hour must be 23 or less[/]"),
                        _ => ValidationResult.Success(),
                    };
                });

            if (year == referenceDateTime.Year && month == referenceDateTime.Month && day == referenceDateTime.Day)
            {
                hoursPrompt = hoursPrompt.DefaultValue(referenceDateTime.Hour);
            }

            var hours = AnsiConsole.Prompt(hoursPrompt);

            // Minutes selection
            var minutesPrompt = new TextPrompt<int>("Enter the [green]minute[/] [blue][[0-59]][/] ")
                .PromptStyle("yellow")
                .ValidationErrorMessage("[red]That's not a valid minute[/]")
                .Validate(minutes =>
                {
                    return minutes switch
                    {
                        < 0 => ValidationResult.Error("[red]The minutes must be 0 or greater[/]"),
                        > 59 => ValidationResult.Error("[red]The minutes must be 59 or less[/]"),
                        _ => ValidationResult.Success(),
                    };
                });

            if (year == referenceDateTime.Year && month == referenceDateTime.Month && day == referenceDateTime.Day && hours == referenceDateTime.Hour)
            {
                minutesPrompt = minutesPrompt.DefaultValue(referenceDateTime.Minute);
            }

            var minutes = AnsiConsole.Prompt(minutesPrompt);


            return new DateTime(year, month, day, hours, minutes, 0);
        }

        public void DisplaySessions(List<CodingSession> sessions)
        {
            DisplayTitle("List of all sessions");

            var table = new Table();
            table.AddColumn("ID");
            table.AddColumn("Start Time");
            table.AddColumn("End Time");
            table.AddColumn("Duration");

            foreach (var session in sessions)
            {
                table.AddRow(
                    session.Id.ToString(),
                    session.StartTime.ToString("yyyy-MM-dd HH:mm"),
                    session.EndTime.ToString("yyyy-MM-dd HH:mm"),
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

        public void DisplayTitle(string title)
        {
            var rule = new Rule("[blue]" + title + "[/]");
            rule.LeftJustified();
            AnsiConsole.Write(rule);
        }

        public void WaitForKeyPress()
        {
            AnsiConsole.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
        }

    }
}