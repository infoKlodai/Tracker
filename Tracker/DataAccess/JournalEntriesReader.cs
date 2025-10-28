using JournalApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JournalApp.DataAccess
{
    public static class JournalEntriesReader
    {
        //private readonly string filePath = LocalFileAccess.GetLocalPath(FileData.notesFile);
        public static async Task<List<JournalEntry>> ReadEntriesAsync(string filePath)
        {
            var entries = new List<JournalEntry>();
            try
            {
                if (!File.Exists(filePath))
                {
                    return entries;
                }
                var json = await LocalFileAccess.ReadFile(filePath);
                if (!string.IsNullOrEmpty(json))
                {
                    entries = JsonSerializer.Deserialize<List<JournalEntry>>(json);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error!", $"An exception occurred: {ex.Message}", "OK");
            }
            return entries;
        }
    }
}
