using TestGame.Conveyor;

namespace TestGame.Triggers
{
    public enum ConveyorType
    {
        Submitter,
        Receiver
    }
    
    public interface IConveyorInteractionTrigger
    {
        IConveyor Conveyor { get; }
        ConveyorType CurrentConveyorType { get; }
    }
}