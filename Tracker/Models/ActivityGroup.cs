using System.Text.Json.Serialization;

namespace JournalApp.Models
{
    public class ActivityGroup
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Icon { get; set; }
        public string? Color { get; set; }
        public List<Guid> Activities { get; set; } = new List<Guid>();
        public bool IsSelected { get; set; } = true;

        public ActivityGroup()
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Icon = string.Empty;
            Color = string.Empty;
        }
        [JsonConstructor]
        public ActivityGroup(Guid id, string name, string icon, string color, List<Guid> activities, bool isSelected)
        {
            Id = id;
            Name = name;
            Icon = icon;
            Color = color;
            Activities = activities;
            IsSelected = isSelected;
        }
    }
}
