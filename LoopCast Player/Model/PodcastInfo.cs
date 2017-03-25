using System.Linq;
using System.ServiceModel.Syndication;

namespace LoopCast_Player.Model
{
    public struct PodcastInfo
    {
        public string URL { get; }
        public string Name { get; }

        public PodcastInfo(string url, string name)
        {
            URL = url;
            Name = name;
        }

        public static explicit operator PodcastInfo(SyndicationItem podcastItem)
        {
            return new PodcastInfo(podcastItem.Links
                .First(u => u.MediaType != null && u.MediaType.StartsWith("audio"))
                    .Uri.OriginalString, podcastItem.Title.Text);
        }


        public override string ToString()
        {
            return Name;
        }
    }
}
