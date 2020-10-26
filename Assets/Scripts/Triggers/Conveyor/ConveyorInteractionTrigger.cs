using UnityEngine;
using TestGame.Conveyor;

namespace TestGame.Triggers
{
    public class ConveyorInteractionTrigger : MonoBehaviour, IConveyorInteractionTrigger
    {       
        [SerializeField] private GameObject conveyorObject = default;
        [SerializeField] private ConveyorType conveyorType = default;
        private IConveyor conveyor;
        
        public IConveyor Conveyor => conveyor;
        public ConveyorType CurrentConveyorType => conveyorType;

        private void OnValidate()
        {
            if (conveyorObject != null) conveyorObject.GetComponentWithInterface<IConveyor>();
        }

        private void Awake()
        {
            conveyor = conveyorObject.GetComponentWithInterface<IConveyor>();
        }
    }
}