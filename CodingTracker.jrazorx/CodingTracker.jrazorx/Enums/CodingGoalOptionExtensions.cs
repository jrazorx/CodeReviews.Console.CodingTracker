namespace CodingTracker.jrazorx.Enums
{
    public static class CodingGoalOptionExtensions
    {
        private static readonly Dictionary<CodingGoalOption, string> DisplayTexts = new()
        {
            { CodingGoalOption.AddGoal, "Add a coding goal" },
            { CodingGoalOption.ViewGoals, "View coding goals" },
            { CodingGoalOption.UpdateGoal, "Update a coding goal" },
            { CodingGoalOption.DeleteGoal, "Delete a coding goal" },
            { CodingGoalOption.Back, "Back to main menu" }
        };

        public static string GetDisplayText(this CodingGoalOption option)
        {
            return DisplayTexts.TryGetValue(option, out var text) ? text : option.ToString();
        }
    }
}