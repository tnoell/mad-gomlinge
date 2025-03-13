using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace VisualEffects
{
    public class VisualEffectsManager : MonoBehaviour
    {
        public Vector3 CurrentOffset { get; private set; }

        private List<ScreenShakeAction> screenShakes;
        private Vector3 basePos;
        private static VisualEffectsManager instance = null;

        public static VisualEffectsManager GetInstance()
        {
            if (instance == null)
            {
                Debug.LogError("No VisualEffectsManager instance exists");
            }
            return instance;
        }

        void Awake()
        {
#if UNITY_EDITOR
            Assert.AreEqual(Camera.main.gameObject, gameObject);
#endif
            instance = this;
            screenShakes = new List<ScreenShakeAction>();
            basePos = transform.position;
        }

        void Update()
        {
            UpdateScreenShake();
            transform.position = basePos + CurrentOffset;
        }

        private void UpdateScreenShake()
        {
            Vector2 offset = Vector2.zero;
            float deltaTime = Time.deltaTime;
            for (int i = screenShakes.Count - 1; i >= 0; i--)
            {
                offset += screenShakes[i].GetOffset(deltaTime); // screenshakes remove themselves when they're finished
            }
			// offset = offset.Rotated(transform.localEulerAngles.z); // rotate so the screen shake is in screen-space (sort of)
            CurrentOffset = offset;
        }

        public void AddScreenShake(ScreenShakeAction screenShake)
        {
            screenShakes.Add(screenShake);
        }

        public void RemoveScreenShake(ScreenShakeAction screenShake)
        {
            screenShakes.Remove(screenShake);
        }
    }
}
