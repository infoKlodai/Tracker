using Tracker.PageModels;

namespace Tracker.Pages;

public partial class TrackerPage : ContentPage
{
    TrackerPageModel _pageModel;
    public TrackerPage(TrackerPageModel model)
	{
		InitializeComponent();
        _pageModel = model;
        BindingContext = _pageModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _pageModel.LoadActivitiesAsync();
    }
}