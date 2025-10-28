using Tracker.Pages;
namespace Tracker
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(InsertTrackerEntryPage), 
                typeof(InsertTrackerEntryPage));
        }
    }
}
