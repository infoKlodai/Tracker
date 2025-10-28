using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Tracker.PageModels
{
    public partial class DailyActivitiesViewModel : ObservableObject, IQueryAttributable
    {
        [ObservableProperty]
        private DateTime _date;
        public ObservableCollection<TrackerEntryViewModel> Activities { get; } = [];

        public DailyActivitiesViewModel(DateTime date)
        {
            Date = date;
        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("date"))
            {
                Date = (DateTime)query["date"];
            }
        }
    }
}
