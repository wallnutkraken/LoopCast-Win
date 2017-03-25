using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;

namespace LoopCast_Player.Model
{
    public class Feed
    {
        public string URL { get; }
        public string Name { get; }
        private List<PodcastInfo> _podcasts;

        public List<PodcastInfo> Podcasts => _podcasts.ToList();

        private SyndicationFeed _rssFeed;

        public Feed(string url)
        {
            URL = url;

            /* Get rss feed */
            _rssFeed = SyndicationFeed.Load(XmlReader.Create(url));

            Name = _rssFeed.Title.Text;
            _podcasts = _rssFeed.Items.Select(item => (PodcastInfo)item).ToList();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
