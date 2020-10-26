using UnityEngine;
using TestGame.Entities;

namespace TestGame.Conveyor.Receiver
{
    [System.Serializable]
    public class OrderData : IOrderData
    {
        [SerializeField] private CargoList cargoList = default;
        [SerializeField] private int maxOrderSequenceLength = 5;
        [SerializeField] private float timePerOrderItem = 30f;
        private CargoEntity.Type[] orderSequence;
        private int orderSize;
        private int currentOrderItemIndex;
        private float timeToCompleteOrder;

        public int MaxOrderSequenceLength => maxOrderSequenceLength;
        public float TimePerOrderItem => timePerOrderItem;      
        public CargoEntity.Type[] OrderSequence => orderSequence;
        public int OrderSize => orderSize;        
        public int CurrentOrderItemIndex => currentOrderItemIndex;
        public float TimeToCompleteOrder => timeToCompleteOrder;               
        public CargoEntity.Type this[int index] => orderSequence[index];

        public void Initialize()
        {
            orderSequence = new CargoEntity.Type[maxOrderSequenceLength];
        }

        public void CreateOrder(int sequenceSize)
        {
            if (sequenceSize > MaxOrderSequenceLength) Debug.LogError($"ERROR: Max sequence size is {MaxOrderSequenceLength}");
            orderSize = sequenceSize;
            for (int i = 0; i < sequenceSize; i++)
            {
                var randomEntityType = cargoList[Random.Range(0, cargoList.Count)].CargoType;
                if (i > 0)
                {
                    while (orderSequence[i - 1] == randomEntityType)
                    {
                        randomEntityType = cargoList[Random.Range(0, cargoList.Count)].CargoType;
                    }
                }
                orderSequence[i] = randomEntityType;
            }
            timeToCompleteOrder = timePerOrderItem * sequenceSize;
            currentOrderItemIndex = 0;
        }

        public bool CompletedOrNextItemIndex()
        {
            currentOrderItemIndex++;
            if (currentOrderItemIndex == orderSize)
                return true;
            else
                return false;
        }
    }
}