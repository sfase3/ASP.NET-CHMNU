namespace MyApp.services.TimeService
{
    public class TimeService : ITimeService
    {
        private string? CustomPhrase { get; set; }
        public TimeService()
        {
            DateTime currentTime = DateTime.Now;
            int currentHour = currentTime.Hour;
            // Determine the time of day and set custom phrases and colors
            if (currentHour >= 12 && currentHour < 18)
            {
                CustomPhrase = "Good afternoon";
            }
            else if (currentHour >= 18 && currentHour < 24)
            {
                CustomPhrase = "Good evening";
            }
            else if (currentHour >= 0 && currentHour < 6)
            {
                CustomPhrase = "Good night";
            }
            else
            {
                CustomPhrase = "Good morning";
            }
        }
        public string GetDatePhrase()
        {
            if (CustomPhrase == null)
            {
                throw new NullReferenceException("Server is not able to process time");
            }
            return CustomPhrase;
        }
    }
}
