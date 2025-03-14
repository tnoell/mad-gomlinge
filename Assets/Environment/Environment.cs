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
    public delegate void OnMove(float distance);
    public OnMove onMove;
    [SerializeField] private Transform scrollingTransform;
    [SerializeField] private float scrollSpeedFactor;
    [FormerlySerializedAs("spawnArea")]
    [SerializeField] private Rect itemSpawnArea;
    [SerializeField] private Vector2 enemySpawnPoint;
    [SerializeField] private Vector2 enemyTargetPos;
    [SerializeField] private SpriteRenderer background;

    private PlayerStats playerStats;
    private float scrollingStepSize;

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
        scrollingStepSize = background.bounds.size.y;
        // duplicate background downwards
        Transform doubledBackground = GameObject.Instantiate(background,
                background.transform.position + Vector3.down * scrollingStepSize,
                Quaternion.identity, background.transform).transform;
        doubledBackground.localScale = Vector3.one;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        float scrollDistance = Time.deltaTime * playerStats.GetSpeed() * scrollSpeedFactor;
        Vector3 pos = scrollingTransform.position;
        pos.y += scrollDistance;
        if(pos.y > scrollingStepSize)
        {
            StepScrolling(ref pos);
        }
        scrollingTransform.position = pos;
        onMove?.Invoke(scrollDistance);
    }

    private void StepScrolling(ref Vector3 pos)
    {
        Vector3 step = new Vector3(0, -scrollingStepSize, 0);
        pos += step;
        step *= -1;
        foreach(Transform child in scrollingTransform)
        {
            if(child == background.transform) continue;
            child.Translate(step);
        }
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

    public void MakeScrolling(Transform transform)
    {
        transform.parent = scrollingTransform;
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
