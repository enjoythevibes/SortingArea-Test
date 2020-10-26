using UnityEngine;

namespace TestGame
{
    public static class Helper
    {
        public static void Shuffle<T>(this T[] array, System.Random random)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                var j = random.Next(i, array.Length);
                var temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }

        public static T GetComponentWithInterface<T>(this GameObject gameObject)
        {            
            if (!typeof(T).IsInterface) 
            {
                Debug.LogError($"{typeof(T).Name} is not an interface");
            }
            T component = default(T);
            var components = gameObject.GetComponents<Component>();
            foreach (var item in components)
            {
                if (item is T)
                {
                    component = item.GetComponent<T>();
                    break;
                }
            }
            if (component == null)
            {
                Debug.LogError($"{gameObject.name} doesnt contain {typeof(T).Name} interface");
            }
            return component;
        }
    }
}