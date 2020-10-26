using UnityEngine;
using TMPro;
using TestGame.Conveyor.Receiver;
using TestGame.Player;

namespace TestGame.UI.Game
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI pointsField = default;
        [SerializeField] private TextMeshProUGUI timeLeftField = default;
        [SerializeField] private GameObject[] conveyorOrderManagerObjects = default;
        private IConveyorOrderManager[] conveyorOrderManagers;
        private string pointsFieldFormat;
        private string timeLeftFieldFormat;
        private int lastTime;

        private void OnValidate()
        {
            if (conveyorOrderManagerObjects != null)
            {
                foreach (var conveyorOrderManagerObject in conveyorOrderManagerObjects)
                {
                    conveyorOrderManagerObject.GetComponentWithInterface<IConveyorOrderManager>();                    
                }
            }
        }

        private void Awake() 
        {
            conveyorOrderManagers = new IConveyorOrderManager[conveyorOrderManagerObjects.Length];
            if (conveyorOrderManagerObjects.Length == 0) Debug.LogError("ERROR: ConveyorOrderManagers Length is 0");
            for (int i = 0; i < conveyorOrderManagerObjects.Length; i++)
            {
                conveyorOrderManagers[i] = conveyorOrderManagerObjects[i].GetComponentWithInterface<IConveyorOrderManager>();
                conveyorOrderManagers[i].OnOrderCompletedNotify += OnOrderCompleted;
            }
            pointsFieldFormat = pointsField.text;
            timeLeftFieldFormat = timeLeftField.text;
        }

        private void Start() 
        {
            SetPoints(0);    
        }

        private void OnOrderCompleted(IOrderData orderData)
        {
            PlayerScores.Scores += orderData.OrderSize;
            SetPoints(PlayerScores.Scores);
        }

        private void SetPoints(int points)
        {
            pointsField.SetText(pointsFieldFormat, points);
        }

        private void Update()
        {
            var time = (int)(Managers.GameLoop.GameTimeLimit - Managers.GameLoop.GameTimer);
            if (lastTime != time)
            {
                lastTime = time;
                timeLeftField.SetText(timeLeftFieldFormat, time);
            }
        }
    }
}