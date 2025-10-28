using Tracker.PageModels;

namespace Tracker.Pages;

public partial class CalendarPage : ContentPage
{
    private CalendarPageModel _model;

    public CalendarPage(CalendarPageModel model)
	{
        InitializeComponent();
        BindingContext = model;
        _model = model;
    }

    private async void datePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        await _model.SelectDate();
    }
}