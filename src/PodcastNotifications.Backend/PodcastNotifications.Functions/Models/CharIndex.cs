using System.Collections.Generic;

namespace PodcastNotifications.Functions.Models
{
    public class CharIndex
    {
        public string Index { get; set; }
        public IEnumerable<Podcast> Data { get; set; }
    }
}
