using TestGame.Entities;

namespace TestGame.Conveyor.Receiver
{
    public interface IOrderData
    {
        int MaxOrderSequenceLength { get; }
        float TimePerOrderItem { get; }
        CargoEntity.Type[] OrderSequence { get; }
        int OrderSize { get; }
        int CurrentOrderItemIndex { get; }
        float TimeToCompleteOrder { get; }
        CargoEntity.Type this[int index] { get; }
    }
}