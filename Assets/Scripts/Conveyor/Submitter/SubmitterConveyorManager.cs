using UnityEngine;
using TestGame.Entities;
using enjoythevibes.Managers;

namespace TestGame.Conveyor.Submitter
{
    public class SubmitterConveyorManager : MonoBehaviour, ISubmitterConveyorManager
    {
        [SerializeField] private CargoList cargoList = default;
        [SerializeField] private int maxRandomInRow = 3;
        private int currentCargoIndex;
        private CargoEntity.Type[] randomCargoTypes;
        private System.Random random = new System.Random();
        private IConveyor conveyor;
        private bool isWorking;
        
        public bool IsWorking => isWorking;

        private void Awake()
        {
            cargoList.Awake();
            randomCargoTypes = new CargoEntity.Type[cargoList.Count * maxRandomInRow];
            for (int i = 0; i < cargoList.Count; i++)
            {
                for (int j = 0; j < maxRandomInRow; j++)
                {
                    randomCargoTypes[(i * maxRandomInRow) + j] = (CargoEntity.Type)(i + 1);
                }
            } 
            if (cargoList.Count == 0) Debug.LogError("ERROR: Cargo List length is 0");
            randomCargoTypes.Shuffle(random);
            conveyor = gameObject.GetComponentWithInterface<IConveyor>();
            this.enabled = false;
        }

        public void Start()
        {
            if (isWorking) return;
            isWorking = true;
            this.enabled = true;
            conveyor.SetState(ConveyorState.Working);
        }

        public void Stop()
        {
            if (!isWorking) return;
            conveyor.SetState(ConveyorState.Waiting);
            this.enabled = false;
            isWorking = false;
        }

        private void AddCargoToConveyor()
        {
            if (conveyor.ConveyorBelt.StartPointFilled) return;
            var cargoEntity = SpawnCargo();
            conveyor.ConveyorBelt.AddCargoOnStartPoint(cargoEntity);
        }

        private CargoEntity SpawnCargo()
        {
            var cargoObject = PoolsManager.GetGameObjectsPool(cargoList[randomCargoTypes[currentCargoIndex]].PoolName).Take();
            var cargoEntity = cargoObject.GetComponent<CargoEntity>();
            currentCargoIndex++;
            if (currentCargoIndex == cargoList.Count)
            {
                randomCargoTypes.Shuffle(random);
                currentCargoIndex = 0;
            }
            return cargoEntity;
        }

        private void Update()
        {
            if (conveyor.CurrentState == ConveyorState.Working)
            {
                if (!conveyor.ConveyorBelt.StartPointFilled)
                {
                    AddCargoToConveyor();
                }
            }
        }
    }
}