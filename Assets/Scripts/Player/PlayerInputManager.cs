using UnityEngine;

namespace TestGame.Player
{
    public class PlayerInputManager : MonoBehaviour 
    {
        public static Vector2 PlayerInput { private set; get; }        
        public static bool EnterDown { private set; get; }
        public static bool EKeyDown { private set; get; }
        public static bool RKeyDown { private set; get; }

        private void Update()
        {
            var xInput = Input.GetAxis("Horizontal");
            var yInput = Input.GetAxis("Vertical");

            PlayerInput = new Vector2(xInput, yInput);
            EnterDown = Input.GetKeyDown(KeyCode.Return);
            EKeyDown = Input.GetKeyDown(KeyCode.E);
            RKeyDown = Input.GetKeyDown(KeyCode.R);
        }
    }
}