namespace TestGame.Conveyor.Receiver
{
    public delegate void OnNextOrder(IOrderData orderData);
    public delegate void OnNextOrderItem(IOrderData orderData);
    public delegate void OnOrderCompleted(IOrderData orderData);
    public delegate void OnOrderFailed(IOrderData orderData);
    public delegate void OnOrderTimeRunsOut(IOrderData orderData);
    public interface IConveyorOrderManager
    {
        bool IsWorking { get; }
        void StartWorking();
        void StopWorking();
        event OnNextOrder OnNextOrderNotify;
        event OnNextOrderItem OnNextOrderItemNotify;
        event OnOrderCompleted OnOrderCompletedNotify;
        event OnOrderFailed OnOrderFailedNotify; 
        event OnOrderTimeRunsOut OnOrderTimeRunsOutNotify;
    }
}