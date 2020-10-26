using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestGame.Character
{
    public class CharacterAnimatorControllerYBot : MonoBehaviour, ICharacterAnimatorController
    {
        [SerializeField] private string movementSpeedName = "forwardSpeed";
        [SerializeField] private string takeCargoName = "takeCargo";
        [SerializeField] private string putCargoName = "putCargo";

        private ICharacterMovementController characterMovementController;
        private Animator animator;
        private int movementSpeedHash;
        private int takeCargoHash;
        private int putCargoHash;

        private void Awake()
        {
            characterMovementController = gameObject.GetComponentWithInterface<ICharacterMovementController>();
            animator = GetComponent<Animator>();
            movementSpeedHash = Animator.StringToHash(movementSpeedName);
            takeCargoHash = Animator.StringToHash(takeCargoName);
            putCargoHash = Animator.StringToHash(putCargoName);
        }

        private void Update()
        {
            animator.SetFloat(movementSpeedHash, characterMovementController.InputMagnitude);
        }

        public void SetAnimation(CharacterState nextState)
        {
            if (nextState == CharacterState.WithCargo)
            {
                animator.ResetTrigger(putCargoHash);
                animator.SetTrigger(takeCargoHash);
            }
            else
            if (nextState == CharacterState.FreeHands)
            {
                animator.ResetTrigger(takeCargoHash);
                animator.SetTrigger(putCargoHash);
            }
        }
    }
}