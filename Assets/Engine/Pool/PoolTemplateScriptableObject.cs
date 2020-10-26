using UnityEngine;

namespace enjoythevibes.Pool
{
    [CreateAssetMenu(fileName = "PoolTemplate", menuName = "enjoythevibes/PoolTemplate", order = 100)]
    public class PoolTemplateScriptableObject : ScriptableObject
    {
        [SerializeField] private string templateTagName = default;
        [SerializeField] private GameObject templatePrefab = default;
        [SerializeField] private int amountToPool = default;
        
        public string TemplateTagName => templateTagName;
        public GameObject TemplatePrefab => templatePrefab;
        public int AmountToPool => amountToPool;
    }
}