using JournalApp.DataAccess;
using JournalApp.Models;
using JournalApp.PageModels.ActivityPageModels;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace JournalApp.PageModels
{
    public partial class DailyReportsPageModel : BaseViewModel, IQueryAttributable
    {
        [ObservableProperty]
        private DateTime _selectedDate;
        public List<ActivityViewModel> Activities { get; } = [];
        List<TrackerEntry> _entries = [];
        public ObservableCollection<ActivityStatViewModel> ActivityStats { get; } = [];

        public DailyReportsPageModel()
        {
            SelectedDate = DateTime.Now;
        }
        [RelayCommand]
        public async Task PreviousDay()
        {
            SelectedDate = SelectedDate.AddDays(-1);
            await GetTrackerEntriesAsync(SelectedDate);
            await CalculateStatsAsync();
        }
        [RelayCommand]
        public async Task NextDay()
        {
            SelectedDate = SelectedDate.AddDays(1);
            await GetTrackerEntriesAsync(SelectedDate);
            await CalculateStatsAsync();
        }
        public async Task CalculateStatsAsync()
        {
            if (_entries.Count == 0)
            {
                await Shell.Current.DisplayAlert("Ups!", $"No statistics available.", "OK");
                return;
            }
            if (ActivityStats.Count != 0)
            {
                ActivityStats.Clear();
            }

            var groupedEntries = _entries.GroupBy(a => a.Activity)
                .Select(g => new
                {
                    Activity = g.Key,
                    Entries = g.ToList()
                }).ToList();

            List<ActivityStatViewModel> activities = [];
            foreach (var group in groupedEntries)
            {
                var activityId = Activities?.FirstOrDefault(a => a.ActivityId == group.Activity);
                if (activityId is null)
                {
                    continue;
                }

                TimeSpan totalDuration = group.Entries.Aggregate(TimeSpan.Zero, (subtotal, t) => subtotal.Add(t.Duration));

                var activityStat = new ActivityStatViewModel(activityId.Icon, activityId.Name, totalDuration);
                activities.Add(activityStat);
            }
            var temporaryList = activities.OrderByDescending(a => a.TotalDuration);
            foreach (var activity in temporaryList)
            {
                ActivityStats.Add(activity);
            }
        }
        public async Task GetTrackerEntriesAsync(DateTime date)
        {
            if (_entries.Count != 0)
            {
                _entries.Clear();
            }
            
                string fileName = GetFileName(date);
                var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);
                if (!File.Exists(filePath))
                {
                    await Shell.Current.DisplayAlert("Ups!", $"No activities on this day", "OK");
                return;
                }
                else
                {
                    var entries = new List<TrackerEntry>();
                    try
                    {
                        var json = await LocalFileAccess.ReadFile(filePath);
                        if (!string.IsNullOrEmpty(json))
                        {
                            entries = JsonSerializer.Deserialize<List<TrackerEntry>>(json);
                        }
                    }
                    catch (Exception ex)
                    {
                        await Shell.Current.DisplayAlert("Error!", $"An exception occurred: {ex.Message}", "OK");
                    }
                    if (entries is not null)
                    {
                        foreach (var entry in entries)
                        {
                            _entries.Add(entry);
                        }
                    }
                }
            
        }
        private string GetFileName(DateTime date)
        {
            return $"{date:yyyy-MM-dd}.json";
        }
        public async Task LoadActivitiesAsync()
        {
            if (Activities.Count != 0)
            {
                Activities.Clear();
            }
            using var stream = await FileSystem.OpenAppPackageFileAsync("activities.json");
            using var reader = new StreamReader(stream);

            var contents = reader.ReadToEnd();
            var activities = JsonSerializer.Deserialize<List<Activity>>(contents);
            if (activities is not null)
            {
                foreach (var activity in activities)
                {
                    var activityViewModel = new ActivityViewModel(activity);
                    Activities.Add(activityViewModel);
                }
            }
        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            throw new NotImplementedException();
        }
    }
}
