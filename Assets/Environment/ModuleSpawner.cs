using UnityEngine;

public class ModuleSpawner : MonoBehaviour
{
    [SerializeField] private Module[] modulePrefabs;
    [SerializeField] private float spawnDelay;
    private float nextSpawnTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nextSpawnTime = Time.fixedTime + spawnDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.fixedTime > nextSpawnTime)
        {
            Module prefab = modulePrefabs[Random.Range(0, modulePrefabs.Length)];
            GameObject instance = Environment.GetInstance().Spawn(prefab.gameObject);
            instance.GetComponent<Module>().SetOnGround(true);
            nextSpawnTime = Time.fixedTime + spawnDelay;
        }
    }
}
