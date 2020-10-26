using TestGame.Entities;
using UnityEngine;

namespace TestGame.Conveyor.Belt
{
    public partial class ConveyorBelt : MonoBehaviour, IConveyorBelt
    {
        [SerializeField] private ConveyorBeltAnimation conveyorBeltAnimation = default;
        [SerializeField] private ConveyorBeltCargoMover conveyorBeltCargoMover = default;
        private bool isWorking;

        public bool IsWorking => isWorking;
        public bool StartPointFilled => conveyorBeltCargoMover.StartSlotFilled;
        public bool EndPointFilled => conveyorBeltCargoMover.EndSlotFilled;
        
        private void Awake()
        {
            conveyorBeltAnimation.Initialize(new MaterialPropertyBlock());
            conveyorBeltCargoMover.Initialize();
        }

        private void Start() 
        {
            var setWorking = isWorking;
            isWorking = !isWorking;
            if (setWorking)
                Run();
            else
                Stop();    
        }

        public void Run()
        {
            if (isWorking) return;
            conveyorBeltAnimation.Play();
            isWorking = true;
            this.enabled = true;
        }

        public void Stop()
        {
            if (!isWorking) return;
            conveyorBeltAnimation.Stop();
            isWorking = false;
            this.enabled = false;
        }

        public void AddCargoOnStartPoint(CargoEntity cargoEntity) => conveyorBeltCargoMover.AddCargoOnStartSlot(cargoEntity);
        public CargoEntity TakeCargoFromEndPoint() => conveyorBeltCargoMover.TakeCargoFromEndSlot(this);
        public CargoEntity PeekCargoFromEndPoint() => conveyorBeltCargoMover.PeekCargoFromEndPoint();
        public void RemoveCargoFromEndPoint() => conveyorBeltCargoMover.RemoveCargoFromEndPoint();
        public void ClearBelt() => conveyorBeltCargoMover.ClearBelt();

        private void Update()
        {
            conveyorBeltCargoMover.Tick(Time.deltaTime);
            if (conveyorBeltCargoMover.Moving)
            {
                conveyorBeltAnimation.Play();    
            }
            else
            {
                conveyorBeltAnimation.Stop();
            }
        }        
    }
}