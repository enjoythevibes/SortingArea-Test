namespace TestGame.Conveyor.Belt
{
    public interface IConveyorBelt
    {
        bool IsWorking { get; }
        bool StartPointFilled { get; }
        bool EndPointFilled { get; }
        void Run();
        void Stop();
        void AddCargoOnStartPoint(Entities.CargoEntity cargoEntity);
        Entities.CargoEntity TakeCargoFromEndPoint();
        Entities.CargoEntity PeekCargoFromEndPoint();
        void RemoveCargoFromEndPoint();
        void ClearBelt();
    }
}