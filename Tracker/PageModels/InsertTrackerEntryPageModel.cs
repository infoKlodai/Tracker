using JournalApp.PageModels.ActivityPageModels;

namespace JournalApp.PageModels
{
    public partial class InsertTrackerEntryPageModel : BaseViewModel, IQueryAttributable
    {
        private IPopupService _popupService;
        [ObservableProperty]
        private DateTime _date = DateTime.Now;
        [ObservableProperty]
        TrackerEntryViewModel _trackerEntry;
        
        TimeSpan _startTime;
        [JsonIgnore]
        public TimeSpan StartTime
        {
            get => _startTime;
            set
            {
                if (_startTime != value)
                {
                    _startTime = value;
                    OnPropertyChanged();
                    TrackerEntry.Start = Date + _startTime;
                    OnPropertyChanged(nameof(TrackerEntry.Start));
                }
            }
        }
        TimeSpan _endTime = TimeSpan.Zero;
        [JsonIgnore]
        public TimeSpan EndTime
        {
            get => _endTime;
            set
            {
                if (_endTime != value)
                {
                    _endTime = value;
                    OnPropertyChanged();
                    TrackerEntry.End = Date + _endTime;
                    OnPropertyChanged(nameof(TrackerEntry.End));
                }
            }
        }
        [JsonIgnore]
        public bool IsDurationVisible => TrackerEntry.End is not null && TrackerEntry.End > TrackerEntry.Start;
        [JsonIgnore]
        public bool IsActivityButtonVisible => TrackerEntry.Job.Activity == Guid.Empty;
        public InsertTrackerEntryPageModel(IPopupService popupService)
        {
            _popupService = popupService;
            _trackerEntry = new TrackerEntryViewModel();
            _startTime =  _trackerEntry.Start.TimeOfDay;
        }
        [RelayCommand]
        public async Task SaveEntry()
        {
            if (TrackerEntry.Start >= TrackerEntry.End)
            {
                await Shell.Current.DisplayAlert("All fields must be filled.", "Please select start time.", "OK");
                return;
            }
            
            if (TrackerEntry.End == null || TrackerEntry.End <= TrackerEntry.Start)
            {
                await Shell.Current.DisplayAlert("All fields must be filled.", "Please select end time.", "OK");
                return;
            }
            if (TrackerEntry.ActivityId == Guid.Empty)
            {
                await Shell.Current.DisplayAlert("All fields must be filled.", "Please select an activity.", "OK");
                return;
            }
            var trackerEntry = new ShellNavigationQueryParameters
            {
                { "Entry", TrackerEntry }
            };
            await Shell.Current.GoToAsync("..", trackerEntry);
        }
        [RelayCommand]
        public async Task SelectActivity()
        {
            var activity = await DisplayPopup();
            if (activity is not null)
            {
                TrackerEntry.ActivityId = activity.ActivityId;
                TrackerEntry.Name = activity.Name;
                TrackerEntry.Icon = activity.Icon;
                OnPropertyChanged(nameof(TrackerEntry.ActivityId));
                OnPropertyChanged(nameof(TrackerEntry.Name));
                OnPropertyChanged(nameof(TrackerEntry.Icon));
            }
        }
        public async Task<ActivityViewModel?> DisplayPopup()
        {
            var activity = await _popupService.ShowPopupAsync<AddActivityPopupModel>();
            if (activity is null)
            {
                return null; // User canceled the popup
            }
            return (ActivityViewModel)activity;
        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("Date"))
            {
                Date = (DateTime)query["Date"];
                TrackerEntry = new TrackerEntryViewModel();
            }
        }
    }
}
