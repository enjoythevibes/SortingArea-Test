using UnityEngine;

namespace TestGame.Character
{
    [RequireComponent(typeof(CharacterController))]    
    public class CharacterMovementController : MonoBehaviour, ICharacterMovementController
    {
        private CharacterController characterController;
        private Vector3 gravityVector;
        private Vector3 inputVector;
        private float movementSpeed;
        private bool blockMovement;

        public float InputMagnitude { private set; get; }

        private void Awake() 
        {
            characterController = GetComponent<CharacterController>();    
        }

        public void Move(Vector3 inputVector, float inputMagnitude)
        {
            if (blockMovement)
            {
                InputMagnitude = 0f;
                return;
            }
            this.inputVector = inputVector * movementSpeed;
            InputMagnitude = inputMagnitude;
        }   

        public void SetMovementSpeed(float movementSpeed)
        {
            this.movementSpeed = movementSpeed;
        }

        public void SetBlockMovement(bool status)
        {
            blockMovement = status;
        }    

        private void Update() 
        {
            var deltaTime = Time.deltaTime;
            if (characterController.isGrounded)
            {
                gravityVector = Vector3.zero;
            }
            gravityVector += Physics.gravity * deltaTime;

            var movementVector = gravityVector;
            if (blockMovement == false)
            {
                movementVector += inputVector;
            }
            
            if (InputMagnitude > 0f && blockMovement == false)
            {
                var targetRotation = Quaternion.LookRotation(inputVector.normalized);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 720f * deltaTime);
            }
            if (!characterController.isGrounded || InputMagnitude > 0f)
            {
                characterController.Move(movementVector * deltaTime);
            }
        }
    }
}