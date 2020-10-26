using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestGame.Character
{
    public class CharacterStateController : MonoBehaviour, ICharacterStateController
    {
        [SerializeField] private float defaultMovementSpeed = 3f;
        [SerializeField] private float movementSpeedWithCargo = 1.1f;
        private ICharacterMovementController characterMovementController;
        private ICharacterAnimatorController characterAnimatorController;
        private CharacterState currentCharacterState;
        private bool translation;

        public bool IsTranslation => translation;

        private static readonly Dictionary<CharacterState, Dictionary<CharacterState, float>> translationsTimeList = 
        new Dictionary<CharacterState, Dictionary<CharacterState, float>>()
        {     // from state          // to state
            { CharacterState.FreeHands, new Dictionary<CharacterState, float>()
                                        {
                                            { CharacterState.WithCargo, 1f }
                                        }},
            { CharacterState.WithCargo, new Dictionary<CharacterState, float>()
                                        {
                                            { CharacterState.FreeHands, 0.5f }
                                        }}
        };

        private void Awake()
        {
            characterMovementController = gameObject.GetComponentWithInterface<ICharacterMovementController>();
            characterAnimatorController = gameObject.GetComponentWithInterface<ICharacterAnimatorController>();
        }

        private IEnumerator TranslationTimer(float translationTime)
        {
            var timer = 0f;
            characterMovementController.SetBlockMovement(true);
            translation = true;
            while (timer <= translationTime)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            characterMovementController.SetBlockMovement(false);
            translation = false;
            yield break;
        }

        public void SetCharacterState(CharacterState nextState)
        {
            if (translation) return;
            var translationDuration = 0f;
            if (translationsTimeList.TryGetValue(currentCharacterState, out var toStateList))
            {
                toStateList.TryGetValue(nextState, out translationDuration);
            }
            switch (nextState)
            {
                case CharacterState.FreeHands:
                    characterMovementController.SetMovementSpeed(defaultMovementSpeed);
                    characterAnimatorController.SetAnimation(nextState);
                    break;
                case CharacterState.WithCargo:
                    characterMovementController.SetMovementSpeed(movementSpeedWithCargo);
                    characterAnimatorController.SetAnimation(nextState);
                    break;
            }
            currentCharacterState = nextState;
            if (translationDuration > 0f)
                StartCoroutine(TranslationTimer(translationDuration));
        }

        public float GetTranslationTime(CharacterState nextState)
        {
            var translationDuration = 0f;
            if (translationsTimeList.TryGetValue(currentCharacterState, out var toStateList))
            {
                toStateList.TryGetValue(nextState, out translationDuration);
            }
            return translationDuration;
        }
    }
}