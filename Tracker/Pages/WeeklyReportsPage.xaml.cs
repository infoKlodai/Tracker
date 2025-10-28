using Tracker.PageModels;

namespace Tracker.Pages;

public partial class WeeklyReportsPage : ContentPage
{
	private WeeklyReportsPageModel _pageModel;
    public WeeklyReportsPage(WeeklyReportsPageModel pageModel)
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
        await _pageModel.GetWeeklyTrackerEntriesAsync(_pageModel.FirstDayOfWeek);
        await _pageModel.CalculateWeeklyStatsAsync();
        //if (BindingContext is WeeklyReportsPageModel _pageModel)
        //{
        //    await _pageModel.LoadActivitiesAsync();
        //}
    }
}