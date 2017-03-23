using System;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using LoopCast_Player.Model.Exceptions;

namespace LoopCast_Player.Model
{
    public class Podcast
    {
        public string URL { get; }
        public string Name { get; }

        private Stream _stream;

        public Podcast(string fileURL, string name)
        {
            URL = fileURL;
            Name = name;
        }

        public void ConnectStream()
        {
            WebRequest req = WebRequest.Create(URL);
            using (WebResponse response = req.GetResponse())
                _stream = response.GetResponseStream();
        }

        public void StartPlayer()
        {
            if (_stream == null)
                throw new FileNotLoadedException();

            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return Name;
        }

        public static explicit operator Podcast(SyndicationItem podcastItem)
        {
            return new Podcast(podcastItem.Links.First().Uri.OriginalString, podcastItem.Title.Text);
        }
    }
}
