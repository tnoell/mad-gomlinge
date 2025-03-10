using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Ui
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] private Canvas worldSpaceCanvas;
        [SerializeField] private Transform attachmentPointHolder;
        [SerializeField] private AttachmentPoint attachmentPointPrefab;
        private static UiManager instance = null;

        public static UiManager GetInstance()
        {
            if (instance == null)
            {
                Debug.LogError("No UiManager instance exists");
            }
            return instance;
        }

        void Awake()
        {
            instance = this;
            worldSpaceCanvas.worldCamera = Camera.main;
#if UNITY_EDITOR
            SceneVisibilityManager.instance.Hide(gameObject, true);
#endif
        }

        private void Start()
        {
        }

        private void Update()
        {
            // if (gameInput.GetKeyDown(KeyCode.Escape))
            // {
            //     Time.timeScale = 0;
            //     pauseMenu.SetActive(true);
            //     pausedInput.Activate();
            // }
            // else if (pausedInput.GetKeyDown(KeyCode.Escape))
            // {
            //     Time.timeScale = 1;
            //     pauseMenu.SetActive(false);
            //     pausedInput.Deactivate();
            // }
        }

        public AttachmentPoint AddAttachmentPoint(GameObject trackedObj, Vector3 offset)
        {
            AttachmentPoint ap = GameObject.Instantiate(attachmentPointPrefab, attachmentPointHolder);
            ap.GetComponent<UiTrackObject>().Track(trackedObj, offset);
            return ap;
        }

        public GameObject AddTracking(UiTrackObject trackingPrefab, GameObject trackedObj, Vector3 offset)
        {
            UiTrackObject instance = GameObject.Instantiate(trackingPrefab, worldSpaceCanvas.transform);
            instance.Track(trackedObj, offset);
            return instance.gameObject;
        }

        public GameObject AddTracking(UiTrackObject trackingPrefab, GameObject trackedObj)
        {
            return AddTracking(trackingPrefab, trackedObj, Vector3.zero);
        }
    }

}
