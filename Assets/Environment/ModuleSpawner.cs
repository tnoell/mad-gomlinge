using UnityEngine;

public class ModuleSpawner : MonoBehaviour
{
    [SerializeField] private Module[] modulePrefabs;
    [SerializeField] private float spawnDistanceInterval;
    private float nextSpawnDistance;
    private int orderCounter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nextSpawnDistance = 0;
        orderCounter = 0;
        Environment.GetInstance().onMove += Move;
    }

    private void Move(float distance)
    {
        nextSpawnDistance -= distance;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(nextSpawnDistance <= 0)
        {
            nextSpawnDistance += spawnDistanceInterval;
            Module prefab = modulePrefabs[Random.Range(0, modulePrefabs.Length)];
            GameObject instance = Environment.GetInstance().Spawn(prefab.gameObject, SpawnMode.item);
            instance.GetComponent<Module>().SetOnGround(true, orderCounter);
            orderCounter++; //make visual order match button order
        }
    }
}
