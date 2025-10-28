using CommunityToolkit.Maui.Core;
using JournalApp.Models;
using JournalApp.PageModels.ActivityPageModels;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace JournalApp.PageModels
{
    public partial class AddActivityPopupModel : ObservableObject
    {
        [ObservableProperty]
        string name = "";
        public ObservableCollection<ActivityViewModel> Activities { get; } = [];

        readonly IPopupService popupService;

        public AddActivityPopupModel(IPopupService popupService)
        {
            this.popupService = popupService;
            LoadActivitiesAsync().ConfigureAwait(false);
        }
        [RelayCommand]
        void OnCancel()
        {
            popupService.ClosePopup();
        }

        [RelayCommand] 
        async Task Save(ActivityViewModel activityView)
        {
            await popupService.ClosePopupAsync(activityView);
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
