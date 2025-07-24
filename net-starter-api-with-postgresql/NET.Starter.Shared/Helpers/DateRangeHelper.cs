namespace NET.Starter.Shared.Helpers
{
    public static class DateRangeHelper
    {
        public static int CountDaysBetween(DateOnly startDate, DateOnly endDate)
            => CountDaysBetween(startDate, endDate, []);

        public static int CountDaysBetween(DateOnly startDate, DateOnly endDate, DayOfWeek[] excludedDaysOfWeek)
        {
            if (endDate < startDate)
                throw new ArgumentException("End date cannot be earlier than start date.");

            int count = 0;
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if (!excludedDaysOfWeek.Contains(date.DayOfWeek))
                    count++;
            }

            return count;
        }
    }
}
