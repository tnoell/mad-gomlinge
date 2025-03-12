using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] private Canvas worldSpaceCanvas;
        [SerializeField] private Transform trackingObjectsHolder;
        [SerializeField] private Transform attachmentPointHolder;
        [SerializeField] private Transform minigameHolder;
        [SerializeField] private AttachmentPoint attachmentPointPrefab;
        private static UiManager instance = null;
        private Minigame currentMinigame;

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

        public void EnableAttachmentPoints(bool enable)
        {
            attachmentPointHolder.gameObject.SetActive(enable);
        }

        public GameObject AddTracking(UiTrackObject trackingPrefab, GameObject trackedObj, Vector3 offset)
        {
            UiTrackObject instance = GameObject.Instantiate(trackingPrefab, trackingObjectsHolder);
            instance.Track(trackedObj, offset);
            return instance.gameObject;
        }

        public GameObject AddTracking(UiTrackObject trackingPrefab, GameObject trackedObj)
        {
            return AddTracking(trackingPrefab, trackedObj, Vector3.zero);
        }

        public Minigame StartMinigame(Minigame prefab)
        {
            if(currentMinigame) return null;
            currentMinigame = GameObject.Instantiate(prefab,
                    Vector3.zero, Quaternion.identity, minigameHolder);
            return currentMinigame;
        }
    }

}
