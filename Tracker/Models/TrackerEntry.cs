using System.Text.Json.Serialization;


namespace JournalApp.Models
{
    public class TrackerEntry
    {
        public int Id { get; set; }
        public Guid Activity { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public TimeSpan Duration { get; set; }
        public TrackerEntry(int id, Guid activity)
        {
            Id = id;
            Activity = activity;
            Start = DateTime.Now;
            End = null;
        }
        [JsonConstructor]
        public TrackerEntry(int id, Guid activity, DateTime start, DateTime? end, TimeSpan duration)
        {
            Id = id;
            Activity = activity;
            Start = start;
            End = end;
            Duration = duration;
        }

    }
}
