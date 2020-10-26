namespace TestGame.Door
{
    public interface IDoor
    {
        bool Opened { get; }
        void Open();
        void Close();
    }
}