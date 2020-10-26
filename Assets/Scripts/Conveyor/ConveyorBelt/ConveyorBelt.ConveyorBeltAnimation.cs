using UnityEngine;

namespace TestGame.Conveyor.Belt
{
    public partial class ConveyorBelt
    {
        [System.Serializable]
        private class ConveyorBeltAnimation
        {
            [SerializeField] private Renderer beltRenderer = default;
            [SerializeField] private float beltAnimationSpeed = 11.5f;
            private MaterialPropertyBlock materialPropertyBlock;
            private bool isPlaying;

            public void Initialize(MaterialPropertyBlock materialPropertyBlock)
            {
                this.materialPropertyBlock = materialPropertyBlock;
            }

            public void Play()
            {
                if (isPlaying) return;
                beltRenderer.GetPropertyBlock(materialPropertyBlock);
                materialPropertyBlock.SetFloat("_ScrollYSpeed", beltAnimationSpeed);
                beltRenderer.SetPropertyBlock(materialPropertyBlock);
                isPlaying = true;
            }

            public void Stop()
            {
                if (!isPlaying) return;
                beltRenderer.GetPropertyBlock(materialPropertyBlock);
                materialPropertyBlock.SetFloat("_ScrollYSpeed", 0f);
                beltRenderer.SetPropertyBlock(materialPropertyBlock);
                isPlaying = false;
            }
        }
    }
}