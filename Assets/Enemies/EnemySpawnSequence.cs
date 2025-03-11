using Combat;
using UnityEngine;

public class EnemySpawnSequence : MonoBehaviour
{
    [SerializeField] private AttackLauncher attack;
    [SerializeField] private float movementDuration;
    [SerializeField] private AnimationCurve positionByProgress;
    [SerializeField] private float registerAtPositionT;
    [SerializeField] private float playerBaseSpeed = 0.2f;
    private PlayerStats playerStats;
    private float movementProgress;
    private Vector3 startPos;
    private Vector3 targetPos;
    private bool registered;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        attack.enabled = false;
        registered = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        startPos = transform.position;
        targetPos = (Vector3)Environment.GetInstance().GetEnemyTargetPos();
        movementProgress = 0;
    }

    void Register()
    {
        if(registered) return;
        registered = true;
        Combatant player = GameObject.FindWithTag("Player").GetComponent<Combatant>();
        Combatant self = GetComponent<Combatant>();
        player.SetTarget(self);
        self.SetTarget(player);
    }

    // Update is called once per frame
    void Update()
    {
        movementProgress += Time.deltaTime
            / movementDuration / (playerStats.GetSpeed() + playerBaseSpeed);
        if(movementProgress < 1)
        {
            float posT = positionByProgress.Evaluate(movementProgress);
            transform.position = Vector3.Lerp(startPos, targetPos, posT);
            if(posT >= registerAtPositionT) Register();
        }
        else
        {
            Register(); // just to make sure
            transform.position = targetPos;
            // TODO: run animation here, which triggers attack.enabled instead
            attack.enabled = true;
            this.enabled = false;
        }
    }
}
