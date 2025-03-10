using UnityEngine;


namespace Combat
{
public class AttackLauncher : MonoBehaviour
{
    [SerializeField] private AttackMovement attackPrefab;
    [SerializeField] private float interval;
    [SerializeField] private Transform attackOrigin;
    private float timePassed;
    private Combatant combatant;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timePassed = 0;
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
            AttackMovement attackInstance = GameObject.Instantiate(attackPrefab);
            attackInstance.transform.position = attackOrigin.position;
            attackInstance.Launch(combatant, target);
        }
    }
}
}