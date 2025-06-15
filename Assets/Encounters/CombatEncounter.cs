using UnityEngine;
using UnityEngine.Events;

public class CombatEncounter : SequenceElement
{
    // later these prefabs will come from somewhere else, possibly another component on this gameobject
    [SerializeField] private Module[] modulePrefabs;
    [SerializeField] private Combat.Combatant[] enemyPrefabSequence;
    protected GameObject environment;
    private bool finished;

    protected override void Awake()
    {
        base.Awake();
        environment = GameObject.FindWithTag("EnvironmentRoot");
    }

    protected override void DoBegin()
    {
        base.DoBegin();
        finished = false;

        ModuleSpawner moduleSpawner = environment.GetComponent<ModuleSpawner>();
        moduleSpawner.ModulePrefabs = modulePrefabs;
        moduleSpawner.SpawnAutomatically = true;

        EnemySpawner enemySpawner = environment.GetComponent<EnemySpawner>();
        enemySpawner.EnemyPrefabSequence = enemyPrefabSequence;
        enemySpawner.onFinish.AddListener(Finish);
        enemySpawner.BeginSpawning();

        UnityAction cleanup = () =>
        {
            moduleSpawner.SpawnAutomatically = false;
            enemySpawner.onFinish.RemoveListener(Finish);
        };
        onFinish.AddListener(cleanup);
    }

    protected void Finish()
    {
        finished = true;
    }

    protected override bool SubIsFinished()
    {
        return finished;
    }
}
