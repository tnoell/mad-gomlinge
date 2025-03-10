using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject gameManagerPrefab;

    void Awake()
    {
        if (GameObject.FindWithTag("GameManager") == null)
        {
            GameObject.Instantiate(gameManagerPrefab);
        }
        GameObject.Destroy(gameObject);
    }
}
