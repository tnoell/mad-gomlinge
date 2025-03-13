using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float initialDelay;
    [SerializeField] private Combat.Combatant[] enemyPrefabSequence;
    [SerializeField] private UnityEvent onFinish;

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
            GameObject instance = Environment.GetInstance()
                    .Spawn(enemyPrefabSequence[iNextEnemy].gameObject, SpawnMode.enemy);
            currentEnemyInstance = instance.GetComponent<Combat.Combatant>();
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

    // Update is called once per frame
    void Update()
    {
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
