namespace TestGame.Conveyor.Submitter
{
    public interface ISubmitterConveyorManager
    {
        bool IsWorking { get; }
        void Start();
        void Stop();
    }
}