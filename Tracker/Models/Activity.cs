using System.Text.Json.Serialization;

namespace Tracker.Models
{
    public class Activity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Icon { get; set; }
        public string? Color { get; set; }
        public bool IsGrouped { get; set; } = false;
        public Activity()
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Icon = string.Empty;
            Color = string.Empty;
        }
        [JsonConstructor]
        public Activity(Guid id, string name, string icon, string color, bool isGrouped)
        {
            Id = id;
            Name = name;
            Icon = icon;
            Color = color;
            IsGrouped = isGrouped;
        }
    }
}
