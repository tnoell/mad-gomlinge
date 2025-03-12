using UnityEngine;
using UnityEditor;

public class VisibleOnStart : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        #if UNITY_EDITOR
        SceneVisibilityManager.instance.Hide(gameObject, false);
        #endif
    }
}
