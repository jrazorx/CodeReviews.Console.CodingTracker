namespace CodingTracker.jrazorx.Enums
{
    public static class SessionViewOptionExtensions
    {
        private static readonly Dictionary<SessionViewOption, string> DisplayTexts = new()
        {
            { SessionViewOption.ChangeFilter, "Change filter" },
            { SessionViewOption.ChangeSortOrder, "Change sort order" },
            { SessionViewOption.Back, "Back to main menu" }
        };

        public static string GetDisplayText(this SessionViewOption option)
        {
            return DisplayTexts.TryGetValue(option, out var text) ? text : option.ToString();
        }
    }
}