using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace LoopCast_Player.Model.SQL
{
    public static class DB
    {
        private static SQLiteConnection _conn;
        private static string _dbUrl
            => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "LoopCast.sqlite");

        /// <summary>
        /// Either loads the database, or creates one if it does not exist
        /// </summary>
        public static void Load()
        {
            if (!File.Exists(_dbUrl))
            {
                Create();
            }
            _conn = new SQLiteConnection($"Data Source={_dbUrl};");
            _conn.Open();
        }

        private static void Create()
        {
            SQLiteConnection.CreateFile(_dbUrl);
            _conn = new SQLiteConnection($"Data Source={_dbUrl};");
            _conn.Open();

            const string createFeeds = "CREATE TABLE Feeds (url varchar(500) NOT NULL PRIMARY KEY)";
            SQLiteCommand command = new SQLiteCommand(_conn);
            command.CommandText = createFeeds;
            command.ExecuteNonQuery();
        }

        public static void AddFeed(Feed feed)
        {
            const string addFeedSQL = "INSERT INTO Feeds (url) VALUES (@userUrl);";
            SQLiteCommand command = new SQLiteCommand(_conn);
            command.CommandText = addFeedSQL;
            command.Parameters.Add(new SQLiteParameter("@userUrl", feed.URL));

            command.ExecuteNonQuery();
        }

        public static List<Feed> GetFeeds()
        {
            List<Feed> feeds = new List<Feed>();

            const string getFeedsSQL = "SELECT url FROM Feeds";
            SQLiteCommand command = new SQLiteCommand(_conn);
            command.CommandText = getFeedsSQL;

            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                try
                {
                    feeds.Add(new Feed(reader.GetString(0)));
                }
                catch
                {
                    /* Todo: log */
                }
            }
            return feeds;
        }

        public static void Disconnect()
        {
            _conn.Close();
        }
    }
}
