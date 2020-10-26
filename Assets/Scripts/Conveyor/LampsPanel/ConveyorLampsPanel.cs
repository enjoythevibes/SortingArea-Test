using TestGame.Lamp;
using UnityEngine;

namespace TestGame.Conveyor.LampsPanel
{
    public class ConveyorLampsPanel : MonoBehaviour, IConveyorLampsPanel
    {
        [SerializeField] private SimpleLamp greenLamp = default;
        [SerializeField] private SimpleLamp blueLamp = default;
        [SerializeField] private BlinkingLamp redLamp = default;
        [SerializeField] private ConveyorState conveyorState = default;
        
        public ConveyorState ConveyorState => conveyorState;

        private void Start()
        {
            var setState = conveyorState;
            conveyorState = conveyorState + 1;            
            SetState(setState);
        }

        public void SetState(ConveyorState state)
        {
            if (conveyorState == state) return;
            DisableAll();
            switch (state)
            {
                case ConveyorState.Working:
                    greenLamp.Enable();
                    break;
                case ConveyorState.Waiting:
                    blueLamp.Enable();
                    break;
                case ConveyorState.Stopped:
                    redLamp.SetSimpleMode();
                    redLamp.Enable();
                    break;
                case ConveyorState.TimeRunsOut:
                    redLamp.SetBlinkMode();
                    redLamp.Enable();
                    break;
            }
            conveyorState = state;
        }

        private void DisableAll()
        {
            greenLamp.Disable();
            blueLamp.Disable();
            redLamp.Disable();
        }
    }
}
