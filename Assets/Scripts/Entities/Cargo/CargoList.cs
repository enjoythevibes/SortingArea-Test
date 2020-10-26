using System.Collections.Generic;
using UnityEngine;

namespace TestGame.Entities
{
    [CreateAssetMenu(fileName = "CargoList", menuName = "TestGame/CargoList", order = 99)]
    public class CargoList : ScriptableObject
    {
        [System.Serializable]
        public class CargoItemData
        {
            [SerializeField] private CargoEntity.Type cargoType = default;
            [SerializeField] private string poolName = default;
            [SerializeField] private Sprite cargoSprite = default;
            
            public CargoEntity.Type CargoType => cargoType;
            public string PoolName => poolName;
            public Sprite CargoSprite => cargoSprite;
        }
        [SerializeField] private CargoItemData[] cargoList = default;
        private Dictionary<CargoEntity.Type, CargoItemData> cargoDictionary;
        
        public CargoItemData this[CargoEntity.Type type] => cargoDictionary[type];
        public CargoItemData this[int index] => cargoList[index];

        public int Count => cargoDictionary.Count;

        public void Awake()
        {
            if (cargoDictionary != null) return;
            cargoDictionary = new Dictionary<CargoEntity.Type, CargoItemData>();
            foreach (var item in cargoList)
            {
                cargoDictionary.Add(item.CargoType, item);
            }
        }
    }
}