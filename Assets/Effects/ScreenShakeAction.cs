using UnityEngine;
using System;
using Random = UnityEngine.Random;

namespace VisualEffects
{
    // This can handle running multiple times in parallel, but since the cycle duration is equal,
    // they will be perfectly in sync. Probably fine for such an edge case to look slightly strange though. 
    [Serializable]
    public class ScreenShakeAction : GameAction
    {
        [SerializeField] private Vector2 directionMin;
        [SerializeField] private Vector2 directionMax;
        [Tooltip("Inverse of frequency")]
        [SerializeField] private float singleCycleDuration;
        [SerializeField] private float duration;

        private float timePassed = -1;
        private Vector2 pickedDirection;

        public ScreenShakeAction Copy()
        {
            ScreenShakeAction copy = new ScreenShakeAction();
            copy.directionMin = this.directionMin;
            copy.directionMax = this.directionMax;
            copy.singleCycleDuration = this.singleCycleDuration;
            copy.duration = this.duration;
            return copy;
        }

        public void Apply(float multiplier)
        {
            ScreenShakeAction instance = Copy();
            
            instance.timePassed = 0;
            float x = Random.Range(directionMin.x, directionMax.x) * multiplier;
            float y = Random.Range(directionMin.y, directionMax.y) * multiplier;
            instance.pickedDirection = new Vector2(x, y);
            VisualEffectsManager.GetInstance().AddScreenShake(instance);
        }

        public override void Execute()
        {
            Apply(1);
        }
        
        public Vector2 GetOffset(float deltaTime)
        {
            timePassed += deltaTime;
            if (timePassed > duration)
            {
                Finish();
                return Vector2.zero;
            }
            float phase = Mathf.Sin(timePassed / singleCycleDuration * 2 * Mathf.PI);
            return pickedDirection * (
                (1 - (timePassed / duration)) *
                phase);
        }

        private void Finish()
        {
            VisualEffectsManager.GetInstance().RemoveScreenShake(this);
            timePassed = -1;
        }
    }
}
