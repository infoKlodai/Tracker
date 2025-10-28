using CommunityToolkit.Mvvm.ComponentModel;
using JournalApp.Models;
using JournalApp.PageModels.ActivityPageModels;
namespace JournalApp.PageModels
{
    public partial class TrackerEntryViewModel : ObservableObject, IQueryAttributable
    {
        [ObservableProperty]
        private TrackerEntry _job;
        [JsonIgnore]
        public DateTime Start
        {
            get => Job.Start;
            set
            {
                if (Job.Start != value)
                {
                    Job.Start = value;
                    OnPropertyChanged();
                    if (Job.End is not null)
                    {
                        var duration = (Job.End - Job.Start).GetValueOrDefault();
                        SetProperty(Job.Duration, duration, Job, (u, n) => u.Duration = n);
                    }
                    OnPropertyChanged(nameof(Duration));
                }
            }
        }
        [JsonIgnore]
        public DateTime? End
        {
            get => Job.End;
            set
            {
                if (Job.End != value)
                {
                    Job.End = value;
                    OnPropertyChanged();
                    if (Job.End is not null)
                    {
                        var duration = (Job.End - Job.Start).GetValueOrDefault();
                        SetProperty(Job.Duration, duration, Job, (u, n) => u.Duration = n);
                    }
                    OnPropertyChanged(nameof(Duration));
                    OnPropertyChanged(nameof(IsDurationVisible));              
                }
            }
        }

        [JsonIgnore]
        public TimeSpan Duration
        {
            get => Job.Duration;
            set
            {
                if (Job.Duration != value)
                { 
                    Job.Duration = value;                   
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsDurationVisible));
                }
            }
        }
        [JsonIgnore]
        public string StartTime => Start.ToString("HH.mm");
        [JsonIgnore]
        public bool IsDurationVisible => (Duration != TimeSpan.Zero);

        [ObservableProperty]
        [property: JsonIgnore]
        private string _name;
        [ObservableProperty]
        [property: JsonIgnore]
        private string _icon;
        [JsonIgnore]
        public Guid ActivityId
        {
            get => Job.Activity;
            set
            {
                if (Job.Activity != value)
                {
                    Job.Activity = value;
                    OnPropertyChanged(); 
                    OnPropertyChanged(nameof(ActivityId));
                }
            }
        }

        public TrackerEntryViewModel()
        {
            _job = new TrackerEntry(0, Guid.Empty);
            Name = string.Empty;
            Icon = string.Empty;
        }
        public TrackerEntryViewModel(TrackerEntry job)
        {
            _job = job;
            Name = string.Empty;
            Icon = string.Empty;
        }
        public TrackerEntryViewModel(TrackerEntry job, ActivityViewModel activityView)
        {
            _job = job;
            _job.Activity = activityView.ActivityId;
            Name = activityView.Name;
            Icon = activityView.Icon;
        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            throw new NotImplementedException();
        }
    }
}
