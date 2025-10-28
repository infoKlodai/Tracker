using JournalApp.Pages;
namespace JournalApp
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
