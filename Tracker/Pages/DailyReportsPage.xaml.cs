using Tracker.PageModels;

namespace Tracker.Pages;

public partial class DailyReportsPage : ContentPage
{
	DailyReportsPageModel _pageModel;
    public DailyReportsPage(DailyReportsPageModel pageModel)
	{
		InitializeComponent();
		_pageModel = pageModel;
		BindingContext = _pageModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_pageModel.Activities.Count == 0)
        {
            await _pageModel.LoadActivitiesAsync();
        }
        await _pageModel.GetTrackerEntriesAsync(_pageModel.SelectedDate);
        await _pageModel.CalculateStatsAsync();
    }
}