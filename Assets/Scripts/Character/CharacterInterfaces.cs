using System;
using UnityEngine;

namespace TestGame.Character
{
    public interface ICharacterMovementController
    {
        float InputMagnitude { get; }
        void Move(Vector3 inputVector, float inputMahnitude);
        void SetMovementSpeed(float movementSpeed);
        void SetBlockMovement(bool status);
    }

    public interface ICharacterAnimatorController
    {
        void SetAnimation(CharacterState nextState);
    }

    public interface ICharacterStateController
    {
        bool IsTranslation { get; }
        float GetTranslationTime(CharacterState nextState);
        void SetCharacterState(CharacterState nextState);
    }

    public interface ICharacterInteraction
    {
        ValueTuple<bool, Entities.CargoEntity> InteractionTake();
        bool InteractionPut(Entities.CargoEntity cargoEntity);
    }
}