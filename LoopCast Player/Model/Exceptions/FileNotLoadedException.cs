using System;

namespace LoopCast_Player.Model.Exceptions
{
    public class FileNotLoadedException : Exception
    {
        public FileNotLoadedException() : base("The file was not yet loaded")
        {
            
        }

        public FileNotLoadedException(string message) : base(message)
        {
            
        }
    }
}
