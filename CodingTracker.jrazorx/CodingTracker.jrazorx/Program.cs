using Spectre.Console;
using System.Configuration;

class Program
{
    static async Task Main(string[] args)
    {
        var connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

        if (string.IsNullOrEmpty(connectionString))
        {
            AnsiConsole.MarkupLine("[red]Error: Connection string not found in App.config[/]");
            return;
        }
    }
}