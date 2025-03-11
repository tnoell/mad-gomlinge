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
    [SerializeField] private Vector2 enemyTargetPos;

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
        switch(spawnMode)
        {
        case SpawnMode.item:
            Vector3 position = new Vector3(
            Random.Range(itemSpawnArea.xMin, itemSpawnArea.xMax),
            Random.Range(itemSpawnArea.yMin, itemSpawnArea.yMax),
            0);
            return GameObject.Instantiate(prefab, position, Quaternion.identity, scrollingTransform);
        case SpawnMode.enemy:
            return GameObject.Instantiate(prefab, (Vector3)enemySpawnPoint, Quaternion.identity, transform);
        default:
            throw new Exception("SpawnMode not implemented: " + spawnMode);
        }
        
    }

    public Vector2 GetEnemyTargetPos()
    {
        return enemyTargetPos;
    }
    
    void OnDrawGizmosSelected()
    {
        Util.DrawRect(itemSpawnArea, Color.blue);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere((Vector3)enemySpawnPoint, 0.5f);
        Gizmos.color = new Color(224 / 255.0f, 109 / 255.0f, 15 / 255.0f);
        Gizmos.DrawSphere((Vector3)enemyTargetPos, 0.5f);
    }
}
