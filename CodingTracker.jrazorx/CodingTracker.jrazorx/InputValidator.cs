using System.Globalization;

namespace CodingTracker
{
    public class InputValidator
    {
        public bool TryParseDateTime(string input, out DateTime result)
        {
            return DateTime.TryParseExact(input, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
        }

        public bool IsSessionTimeRangeValid(DateTime startTime, DateTime endTime)
        {
            return startTime < endTime;
        }

        public bool IsSessionDurationValid(DateTime startTime, DateTime endTime)
        {
            TimeSpan duration = endTime - startTime;
            return duration.TotalHours < 24;
        }

        public bool TryParseInt(string input, out int result)
        {
            return int.TryParse(input, out result);
        }
    }
}