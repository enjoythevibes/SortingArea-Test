using System.Collections;
using UnityEngine;

namespace TestGame.Conveyor.Receiver
{
    public class ConveyorOrderManager : MonoBehaviour, IConveyorOrderManager
    {
        [SerializeField] private OrderData currentOrder = new OrderData();
        private bool anyOrder;
        private IConveyor receivingConveyor;
        private WaitForSeconds waitBeforeSetWait = new WaitForSeconds(3f);
        private bool isWorking;     
           
        public bool IsWorking => isWorking;

        public event OnNextOrder OnNextOrderNotify;
        public event OnNextOrderItem OnNextOrderItemNotify;
        public event OnOrderCompleted OnOrderCompletedNotify;
        public event OnOrderFailed OnOrderFailedNotify;
        public event OnOrderTimeRunsOut OnOrderTimeRunsOutNotify;

        private void Awake()
        {
            receivingConveyor = gameObject.GetComponentWithInterface<IConveyor>();
            currentOrder.Initialize();
        }
      
        public void StartWorking()
        {
            if (isWorking) return;
            isWorking = true;
            GenerateOrder();
        }

        public void StopWorking()
        {
            if (!isWorking) return;
            isWorking = false;            
        }
        
        private void GenerateOrder()
        {
            if (anyOrder || !isWorking) return;
            if (receivingConveyor.CurrentState == ConveyorState.Waiting)
            {
                var randomSequenceSize = 2;                
                if (Player.PlayerScores.Scores >= 4 )
                {
                    randomSequenceSize = Random.Range(2, currentOrder.MaxOrderSequenceLength + 1);
                }
                currentOrder.CreateOrder(randomSequenceSize);
                anyOrder = true;                
                StartCoroutine(OrderTimer());
                receivingConveyor.SetState(ConveyorState.Working);
                OnNextOrderNotify?.Invoke(currentOrder);
            }
        }
        
        private IEnumerator OrderTimer()
        {
            var completed = false;
            var timeRunsOut = false;
            var orderTimer = 0f;
            var failed = false;
            while (orderTimer <= currentOrder.TimeToCompleteOrder && !completed && isWorking)
            {
                if (!timeRunsOut)
                {
                    if (orderTimer > currentOrder.TimeToCompleteOrder - currentOrder.TimePerOrderItem)
                    {
                        receivingConveyor.SetState(ConveyorState.TimeRunsOut);
                        OnOrderTimeRunsOutNotify?.Invoke(currentOrder);
                        timeRunsOut = true;
                    }
                }
                completed = ReadyOrNot(ref failed);
                orderTimer += Time.deltaTime;
                yield return null;
            }     
            if (!completed || failed)
            {
                OnOrderFailedNotify?.Invoke(currentOrder);
            }  
            else
            {
                OnOrderCompletedNotify?.Invoke(currentOrder);
            }
            receivingConveyor.SetState(ConveyorState.Stopped);  
            receivingConveyor.ConveyorBelt.ClearBelt();
            yield return waitBeforeSetWait;
            receivingConveyor.SetState(ConveyorState.Waiting);
            StartCoroutine(WaitUntilNextOrder());
            anyOrder = false;
            yield break;
        }

        private bool ReadyOrNot(ref bool failed)
        {
            var completed = false;
            var conveyorBelt = receivingConveyor.ConveyorBelt;
            
            if (conveyorBelt.EndPointFilled)
            {
                if (conveyorBelt.PeekCargoFromEndPoint().CargoEntityType == currentOrder[currentOrder.CurrentOrderItemIndex])
                {
                    completed = currentOrder.CompletedOrNextItemIndex();
                    OnNextOrderItemNotify?.Invoke(currentOrder);
                    conveyorBelt.RemoveCargoFromEndPoint();
                }
                else
                {
                    failed = true;
                    completed = true;
                    conveyorBelt.RemoveCargoFromEndPoint();
                }
            }
            return completed;
        }

        private IEnumerator WaitUntilNextOrder()
        {
            var waitTime = (int)Random.Range(3f, 10f);
            var timer = 0f;
            while (timer <= waitTime)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            GenerateOrder();
            yield break;   
        }
    }
}