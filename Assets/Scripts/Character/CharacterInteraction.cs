using System;
using UnityEngine;
using TestGame.Triggers;

namespace TestGame.Character
{
    public class CharacterInteraction : MonoBehaviour, ICharacterInteraction
    {
        [SerializeField] private LayerMask interactionLayer = default;
        [SerializeField] private Vector3 characterChestOffsetPosition = new Vector3(0f, 1.3f, 0f);

        private void Awake() 
        {
            if (interactionLayer == default)
            {
                interactionLayer = LayerMask.GetMask("Interaction");
            }    
        }

        public ValueTuple<bool, Entities.CargoEntity> InteractionTake()
        {
            if (Physics.Raycast(transform.position + characterChestOffsetPosition, transform.forward, out var hitInfo, 1f, interactionLayer))
            {
                var conveyorInteractionTrigger = hitInfo.transform.GetComponent<IConveyorInteractionTrigger>();
                var direction = (transform.position - hitInfo.transform.position);
                    direction.y = 0f;
                    direction.Normalize();
                var dotProduct = Mathf.Abs(Vector3.Dot(hitInfo.transform.forward, direction));
                if (dotProduct > 0f && dotProduct < 0.6f)
                {
                    var conveyorSubmitter = conveyorInteractionTrigger.Conveyor;
                    if (conveyorInteractionTrigger.CurrentConveyorType == ConveyorType.Submitter)
                    {
                        if (conveyorSubmitter.ConveyorBelt.EndPointFilled)
                        {
                            var cargoEntity = conveyorSubmitter.ConveyorBelt.TakeCargoFromEndPoint();
                            return (true, cargoEntity);
                        }                        
                    }
                }
            }
            return (false, null);
        }

        public bool InteractionPut(Entities.CargoEntity cargoEntity)
        {
            if (Physics.Raycast(transform.position + characterChestOffsetPosition, transform.forward, out var hitInfo, 1f, interactionLayer))
            {
                var conveyorInteractionTrigger = hitInfo.transform.GetComponent<IConveyorInteractionTrigger>();
                var direction = (transform.position - hitInfo.transform.position);
                    direction.y = 0f;
                    direction.Normalize();
                var dotProduct = Mathf.Abs(Vector3.Dot(hitInfo.transform.forward, direction));
                if (dotProduct > 0f && dotProduct < 0.6f)
                {
                    var conveyorReceiver = conveyorInteractionTrigger.Conveyor;
                    if (conveyorInteractionTrigger.CurrentConveyorType == ConveyorType.Receiver)
                    {
                        if (!conveyorReceiver.ConveyorBelt.StartPointFilled && (conveyorReceiver.CurrentState == Conveyor.ConveyorState.Working || conveyorReceiver.CurrentState == Conveyor.ConveyorState.TimeRunsOut))
                        {
                            conveyorReceiver.ConveyorBelt.AddCargoOnStartPoint(cargoEntity);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}