using System;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using LoopCast_Player.Model.Exceptions;
using NAudio.Wave;

namespace LoopCast_Player.Model
{
    public class Podcast
    {
        public string URL { get; }
        public string Name { get; }

        private Stream _stream;
        private IWavePlayer _player;
        private WaveStream _waveStream;

        public Podcast(string fileURL, string name)
        {
            URL = fileURL;
            Name = name;
        }

        public bool IsPlaying => _player?.PlaybackState == PlaybackState.Playing;

        public void ConnectStream()
        {
            /* Upgrade later: http://mark-dot-net.blogspot.dk/2011/05/how-to-play-back-streaming-mp3-using.html */
            WebRequest req = WebRequest.Create(URL);
            using (WebResponse response = req.GetResponse())
            {
                _stream = new MemoryStream();
                using (Stream stream = response.GetResponseStream())
                {
                    byte[] buffer = new byte[32768];
                    int read;
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        _stream.Write(buffer, 0, read);
                    }
                }
            }
            _stream.Position = 0;
        }

        public void StartPlayer()
        {
            if (_stream == null)
                throw new FileNotLoadedException();

            _player = new WaveOut();
            _waveStream = new BlockAlignReductionStream(WaveFormatConversionStream.CreatePcmStream(new Mp3FileReader(_stream)));
            _player.Init(_waveStream);
        }

        public void Play()
        {
            if (_player.PlaybackState != PlaybackState.Playing)
                _player.Play();
        }

        public void Pause()
        {
            if (_player.PlaybackState == PlaybackState.Playing)
                _player.Pause();
        }

        public void Stop()
        {
            if (_player.PlaybackState != PlaybackState.Stopped)
                _player.Stop();
        }

        public void Reverse(TimeSpan timeToGoBack)
        {
            if (_waveStream.CurrentTime < timeToGoBack)
                _waveStream.CurrentTime = TimeSpan.Zero;
            else
                _waveStream.CurrentTime = CurrentTime - timeToGoBack;
        }

        public void Forward(TimeSpan timeToForward)
        {
            if (CurrentTime + timeToForward >= _waveStream.TotalTime)
                Stop();
            else
                _waveStream.CurrentTime = CurrentTime + timeToForward;
        }

        public TimeSpan Length => _waveStream.TotalTime;
        public TimeSpan CurrentTime => TimeSpan.FromSeconds(Math.Floor(_waveStream.CurrentTime.TotalSeconds));

        public override string ToString()
        {
            return Name;
        }

        public static explicit operator Podcast(SyndicationItem podcastItem)
        {
            return new Podcast(podcastItem.Links
                .First(u => u.MediaType != null && u.MediaType.StartsWith("audio"))
                    .Uri.OriginalString, podcastItem.Title.Text);
        }
    }
}
