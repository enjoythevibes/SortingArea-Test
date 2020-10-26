using UnityEngine;

namespace TestGame.Character
{
    public class CharacterAnimatorController : MonoBehaviour, ICharacterAnimatorController
    {
        [SerializeField] private string movementSpeedName = "MoveSpeed";
        [SerializeField] private string actionPickObjectName = "ActionPickObject";
        [SerializeField] private string actionDropObjectName = "ActionDropObject";
        [SerializeField] private string moveModeName = "MoveMode";
        [SerializeField] private string actionModeName = "ActionMode";
        private ICharacterMovementController characterMovementController;
        private Animator animator;
        private int movementSpeedHash;
        private int actionPickObjectHash;
        private int actionDropObjectHash;
        private int moveModeHash;
        private int actionModeHash;

        private void Awake() 
        {
            characterMovementController = gameObject.GetComponentWithInterface<ICharacterMovementController>();
            animator = GetComponent<Animator>();
            movementSpeedHash = Animator.StringToHash(movementSpeedName);
            actionPickObjectHash = Animator.StringToHash(actionPickObjectName);
            actionDropObjectHash = Animator.StringToHash(actionDropObjectName);
            moveModeHash = Animator.StringToHash(moveModeName);
            actionModeHash = Animator.StringToHash(actionModeName);
        }

        private void Update() 
        {
            animator.SetFloat(movementSpeedHash, characterMovementController.InputMagnitude);
        }

        public void SetAnimation(CharacterState nextState)
        {
            if (nextState == CharacterState.WithCargo)
            {
                animator.SetTrigger(actionPickObjectHash);
                animator.SetInteger(actionModeHash, 0);
                animator.SetInteger(moveModeHash, 1);
            }
            else
            if (nextState == CharacterState.FreeHands)
            {
                animator.SetTrigger(actionDropObjectHash);
                animator.SetInteger(actionModeHash, 1);
                animator.SetInteger(moveModeHash, 0);
            }
        }
    }
}