using UnityEngine;

namespace TestGame.Events
{
    [RequireComponent(typeof(AudioSource))]
    public class CharacterFootStepsAnimEvents : MonoBehaviour
    {
        [SerializeField] private AudioClip[] footStepSounds = default;
        private AudioSource audioSource;
        private int currentStep;

        private void Awake() 
        {
            audioSource = GetComponent<AudioSource>();
            if (footStepSounds.Length == 0) Debug.LogError("FootStepsSounds Array must be more than 0");
        }

        public void PlaySoundStep()
        {
            audioSource.PlayOneShot(footStepSounds[currentStep]);            
            currentStep++;
            if (currentStep >= footStepSounds.Length)
            {
                currentStep = 0;
            }
        }
    }
}