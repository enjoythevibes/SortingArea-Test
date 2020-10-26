using System.Collections;
using UnityEngine;
using TestGame.Conveyor.Submitter;
using TestGame.Conveyor.Receiver;

namespace TestGame.Managers
{
    public class GameLoop : MonoBehaviour
    {
        private static GameLoop instance;
        
        [SerializeField] private GameObject submitterConveyorManagerObject = default;
        [SerializeField] private GameObject[] conveyorOrderManagerObjects = default;
        [SerializeField] private float gameTimeLimit = 180f;
        private ISubmitterConveyorManager submitterConveyorManager;
        private IConveyorOrderManager[] conveyorOrderManagers;
        private float gameTimer;

        public static float GameTimeLimit => instance.gameTimeLimit;
        public static float GameTimer => instance.gameTimer;

        public delegate void OnEndGame();
        public static event OnEndGame OnEndGameNotify;

        private void OnValidate()
        {
            if (submitterConveyorManagerObject != null)
                submitterConveyorManagerObject.GetComponentWithInterface<ISubmitterConveyorManager>();
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
            instance = this;
            submitterConveyorManager = submitterConveyorManagerObject.GetComponentWithInterface<ISubmitterConveyorManager>();
            conveyorOrderManagers = new IConveyorOrderManager[conveyorOrderManagerObjects.Length];
            if (conveyorOrderManagerObjects.Length == 0) Debug.LogError("ERROR: ConveyorOrderManagers Length is 0");
            for (int i = 0; i < conveyorOrderManagerObjects.Length; i++)
            {
                conveyorOrderManagers[i] = conveyorOrderManagerObjects[i].GetComponentWithInterface<IConveyorOrderManager>();
            }
            this.enabled = false;
        }

        public static void StartGame()
        {
            instance.submitterConveyorManager.Start();
            instance.StartCoroutine(instance.WaitAndStartOrdering());
        }

        private void EndGame()
        {
            this.enabled = false;
            OnEndGameNotify?.Invoke();
            submitterConveyorManager.Stop();
            foreach (var conveyorOrderManager in conveyorOrderManagers)
            {
                conveyorOrderManager.StopWorking();
            }
        }

        private IEnumerator WaitAndStartOrdering()
        {
            yield return new WaitForSeconds(7f);
            foreach (var conveyorOrderManager in conveyorOrderManagers)
            {
                conveyorOrderManager.StartWorking();
            }
            this.enabled = true;
            yield break;
        }

        private void Update()
        {
            gameTimer += Time.deltaTime;
            if (gameTimer > gameTimeLimit)
            {
                EndGame();                
            }
        }
    }
}