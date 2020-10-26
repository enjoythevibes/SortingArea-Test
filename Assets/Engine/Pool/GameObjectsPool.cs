using System.Collections.Generic;
using UnityEngine;

namespace enjoythevibes.Pool
{
    public class GameObjectsPool
    {
        private GameObject objectToPoolPrefab;
        private Stack<GameObject> pooledObjects = new Stack<GameObject>();
        private GameObject gameObjectsPool;

        public GameObjectsPool(GameObject objectToPoolPrefab, int amountToPool, string name)
        {
            this.gameObjectsPool = new GameObject("GameObjects Pool " + name);
            this.objectToPoolPrefab = objectToPoolPrefab;
            for (int i = 0; i < amountToPool; i++)
            {   
                var objectToPool = MonoBehaviour.Instantiate(objectToPoolPrefab, Vector3.zero, objectToPoolPrefab.transform.rotation, gameObjectsPool.transform);
                objectToPool.SetActive(false);
                pooledObjects.Push(objectToPool);
            }
        }

        public GameObject Take()
        {
            if (pooledObjects.Count > 0)
            {
                var returnObject = pooledObjects.Pop();
                returnObject.SetActive(true);
                returnObject.transform.parent = null;
                return returnObject;
            }
            else
            {
                var returnObject = MonoBehaviour.Instantiate(objectToPoolPrefab, Vector3.zero, Quaternion.identity);
                return returnObject;
            }
        }

        public void Put(GameObject objectToPool)
        {
            if (objectToPool.tag == objectToPoolPrefab.tag)
            {
                objectToPool.transform.parent = gameObjectsPool.transform;
                objectToPool.SetActive(false);
                pooledObjects.Push(objectToPool);
            }
        }
    }
}