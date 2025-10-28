using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JournalApp.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JournalApp.PageModels
{
    public partial class CalendarPageModel : ObservableObject, IQueryAttributable
    {
        [ObservableProperty]
        private DateTime _date;
        public CalendarPageModel()
        {
            Date = DateTime.Now;
        }
        public CalendarPageModel(DateTime date)
        {
            Date = date;
        }
        public async Task SelectDate()
        {
            var date = new ShellNavigationQueryParameters
            {
                { "Date", Date }
            };
            await Shell.Current.GoToAsync("//TrackerPage", date);
        }
        [RelayCommand]
        async Task Back()
        {
            
                await Shell.Current.GoToAsync("..");
            
        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("date"))
            {
                Date = (DateTime)query["date"];
            }
        }
    
    }
}
