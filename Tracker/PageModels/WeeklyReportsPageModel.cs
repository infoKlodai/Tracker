using CommunityToolkit.Maui.Core.Extensions;
using JournalApp.DataAccess;
using JournalApp.Models;
using JournalApp.PageModels.ActivityPageModels;
using Microsoft.Maui.Storage;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace JournalApp.PageModels
{
    public partial class WeeklyReportsPageModel : BaseViewModel, IQueryAttributable
    {
        [ObservableProperty]
        private DateTime _firstDayOfWeek;
        [ObservableProperty]
        private DateTime _lastDayOfWeek;
        public List<ActivityViewModel> Activities { get; } = [];
        public ObservableCollection<ActivityStatViewModel> ActivityStats { get; } = [];
        List<TrackerEntry> _entries = [];
        public WeeklyReportsPageModel()
        {
            FirstDayOfWeek = GetFirstDayOfWeek(DateTime.Now);
            LastDayOfWeek = GetFirstDayOfWeek(DateTime.Now).AddDays(6);
        }
        [RelayCommand]
        public async Task PreviousWeek()
        {
            FirstDayOfWeek = FirstDayOfWeek.AddDays(-7);
            LastDayOfWeek = LastDayOfWeek.AddDays(-7);
            await GetWeeklyTrackerEntriesAsync(FirstDayOfWeek);
            await CalculateWeeklyStatsAsync();
        }
        [RelayCommand]
        public async Task NextWeek()
        {
            FirstDayOfWeek = FirstDayOfWeek.AddDays(7);
            LastDayOfWeek = LastDayOfWeek.AddDays(7);
            await GetWeeklyTrackerEntriesAsync(FirstDayOfWeek);
            await CalculateWeeklyStatsAsync();
        }
        public async Task CalculateWeeklyStatsAsync()
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
        public async Task GetWeeklyTrackerEntriesAsync(DateTime firstDay)         
        {
            if (_entries.Count != 0)
            {
                _entries.Clear();
            }
            for (int i = 0; i < 6; i++)
            {
                string fileName = GetFileName(firstDay.AddDays(i));
                var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);
                if (!File.Exists(filePath))
                {
                    continue;
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
        public static DateTime GetFirstDayOfWeek(DateTime date)
        {
            var culture = Thread.CurrentThread.CurrentCulture; // Getting current culture information
            var diff = date.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek; // Calculating the difference between the input date's day of the week and the first day of the week in the culture

            if (diff < 0)
                diff += 7; // If the difference is negative, add 7 days to get the correct first day of the week

            return date.AddDays(-diff).Date; // Returning the date adjusted to the first day of the week
        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            throw new NotImplementedException();
        }
    }
}


