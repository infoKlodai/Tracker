using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JournalApp.DataAccess
{
    public static class JournalEntriesWriter
    {
        //TODO: Json ir kt i failus rasymus i atskira projektą iskelti
        public static async Task WriteEntriesAsync(string filePath, IEnumerable<Models.JournalEntry> entries)
        {
            try
            {
                using FileStream stream = File.Create(filePath);
                await JsonSerializer.SerializeAsync(stream, entries);
                await stream.DisposeAsync();
                //var json = System.Text.Json.JsonSerializer.Serialize(entries);
                //await LocalFileAccess.SaveState(json, filePath);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error!", $"An exception occurred: {ex.Message}", "OK");
            }
        }
    }
}
