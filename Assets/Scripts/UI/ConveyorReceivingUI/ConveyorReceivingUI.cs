using System.Collections;
using UnityEngine;
using TestGame.Conveyor.Receiver;
using UnityEngine.UI;
using TestGame.Entities;
using TMPro;

namespace TestGame.UI.Conveyor
{
    public class ConveyorReceivingUI : MonoBehaviour
    {
        [SerializeField] private CargoList cargoList = default;
        [SerializeField] private GameObject conveyorOrderManagerObject = default;
        [SerializeField] private Image cargoEntityImage1 = default;
        [SerializeField] private Image cargoEntityImage2 = default;
        [SerializeField] private TextMeshProUGUI ordersLeftField = default;
        [SerializeField] private Image timerImage = default;
        [SerializeField] private string showUIName = "Show";
        [SerializeField] private string hideUIName = "Hide";
        [SerializeField] private string nextOrderUIName = "Next Order";
        [SerializeField] private string showTimerUIName = "Show Timer";
        private IConveyorOrderManager conveyorOrderManager;
        private Animator animator;
        private int showUIHash;
        private int hideUIHash;
        private int nextOrderUIHash;
        private int showTimerUIHash;
        private readonly string ordersLeftFieldformat = "{0}";

        private void OnValidate()
        {
            if (conveyorOrderManagerObject != null)
                conveyorOrderManagerObject.GetComponentWithInterface<IConveyorOrderManager>();
        }

        private void Awake()
        {
            conveyorOrderManager = conveyorOrderManagerObject.GetComponentWithInterface<IConveyorOrderManager>();
            showUIHash = Animator.StringToHash(showUIName);
            hideUIHash = Animator.StringToHash(hideUIName);
            nextOrderUIHash = Animator.StringToHash(nextOrderUIName);
            showTimerUIHash = Animator.StringToHash(showTimerUIName);
            animator = GetComponent<Animator>();
            conveyorOrderManager.OnNextOrderNotify += OnNextOrder;
            conveyorOrderManager.OnNextOrderItemNotify += OnNextOrderItem;
            conveyorOrderManager.OnOrderCompletedNotify += OnOrderCompleted;
            conveyorOrderManager.OnOrderFailedNotify += OnOrderFailed;
            conveyorOrderManager.OnOrderTimeRunsOutNotify += OnOrderTimeRunsOut;
        }

        private void OnNextOrder(IOrderData orderData)
        {
            animator.SetTrigger(showUIHash);
            var sprite1 = cargoList[orderData.OrderSequence[0]].CargoSprite;
            cargoEntityImage1.sprite = sprite1;
            var sprite2 = cargoList[orderData.OrderSequence[1]].CargoSprite;
            cargoEntityImage2.sprite = sprite2;
            ordersLeftField.SetText(ordersLeftFieldformat, orderData.OrderSize - orderData.CurrentOrderItemIndex);
            cargoEntityImage1.gameObject.SetActive(true);
            cargoEntityImage2.gameObject.SetActive(true);
        }

        private void OnNextOrderItem(IOrderData orderData)
        {
            if (orderData.CurrentOrderItemIndex >= orderData.OrderSize)
            {
                cargoEntityImage1.gameObject.SetActive(false);                
            }
            else
            {
                var sprite1 = cargoList[orderData.OrderSequence[orderData.CurrentOrderItemIndex]].CargoSprite;
                cargoEntityImage1.sprite = sprite1;
            }
            if (orderData.CurrentOrderItemIndex + 1 >= orderData.OrderSize)
            {
                cargoEntityImage2.gameObject.SetActive(false);
            }
            else
            {
                var sprite2 = cargoList[orderData.OrderSequence[orderData.CurrentOrderItemIndex + 1]].CargoSprite;
                cargoEntityImage2.sprite = sprite2;
            }
            ordersLeftField.SetText(ordersLeftFieldformat, orderData.OrderSize - orderData.CurrentOrderItemIndex);
        }

        private void OnOrderCompleted(IOrderData orderData)
        {
            animator.SetTrigger(hideUIHash);
        }

        private void OnOrderFailed(IOrderData orderData)
        {
            animator.SetTrigger(hideUIHash);
        }

        private void OnOrderTimeRunsOut(IOrderData orderData)
        {
            animator.SetTrigger(showTimerUIHash);
            StartCoroutine(TimerAnimation(orderData));
        }

        private void OnDestroy()
        {
            conveyorOrderManager.OnNextOrderNotify -= OnNextOrder;
            conveyorOrderManager.OnNextOrderItemNotify -= OnNextOrderItem;
            conveyorOrderManager.OnOrderCompletedNotify -= OnOrderCompleted;
            conveyorOrderManager.OnOrderFailedNotify -= OnOrderFailed;
            conveyorOrderManager.OnOrderTimeRunsOutNotify -= OnOrderTimeRunsOut;
        }

        private IEnumerator TimerAnimation(IOrderData orderData)
        {
            var timer = 0f;
            timerImage.fillAmount = 0f;
            while (timer <= orderData.TimePerOrderItem)
            {
                timerImage.fillAmount = timer / orderData.TimePerOrderItem;
                timer += Time.deltaTime;
                yield return null;
            }
            timerImage.fillAmount = 1f;
            yield break;
        }
    }
}