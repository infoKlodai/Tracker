using CommunityToolkit.Maui;
using JournalApp.PageModels;
using JournalApp.Pages;

namespace JournalApp.ExtensionClasses
{
    public static class ServiceExtensions
    {
        public static void AddServicesToDIContainer(this IServiceCollection services)
        {
            AddViewModelClasses(services);
            AddPageClasses(services);
            AddPopupClasses(services);

            //AddRepositoryClasses(services);
        }

        private static void AddPageClasses(IServiceCollection services)
        {
            services.AddTransient<TrackerPage>();
            services.AddScoped<CalendarPage>();
            services.AddScoped<InsertTrackerEntryPage>();
            services.AddScoped<WeeklyReportsPage>();
            services.AddScoped<DailyReportsPage>();
            services.AddScoped<MonthlyReportsPage>();
            //TODO: Add other pages & viewmodels here DI
        }

        private static void AddViewModelClasses(IServiceCollection services)
        {
            services.AddTransient<TrackerPageModel>();
            services.AddScoped<CalendarPageModel>();
            services.AddScoped<InsertTrackerEntryPageModel>();
            services.AddScoped<WeeklyReportsPageModel>();
            services.AddScoped<DailyReportsPageModel>();
            services.AddScoped<MonthlyReportsPageModel>();

        }
        private static void AddPopupClasses(IServiceCollection services)
        {
            services.AddTransientPopup<AddActivityPopup, AddActivityPopupModel>();
        }
        //private static void AddRepositoryClasses(IServiceCollection services)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
