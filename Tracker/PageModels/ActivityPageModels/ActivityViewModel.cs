using CommunityToolkit.Mvvm.ComponentModel;
using JournalApp.Models;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace JournalApp.PageModels.ActivityPageModels
{
    public partial class ActivityViewModel : ObservableObject, IQueryAttributable
    {
        [ObservableProperty]
        private Activity _activity;
        [JsonIgnore]
        public Guid ActivityId
        {
            get => Activity.Id;
        }
        [JsonIgnore]
        public string Name
        {
            get => Activity.Name;
            set
            {
                if (Activity.Name != value)
                {
                    Activity.Name = value;
                    OnPropertyChanged();
                }
            }
        }
        [JsonIgnore]
        public string Icon
        {
            get => Activity.Icon;
            set
            {
                if (Activity.Icon != value)
                {
                    Activity.Icon = value;
                    OnPropertyChanged();
                }
            }
        }
        [JsonIgnore]
        public string Color
        {
            get => Activity.Color;
            set
            {
                if (Activity.Color != value)
                {
                    Activity.Color = value;
                    OnPropertyChanged();
                }
            }
        }
        [JsonIgnore]    
        public bool IsGrouped
        {
            get => Activity.IsGrouped;
            set
            {
                //&& !string.IsNullOrEmpty(Group)
                if (Activity.IsGrouped != value)
                {
                    Activity.IsGrouped = value;
                    OnPropertyChanged();
                }
            }
        }
        [ObservableProperty]
        [property: JsonIgnore]
        //[property: NotifyPropertyChangedFor(nameof(IsGrouped))]
        private string _group = string.Empty;
        public ActivityViewModel(Activity activity)
        {
            _activity = activity;
        }
        //TODO: spalvu sarasai su ju verciu stringais
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            throw new NotImplementedException();
        }
    }
}
