using UnityEngine;


namespace Combat
{
public class AttackLauncher : MonoBehaviour
{
    [SerializeField] private AttackMovement attackPrefab;
    [SerializeField] private float interval;
    [SerializeField] private Transform attackOrigin;
    [SerializeField] private bool startCharged = false;

    private float timePassed;
    private Combatant combatant;

    void Awake()
    {
        timePassed = startCharged ? interval : 0;
    }

    void Start()
    {
        UpdateCombatant();
    }

    public void UpdateCombatant()
    {
        combatant = GetComponentInParent<Combatant>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!combatant) return;
        Combatant target = combatant.GetTarget();
        if (!target) return;
        timePassed += Time.fixedDeltaTime;
        if(timePassed >= interval)
        {
            timePassed -= interval;
            Vector3 position = transform.position;
            if(attackOrigin) position = attackOrigin.position;
            AttackMovement attackInstance = GameObject.Instantiate(attackPrefab, position, Quaternion.identity);
            attackInstance.Launch(combatant, target);
        }
    }
}
}