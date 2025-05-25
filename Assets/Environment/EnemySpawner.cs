using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float initialDelay;
    [SerializeField] private Combat.Combatant[] enemyPrefabSequence;
    [SerializeField] public UnityEvent onFinish;
    [SerializeField] private bool spawnAutomatically = true;

    private Combat.Combatant currentEnemyInstance;
    private int iNextEnemy;
    private float initialDelayLeft;

    public Combat.Combatant[] EnemyPrefabSequence
    {
        get => enemyPrefabSequence;
        set
        {
            enemyPrefabSequence = value;
            iNextEnemy = 0;
        }
    }

    void Awake()
    {
        currentEnemyInstance = null;
        if (spawnAutomatically) BeginSpawning();
    }

    public void BeginSpawning()
    {
        iNextEnemy = 0;
        initialDelayLeft = initialDelay;
        spawnAutomatically = true;
    }

    private bool SpawnNextEnemy()
    {
        if (iNextEnemy < enemyPrefabSequence.Length)
        {
            SpawnEnemy(enemyPrefabSequence[iNextEnemy].gameObject);
            iNextEnemy++;
            return true;
        }
        else
        {
            onFinish.Invoke();
            spawnAutomatically = false;
            return false;
        }
    }

    public GameObject SpawnEnemy(GameObject enemy)
    {
        GameObject instance = Environment.GetInstance()
                .Spawn(enemy, SpawnMode.enemy);
        currentEnemyInstance = instance.GetComponent<Combat.Combatant>();
        return instance;
    }

    public void SpawnEnemyVoid(GameObject enemy)
    {
        SpawnEnemy(enemy);
    }

    void Update()
    {
        if(!spawnAutomatically) return;
        if(initialDelayLeft > 0)
        {
            initialDelayLeft -= Time.deltaTime;
            return;
        }
        if(!currentEnemyInstance)
        {
            if(!SpawnNextEnemy()) return;
        }
    }
}
