using CommunityToolkit.Maui.Views;
using Tracker.PageModels;

namespace Tracker.Pages;

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