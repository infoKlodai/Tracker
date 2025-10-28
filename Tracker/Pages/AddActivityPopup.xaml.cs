using CommunityToolkit.Maui.Views;
using JournalApp.PageModels;

namespace JournalApp.Pages;

public partial class AddActivityPopup : Popup
{
	AddActivityPopupModel _bandymasPopupModel;
    public AddActivityPopup(AddActivityPopupModel bandymasPopupModel)
	{
		InitializeComponent();
		_bandymasPopupModel = bandymasPopupModel;
		BindingContext = _bandymasPopupModel;
    }
}