using Tracker.Models;
using Tracker.PageModels.ActivityPageModels;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace Tracker.PageModels
{
    public class ActivitiesLoaderViewModel
    {
    public ObservableCollection<ActivityViewModel> Activities { get; } = [];
        public ActivitiesLoaderViewModel()
        {
            LoadActivitiesAsync().ConfigureAwait(false);
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
    }
}
