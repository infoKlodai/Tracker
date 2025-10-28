using JournalApp.PageModels;

namespace JournalApp.Pages;

public partial class InsertTrackerEntryPage : ContentPage
{
	public InsertTrackerEntryPage(InsertTrackerEntryPageModel pageModel)
	{
		InitializeComponent();
		BindingContext = pageModel;
    }
}