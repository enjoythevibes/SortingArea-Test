using TestGame.Conveyor.Belt;
using TestGame.Door;
using UnityEngine;
using TestGame.Conveyor.LampsPanel;

namespace TestGame.Conveyor
{
    public class Conveyor : MonoBehaviour, IConveyor
    {
        [SerializeField] private GameObject lampsPanelObject = default;
        [SerializeField] private GameObject conveyorBeltObject = default;
        [SerializeField] private GameObject conveyorDoorObject = default;
        [SerializeField] private ConveyorState currentState = default;
        private IConveyorLampsPanel lampsPanel;
        private IConveyorBelt conveyorBelt;
        private IDoor conveyorDoor;

        public ConveyorState CurrentState => currentState;
        public IConveyorBelt ConveyorBelt => conveyorBelt;

        private void OnValidate()
        {
            if (lampsPanelObject != null) lampsPanelObject.GetComponentWithInterface<IConveyorLampsPanel>();
            if (conveyorBeltObject != null) conveyorBeltObject.GetComponentWithInterface<IConveyorBelt>();
            if (conveyorDoorObject != null) conveyorDoorObject.GetComponentWithInterface<IDoor>();
        }

        private void Awake()
        {
            lampsPanel = lampsPanelObject.GetComponentWithInterface<IConveyorLampsPanel>();
            conveyorBelt = conveyorBeltObject.GetComponentWithInterface<IConveyorBelt>();
            conveyorDoor = conveyorDoorObject.GetComponentWithInterface<IDoor>();
        }

        private void Start()
        {
            var setState = currentState;
            currentState = currentState + 1;
            SetState(setState);
        }

        public void SetState(ConveyorState state)
        {
            if (currentState == state) return;
            lampsPanel.SetState(state);
            if (state == ConveyorState.Working)
            {
                conveyorBelt.Run();
                conveyorDoor.Open();
            }
            else
            if (state == ConveyorState.Waiting || state == ConveyorState.Stopped)
            {
                conveyorBelt.Stop();
                conveyorDoor.Close();
            }
            currentState = state;
        }
    }
}