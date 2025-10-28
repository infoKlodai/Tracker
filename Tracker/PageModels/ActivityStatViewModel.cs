

namespace JournalApp.PageModels
{
    public partial class ActivityStatViewModel : BaseViewModel, IQueryAttributable
    {
        [ObservableProperty]
        private string _icon;
        [ObservableProperty]
        private string _name;
        [ObservableProperty]
        private TimeSpan _totalDuration;

        
        public ActivityStatViewModel(string icon, string name, TimeSpan duration)
        {
            Icon = icon;
            Name = name;
            TotalDuration = duration;
        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            throw new NotImplementedException();
        }
    }
}
