using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public enum SpawnMode
{
    item,
    enemy
}

public class Environment : MonoBehaviour
{
    [SerializeField] private Transform scrollingTransform;
    [SerializeField] private float scrollSpeed;
    [FormerlySerializedAs("spawnArea")]
    [SerializeField] private Rect itemSpawnArea;
    [SerializeField] private Vector2 enemySpawnPoint;

    private static Environment instance = null;

    public static Environment GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("No Environment instance exists");
        }
        return instance;
    }

    void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = scrollingTransform.position;
        pos.y += Time.deltaTime * scrollSpeed;
        scrollingTransform.position = pos;
    }

    public GameObject Spawn(GameObject prefab, SpawnMode spawnMode)
    {
        Vector3 position;
        switch(spawnMode)
        {
        case SpawnMode.item:
            position = new Vector3(
            Random.Range(itemSpawnArea.xMin, itemSpawnArea.xMax),
            Random.Range(itemSpawnArea.yMin, itemSpawnArea.yMax),
            0);
            break;
        case SpawnMode.enemy:
            position = (Vector3)enemySpawnPoint;
            break;
        default:
            throw new Exception("SpawnMode not implemented: " + spawnMode);
        }
        return GameObject.Instantiate(prefab, position, Quaternion.identity, scrollingTransform);
    }
    
    void OnDrawGizmosSelected()
    {
        Util.DrawRect(itemSpawnArea, Color.blue);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere((Vector3)enemySpawnPoint, 0.5f);
    }
}
