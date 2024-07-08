using Spectre.Console;
using System.Globalization;
using System.Diagnostics;
using CodingTracker.jrazorx.Models;
using CodingTracker.jrazorx.Enums;

namespace CodingTracker.jrazorx.UI
{
    public class UserInterface
    {
        public MenuOption GetMenuChoice()
        {
            AnsiConsole.Clear();

            AnsiConsole.Write(
                new FigletText("Coding Tracker")
                    .Centered()
                    .Color(Color.Aqua));

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<MenuOption>()
                    .Title("[green]MAIN MENU[/]")
                    .PageSize(10)
                    .AddChoices(Enum.GetValues<MenuOption>())
                    .UseConverter(option => option.GetDisplayText())
            );

            return choice;
        }

        public bool GetConfirmation(string prompt)
        {
            return AnsiConsole.Confirm(prompt);
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
                        return value > 0 ? ValidationResult.Success() : ValidationResult.Error("[red]Value must be greater than 0.[/]");
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

        private DateTime GetDate(string prompt, DateTime? defaultDateTime = null)
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

            return new DateTime(year, month, day);
        }

        public void DisplaySessions(List<CodingSession> sessions)
        {
            AnsiConsole.Clear();

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

        public SessionViewOption GetSessionViewMenuChoice()
        {
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine("Use the menu below to filter or sort the sessions.");

            return AnsiConsole.Prompt(
                new SelectionPrompt<SessionViewOption>()
                    .Title("Select an option:")
                    .AddChoices(Enum.GetValues(typeof(SessionViewOption)).Cast<SessionViewOption>())
                    .UseConverter(o => o.GetDisplayText()));
        }

        public string GetFilterPeriod()
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select the period to filter:")
                    .AddChoices(new[] { "All", "Week", "Month", "Year" }));
        }

        public bool GetSortOrder()
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<bool>()
                    .Title("Select the sort order:")
                    .AddChoices(new[] { true, false })
                    .UseConverter(b => b ? "Ascending" : "Descending"));
        }

        public void DisplayMessage(string message)
        {
            AnsiConsole.MarkupLine(message);
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

        public void DisplayLiveSessionStopwatch(Stopwatch stopwatch)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("Coding Timer").Centered().Color(Color.Aqua));
            AnsiConsole.WriteLine();

            AnsiConsole.Live(CreateStopwatchPanel(stopwatch))
                .Start(ctx =>
                {
                    while (!Console.KeyAvailable)
                    {
                        ctx.UpdateTarget(CreateStopwatchPanel(stopwatch));
                        Thread.Sleep(100);
                    }
                });
        }

        private Panel CreateStopwatchPanel(Stopwatch stopwatch)
        {
            var elapsed = stopwatch.Elapsed;
            var timeString = $"{elapsed.Hours:D2}:{elapsed.Minutes:D2}:{elapsed.Seconds:D2}";

            return new Panel(new Markup($"[bold green]{timeString}[/]\n\n[yellow]Press any key to stop the timer[/]"))
                .Header("Elapsed Time")
                .Expand()
                .BorderColor(Color.Blue);
        }

        public string GetReportPeriod()
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select the period for the report:")
                    .AddChoices(new[] { "Week", "Month", "Year", "All Time" }));
        }

        public void DisplayReport((TimeSpan TotalTime, TimeSpan AverageTime, int SessionCount) report, string period)
        {
            AnsiConsole.Clear();
            DisplayTitle($"Coding Report for {period}");

            var table = new Table();
            table.AddColumn("Metric");
            table.AddColumn("Value");

            table.AddRow("Total Coding Time", FormatTimeSpan(report.TotalTime));
            table.AddRow("Average Session Time", FormatTimeSpan(report.AverageTime));
            table.AddRow("Number of Sessions", report.SessionCount.ToString());

            AnsiConsole.Write(table);
        }

        private string FormatTimeSpan(TimeSpan time)
        {
            return $"{(int)time.TotalHours:D2}:{time.Minutes:D2}:{time.Seconds:D2}";
        }

        public CodingGoalOption GetCodingGoalMenuChoice()
        {
            AnsiConsole.Clear();
            DisplayTitle("Manage Coding Goals");

            return AnsiConsole.Prompt(
                new SelectionPrompt<CodingGoalOption>()
                    .Title("Select an option:")
                    .AddChoices(Enum.GetValues(typeof(CodingGoalOption)).Cast<CodingGoalOption>())
                    .UseConverter(o => o.GetDisplayText()));
        }

        public void DisplayCodingGoals(IEnumerable<CodingGoal> goals, Dictionary<int, double> hoursPerDay, Dictionary<int, double> totalHoursSpent)
        {
            AnsiConsole.Clear();
            DisplayTitle("Coding Goals");

            var table = new Table();
            table.AddColumn("ID");
            table.AddColumn("Start Date");
            table.AddColumn("End Date");
            table.AddColumn("Target Hours");
            table.AddColumn("Hours Spent");
            table.AddColumn("Hours/Day to Reach Goal");

            foreach (var goal in goals)
            {
                string hoursPerDayStr = hoursPerDay.TryGetValue(goal.Id, out double hours) ? hours.ToString("F2") : "N/A";
                table.AddRow(
                    goal.Id.ToString(),
                    goal.StartDate.ToString("yyyy-MM-dd"),
                    goal.EndDate.ToString("yyyy-MM-dd"),
                    goal.TargetHours.ToString(),
                    totalHoursSpent[goal.Id].ToString("F2"),
                    hoursPerDayStr
                );
            }

            AnsiConsole.Write(table);
        }

        public CodingGoal GetCodingGoalInput(CodingGoal? existingGoal = null)
        {
            var goal = existingGoal ?? new CodingGoal();

            goal.StartDate = GetDate("Enter the start date for the goal (yyyy-MM-dd)", existingGoal?.StartDate.Date ?? DateTime.Now.Date);
            goal.EndDate = GetDate("Enter the end date for the goal (yyyy-MM-dd)", existingGoal?.EndDate.Date ?? null);
            goal.TargetHours = AnsiConsole.Prompt(
                new TextPrompt<int>("Enter the target hours for the goal:")
                    .DefaultValue(existingGoal?.TargetHours ?? 100)
                    .Validate(hours => hours > 0 ? ValidationResult.Success() : ValidationResult.Error("Target hours must be greater than 0")));
            
            return goal;
        }
    }
}