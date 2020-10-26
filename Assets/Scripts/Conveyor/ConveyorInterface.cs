using TestGame.Conveyor.Belt;

namespace TestGame.Conveyor
{
    public interface IConveyor
    {
        ConveyorState CurrentState { get; }
        IConveyorBelt ConveyorBelt { get; }
        void SetState(ConveyorState state);
    }
}