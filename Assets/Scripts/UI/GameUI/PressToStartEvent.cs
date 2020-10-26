using System.Collections;
using TestGame.Managers;
using TestGame.Player;
using UnityEngine;

namespace TestGame.UI.Game
{
    public class PressToStartEvent : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(WaitPlayerInput());
        }

        private IEnumerator WaitPlayerInput()
        {
            yield return new WaitUntil(() => PlayerInputManager.EnterDown);
            GameLoop.StartGame();
            gameObject.SetActive(false);
            yield break;
        }
    }
}