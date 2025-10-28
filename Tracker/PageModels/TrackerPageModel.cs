using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JournalApp.DataAccess;
using JournalApp.Models;
using JournalApp.PageModels.ActivityPageModels;
using JournalApp.Pages;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JournalApp.PageModels
{
    public partial class TrackerPageModel : BaseViewModel,  IQueryAttributable
    {
        private int lastId = 0;
        private readonly IPopupService _popupService;
        private DateTime _date;

        [JsonIgnore]
        public DateTime Date
        {
            get => _date;
            set
            {
                if (_date != value)
                {
                    _date = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(DateLabel));
                    SelectDateCommand.ExecuteAsync(_date);
                }
            }
        }
        public string DateLabel => Date.ToString("d.M.yyyy, ") + Date.DayOfWeek.ToString();
        public ObservableCollection<TrackerEntryViewModel> Entries { get; } = [];
        public ObservableCollection<ActivityViewModel> Activities { get; } = [];
        

        public TrackerPageModel(IPopupService popupService)
        {
            _popupService = popupService;
            Date = DateTime.Today;
           
        }
        public TrackerPageModel(IPopupService popupService, DateTime date)
        {
            _popupService = popupService;
            Date = date;
        }
        [RelayCommand]
        public async Task SelectDate(DateTime date)
        {   
            await LoadEntriesAsync(date);
        }
        [RelayCommand]
        async Task AddEntryPopup()        
        {
            var entryActivity = await DisplayPopup();
            if (entryActivity is not null) await AddEntry(entryActivity);

        }
        [RelayCommand]
        async Task AddEntry(ActivityViewModel activityView)
        {
            if (activityView is null)
            {
                return;
            }
            var entry = new TrackerEntryViewModel(new TrackerEntry(GenerateId(), activityView.ActivityId), activityView);
            entry.Start = DateTime.Now;
            if (Entries.Count > 0)
            {
                var lastEntry = Entries.Last();
                if (lastEntry.End is null)
                {
                    lastEntry.End = DateTime.Now;
                }
            }
            Entries.Add(entry);
            await SaveEntriesAsync();
        }
        [RelayCommand]
        async Task InsertEntry()
        {
            var date = new ShellNavigationQueryParameters
            {
                { "Date", Date }
            };
            await Shell.Current.GoToAsync(nameof(InsertTrackerEntryPage), date);            
        }
        [RelayCommand]
        async Task EditEntry(TrackerEntryViewModel entry)
        {
            var activity = await DisplayPopup();
            if (activity is not null)
            {
                entry.Icon = activity.Icon;
                entry.Name = activity.Name;
                entry.ActivityId = activity.ActivityId;

                await SaveEntriesAsync();
            }
           
        }
        [RelayCommand]
        async Task RemoveEntry(TrackerEntryViewModel entry)
        {
            Entries.Remove(entry);
            await SaveEntriesAsync();
        }
        public async Task LoadEntriesAsync(DateTime date)
        {
            string fileName = GetFileName(date);
            var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);
            
            
            if (!File.Exists(filePath))
            {
                await Shell.Current.DisplayAlert("Ups!", $"No activities on this day", "OK");
                return;
            }
            else
            {
                if (Entries.Count != 0)
                {
                    Entries.Clear();
                }
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
                        var activityId = Activities?.FirstOrDefault(a => a.ActivityId == entry.Activity);
                        if (activityId is not null)
                        {
                            Entries.Add(new TrackerEntryViewModel(entry, activityId));
                        }
                    }
                    OnPropertyChanged(nameof(Entries));
                }
            }
        }

        public async Task SaveEntriesAsync()
        {
            var fileName =GetFileName(Date);
            var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

            var contents = JsonSerializer.Serialize(Entries.Select(n => n.Job));
            await File.WriteAllTextAsync(filePath, contents);           
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

        private int GenerateId()
        {
           var id = Interlocked.Increment(ref lastId);
            lastId = id;
            return id;
        }
        private string GetFileName(DateTime date)
        {
            return $"{date:yyyy-MM-dd}.json";
        }
        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("Date"))
            {
                Date = (DateTime)query["Date"];
            }
            if (query.ContainsKey("Entry"))
            {
                var entryToAdd = (TrackerEntryViewModel)query["Entry"];
                if (entryToAdd is null)
                {
                    return;
                }
                InsertEntry(entryToAdd);
                await SaveEntriesAsync();
            }
        }

        private void InsertEntry(TrackerEntryViewModel entryToAdd)
        {
            var index = 0;

            if (Entries.Count == 0)
            {
                Entries.Add(entryToAdd);
                return;
            }
            //iterpia ir sukuria nauja paskui einanti
            if (entryToAdd.End is not null)
            {
                var bigEntry = Entries.FirstOrDefault(e => e.Start < entryToAdd.Start && e.End > entryToAdd.End);

                if (bigEntry is not null)
                {
                    var bigEntryIndex = Entries.IndexOf(bigEntry);
                    var newEntryAfter = new TrackerEntryViewModel(new TrackerEntry(GenerateId(), bigEntry.ActivityId));
                    newEntryAfter.Start = (DateTime)entryToAdd.End;
                    newEntryAfter.End = bigEntry.End;
                    Entries.ElementAt<TrackerEntryViewModel>(bigEntryIndex).End = entryToAdd.Start;
                    Entries.Insert(bigEntryIndex + 1, entryToAdd);
                    Entries.Insert(bigEntryIndex + 2, newEntryAfter);
                    return;
                }
            }

            var entryAfter = Entries.FirstOrDefault(e => e.Start > entryToAdd.Start);

            if (entryAfter is not null)
            {
                var entryAfterIndex = Entries.IndexOf(entryAfter);

                if (entryAfterIndex > 0)
                {
                    index = entryAfterIndex;
                    var entryBefore = Entries.ElementAt<TrackerEntryViewModel>(index - 1);


                    //priraso pries tai esanciam pabaiga arba ja pakeicia i entryToAdd.Start
                    if (entryBefore.End is null || entryBefore.End > entryToAdd.Start)
                    {
                        Entries.ElementAt<TrackerEntryViewModel>(index - 1).End = entryToAdd.Start;
                    }
                    //jei entryAfter.Start yra mazesnis uz entryToAdd.End, tai entryAfter pradedam nuo entryToAdd pabaigos
                    if (entryAfter.Start < entryToAdd.End)
                    {
                        Entries.ElementAt<TrackerEntryViewModel>(index).Start = (DateTime)entryToAdd.End;
                    }
                    //istrina visus esancius tarp entryToAdd.Start ir entryToAdd.End
                    var range = Entries.Where(e => e.Start > entryToAdd.Start && e.End <= entryToAdd.End).ToList();
                    if (range.Any())
                    {
                        Entries.Insert(index, entryToAdd);
                        foreach (var entry in range)
                        {
                            Entries.Remove(entry);
                        }
                        return;
                    }
                    
                }
            }           

            //jei nera po entryToAdd.Start esancio, tai prideda pirma
            Entries.Insert(index, entryToAdd);
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
    }
}
