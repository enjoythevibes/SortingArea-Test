using System.Collections.Generic;
using UnityEngine;
using enjoythevibes.Pool;

namespace enjoythevibes.Managers
{
    public class PoolsManager : MonoBehaviour
    {
        [SerializeField] private List<PoolTemplateScriptableObject> poolTemplates = default;
        private static PoolsManager instance;
        
        private Dictionary<string, GameObjectsPool> pools = new Dictionary<string, GameObjectsPool>();
        private static Dictionary<string, GameObjectsPool> Pools => instance.pools;

        private void Awake()
        {
            instance = this;
        }

        private void Start() 
        {
            for (int i = 0; i < poolTemplates.Count; i++)
            {
                var gameObjectsPool = new GameObjectsPool(poolTemplates[i].TemplatePrefab, poolTemplates[i].AmountToPool, poolTemplates[i].TemplateTagName);
                pools.Add(poolTemplates[i].TemplateTagName, gameObjectsPool);
            }
        }

        public static GameObjectsPool GetGameObjectsPool(string templateTagName)
        {
            Pools.TryGetValue(templateTagName, out var result);
            return result;
        }
    }
}