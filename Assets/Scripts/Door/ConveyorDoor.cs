using UnityEngine;

namespace TestGame.Door
{
    public class ConveyorDoor : MonoBehaviour, IDoor
    {
        [SerializeField] private bool opened = default;
        [SerializeField] private string openName = "Open";
        [SerializeField] private string closeName = "Close";
        private int openHash;
        private int closeHash;
        private Animator animator;
        
        public bool Opened => opened;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            openHash = Animator.StringToHash(openName);
            closeHash = Animator.StringToHash(closeName);
        }

        private void Start() 
        {
            if (opened)
                animator.SetTrigger(openHash);
            else
                animator.SetTrigger(closeHash);
        }

        public void Open()
        {
            if (opened) return;
            animator.SetTrigger(openHash);
            opened = true;
        }

        public void Close()
        {
            if (!opened) return;
            animator.SetTrigger(closeHash);
            opened = false;
        }
    }
}