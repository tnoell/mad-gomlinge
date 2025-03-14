using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float initialDelay;
    [SerializeField] private Combat.Combatant[] enemyPrefabSequence;
    [SerializeField] private UnityEvent onFinish;
    [SerializeField] private bool spawnAutomatically = true;

    private Combat.Combatant currentEnemyInstance;
    private int iNextEnemy;
    private float initialDelayLeft;

    void Awake()
    {
        currentEnemyInstance = null;
        iNextEnemy = 0;
        initialDelayLeft = initialDelay;
    }

    private bool SpawnNextEnemy()
    {
        if(iNextEnemy < enemyPrefabSequence.Length)
        {
            SpawnEnemy(enemyPrefabSequence[iNextEnemy].gameObject);
            iNextEnemy++;
            return true;
        }
        else if(iNextEnemy == enemyPrefabSequence.Length)
        {
            onFinish.Invoke();
            iNextEnemy++;
            return false;
        }
        else
        {
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

    // Update is called once per frame
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
