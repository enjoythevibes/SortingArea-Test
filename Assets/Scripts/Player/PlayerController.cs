using System.Collections;
using TestGame.Character;
using TestGame.Entities;
using UnityEngine;

namespace TestGame.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform cameraTransform = default;
        [SerializeField] private Transform cargoHoldPosition = default;
        private ICharacterInteraction characterInteraction;
        private ICharacterMovementController characterMovementController;
        private ICharacterStateController characterStateController;
        private CargoEntity cargoInHands;
        private bool withCargo;

        private void Awake() 
        {
            characterMovementController = gameObject.GetComponentWithInterface<ICharacterMovementController>();
            characterStateController = gameObject.GetComponentWithInterface<ICharacterStateController>();
            characterInteraction = gameObject.GetComponentWithInterface<ICharacterInteraction>();
            Managers.GameLoop.OnEndGameNotify += OnGameEnd;
        }

        private void OnGameEnd()
        {
            this.enabled = false;
            characterMovementController.Move(Vector3.zero, 0f);
        }

        private void Start()
        {
            characterStateController.SetCharacterState(CharacterState.FreeHands);
        }

        private IEnumerator WaitAndSetCargo(float translationTime)
        {
            var timer = 0f;
            while (timer < translationTime)
            {
                timer += Time.deltaTime;
                yield return null;
            }            
            cargoInHands.CargoTransform.parent = cargoHoldPosition;
            cargoInHands.CargoTransform.position = cargoHoldPosition.position;
            cargoInHands.CargoTransform.rotation = cargoHoldPosition.rotation;
            yield break;
        }

        private void OnDestroy()
        {
            Managers.GameLoop.OnEndGameNotify -= OnGameEnd;
        }

        private void Update()
        {
            var playerInput = PlayerInputManager.PlayerInput;
            
            var forwardVector = cameraTransform.forward;
                forwardVector.y = 0f;
                forwardVector.Normalize();

            forwardVector *= playerInput.y;

            var rightVector = cameraTransform.right * playerInput.x;
                            
            var inputVector = forwardVector + rightVector;
            var inputMagnitude = inputVector.magnitude;
            if (inputMagnitude > 1f)
            {
                inputVector.Normalize();
                inputMagnitude = 1f;
            }
            characterMovementController.Move(inputVector, inputMagnitude);
            if (PlayerInputManager.EKeyDown)
            {
                if (characterStateController.IsTranslation == false)
                {
                    if (withCargo == false)
                    {
                        var interaction = characterInteraction.InteractionTake();
                        if (interaction.Item1)
                        {
                            var translationTime = characterStateController.GetTranslationTime(CharacterState.WithCargo);
                            characterStateController.SetCharacterState(CharacterState.WithCargo);
                            withCargo = true;
                            cargoInHands = interaction.Item2;
                            var direction = cargoInHands.CargoTransform.position - transform.position;
                                direction.y = 0f;
                                direction.Normalize();
                            var rotation = Quaternion.LookRotation(direction);
                            transform.rotation = rotation;
                            StartCoroutine(WaitAndSetCargo(translationTime));
                        }
                    }
                    else
                    if (withCargo == true)
                    {
                        var interaction = characterInteraction.InteractionPut(cargoInHands);
                        characterStateController.SetCharacterState(CharacterState.FreeHands);
                        withCargo = false;
                        cargoInHands.CargoTransform.parent = null;
                        if (!interaction)
                        {
                            cargoInHands.SetFallState();
                        }
                        cargoInHands = null;                            
                    }
                }
            }
        }
    }
}