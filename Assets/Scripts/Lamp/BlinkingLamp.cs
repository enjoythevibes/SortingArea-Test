using System.Collections;
using UnityEngine;

namespace TestGame.Lamp
{
    public class BlinkingLamp : SimpleLamp
    {
        [SerializeField]
        private float blinkingRate = 1f;
        private WaitForSeconds waitForSeconds;
        private Coroutine blinkingCoroutine;
        public enum Mode
        {
            Simple,
            Blinking
        }
        [SerializeField]
        private Mode lampMode;
        public Mode LampMode => lampMode;

        protected override void Awake()
        {
            base.Awake();
            waitForSeconds = new WaitForSeconds(blinkingRate);
        }
        
        public override void Enable()
        {
            if (this.lampEnabled) return;
            if (lampMode == Mode.Blinking)
            {
                blinkingCoroutine = StartCoroutine(DoBlink());
            }
            base.Enable();
        }

        public override void Disable()
        {
            if (!this.lampEnabled) return;
            if (lampMode == Mode.Blinking)
            {
                if (blinkingCoroutine != null)
                    StopCoroutine(blinkingCoroutine);
            }
            base.Disable();
        }

        public void SetBlinkMode()
        {
            var wasEnabled = this.lampEnabled;
            Disable();
            lampMode = Mode.Blinking;
            if (wasEnabled)
            {
                Enable();
            }
        }

        public void SetSimpleMode()
        {
            var wasEnabled = this.lampEnabled;
            Disable();
            lampMode = Mode.Simple;
            if (wasEnabled)
            {
                Enable();
            }
        }

        private IEnumerator DoBlink()
        {
            while (true)
            {
                currentLamp.enabled = !currentLamp.enabled;
                currentLamp.range = currentLamp.range;
                yield return waitForSeconds;
            }            
        }
    }
}