using System.Text.Json.Serialization;

namespace JournalApp.Models
{
    public class JournalEntry
    {
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public JournalEntry()
        {
            Content = string.Empty;
            DateCreated = DateTime.Now;
            DateModified = null;
        }
        [JsonConstructor]
        public JournalEntry(string content, DateTime dateCreated, DateTime? dateModified)
        {
            Content = content;
            DateCreated = dateCreated;
            DateModified = dateModified;
        }


    }
}
