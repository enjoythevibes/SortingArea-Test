using UnityEngine;

namespace TestGame.Player
{
    public class PlayerScores : MonoBehaviour
    {
        private static int scores;
        public static int Scores 
        {
            set
            {
                if (value < 0) value = 0;
                scores = value;
            }
            get
            {
                return scores;
            }
        }

        private void OnDestroy() 
        {
            scores = 0;    
        }
    }
}