using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ui
{
    public class UiTrackObject : MonoBehaviour
    {
        [SerializeField] private GameObject trackedObject;
        [SerializeField] private bool destroyWithTrackedObject = true;
        private Vector3 offset;

        public void Track(GameObject gameObject, Vector3 offset)
        {
            this.trackedObject = gameObject;
            this.offset = offset;
        }

        public void Track(GameObject gameObject)
        {
            Track(gameObject, Vector3.zero);
        }

        void Update()
        {
            if (!trackedObject)
            {
                if (destroyWithTrackedObject) GameObject.Destroy(gameObject);
                else this.enabled = false;
                return;
            }
            transform.position = trackedObject.transform.position + offset;
        }
    }
}
