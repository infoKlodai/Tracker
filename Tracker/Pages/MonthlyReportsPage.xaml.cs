using JournalApp.PageModels;

namespace JournalApp.Pages;

public partial class MonthlyReportsPage : ContentPage
{
	MonthlyReportsPageModel _pageModel;
	public MonthlyReportsPage(MonthlyReportsPageModel pageModel)
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
		await _pageModel.GetMonthlyTrackerEntriesAsync(_pageModel.FirstDayOfMonth);
		await _pageModel.CalculateMonthlyStatsAsync();
    }
}