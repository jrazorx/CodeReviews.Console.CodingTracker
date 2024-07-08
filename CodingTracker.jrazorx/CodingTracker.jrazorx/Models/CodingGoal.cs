namespace CodingTracker.jrazorx.Models
{
    public class CodingGoal
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TargetHours { get; set; }
        public double GetHoursPerDay(int totalHoursCompleted)
        {
            if (DateTime.Now < StartDate || DateTime.Now > EndDate)
            {
                return 0;
            }

            int daysLeft = (EndDate - DateTime.Now).Days + 1;
            int hoursLeft = TargetHours - totalHoursCompleted;

            return Math.Round((double)hoursLeft / daysLeft, 2);
        }
    }
}
