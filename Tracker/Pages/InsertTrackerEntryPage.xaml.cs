using Tracker.PageModels;

namespace Tracker.Pages;

public partial class InsertTrackerEntryPage : ContentPage
{
	public InsertTrackerEntryPage(InsertTrackerEntryPageModel pageModel)
	{
		InitializeComponent();
		BindingContext = pageModel;
    }
}