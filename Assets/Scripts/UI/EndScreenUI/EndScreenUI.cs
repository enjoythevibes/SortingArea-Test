using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace TestGame.UI.EndScreen
{
    public class EndScreenUI : MonoBehaviour
    {
        [SerializeField] private Image endScreenPanel = default;
        [SerializeField] private TextMeshProUGUI endScreenLabel = default;
        [SerializeField] private TextMeshProUGUI endScreenScoreField = default;
        private string endScreenScoreFieldFormat;

        private void Awake()
        {
            endScreenPanel.color = new Color(endScreenPanel.color.r, endScreenPanel.color.g, endScreenPanel.color.b, 0f);
            endScreenLabel.color = new Color(endScreenLabel.color.r, endScreenLabel.color.g, endScreenLabel.color.b, 0f);
            endScreenScoreField.color = new Color(endScreenScoreField.color.r, endScreenScoreField.color.g, endScreenScoreField.color.b, 0f);
            Managers.GameLoop.OnEndGameNotify += OnEndGame;
            endScreenScoreFieldFormat = endScreenScoreField.text;
        }

        private IEnumerator ShowEndScreenSmooth()
        {
            var timer = 0f;
            while (timer <= 1f)
            {
                endScreenPanel.color = new Color(endScreenPanel.color.r, endScreenPanel.color.g, endScreenPanel.color.b, timer);
                endScreenLabel.color = new Color(endScreenLabel.color.r, endScreenLabel.color.g, endScreenLabel.color.b, timer);
                endScreenScoreField.color = new Color(endScreenScoreField.color.r, endScreenScoreField.color.g, endScreenScoreField.color.b, timer);
                timer += Time.deltaTime;
                yield return null;
            }
            endScreenPanel.color = new Color(endScreenPanel.color.r, endScreenPanel.color.g, endScreenPanel.color.b, 1f);
            endScreenLabel.color = new Color(endScreenLabel.color.r, endScreenLabel.color.g, endScreenLabel.color.b, 1f);
            endScreenScoreField.color = new Color(endScreenScoreField.color.r, endScreenScoreField.color.g, endScreenScoreField.color.b, 1f);
            StartCoroutine(WaitPlayerInput());
            yield break;
        }

        private IEnumerator WaitPlayerInput()
        {
            yield return new WaitUntil(() => Player.PlayerInputManager.RKeyDown);
            SceneManager.LoadScene(0);
        }

        private void OnEndGame()
        {
            endScreenScoreField.SetText(endScreenScoreFieldFormat, Player.PlayerScores.Scores);
            StartCoroutine(ShowEndScreenSmooth());
        }

        private void OnDestroy() 
        {
            Managers.GameLoop.OnEndGameNotify -= OnEndGame;
        }
    }
}