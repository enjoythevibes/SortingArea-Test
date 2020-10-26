using UnityEngine;

namespace TestGame.Lamp
{
    public class SimpleLamp : MonoBehaviour
    {
        protected Light currentLamp;
        [SerializeField]
        protected bool lampEnabled;

        protected virtual void Awake()
        {
            currentLamp = GetComponent<Light>();
        }

        protected virtual void Start() 
        {
            if (lampEnabled) 
            {
                lampEnabled = false;
                Enable();
            }
            else
            {
                lampEnabled = true;
                Disable();
            }
        }

        public virtual void Enable()
        {
            if (lampEnabled) return;
            currentLamp.enabled = true;
            currentLamp.range = currentLamp.range; // to fix bug(no lighting on objects)
            lampEnabled = true;
        }

        public virtual void Disable()
        {
            if (!lampEnabled) return;
            currentLamp.enabled = false;
            lampEnabled = false;
        }
    }    
}