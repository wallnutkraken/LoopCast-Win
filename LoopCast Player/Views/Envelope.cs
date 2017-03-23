namespace LoopCast_Player.Views
{
    /// <summary>
    /// Clever way to get return values from windows
    /// </summary>
    public class Envelope<T>
    {
        public T Item { get; set; }
    }
}
