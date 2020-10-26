using enjoythevibes.Managers;
using UnityEngine;

namespace TestGame.Entities
{
    public class CargoEntity : MonoBehaviour
    {
        public enum Type
        {
            Metal = 1,
            Plastic,
            Wood
        }        
        [SerializeField] private CargoList cargoList = default;
        [SerializeField] private float width = 1.5f;
        [SerializeField] private Type cargoEntityType = default;
        private Transform cargoTransform;

        public float Width => width;
        public Type CargoEntityType => cargoEntityType;
        public Transform CargoTransform => cargoTransform;

        private void Awake()
        {
            cargoTransform = transform;            
            this.enabled = false;
        }

        public void SetFallState()
        {
            this.enabled = true;
        }

        public void DestroyObject()
        {
            transform.rotation = Quaternion.identity;
            PoolsManager.GetGameObjectsPool(cargoList[cargoEntityType].PoolName).Put(gameObject);
        }

        private void Update() 
        {
            transform.position += Vector3.down * Time.deltaTime * 5f;
            if (transform.position.y < -1f)
            {
                this.enabled = false;
                DestroyObject();
            }    
        }
    }
}