using System.Globalization;

namespace CodingTracker
{
    public class InputValidator
    {
        public bool TryParseDateTime(string input, out DateTime result)
        {
            return DateTime.TryParseExact(input, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
        }

        public bool ValidateTimeRange(DateTime startTime, DateTime endTime)
        {
            return startTime < endTime;
        }

        public bool TryParseInt(string input, out int result)
        {
            return int.TryParse(input, out result);
        }
    }
}