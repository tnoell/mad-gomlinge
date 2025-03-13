using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;


namespace Combat
{
public class AttackLauncher : MonoBehaviour
{
    [SerializeField] private AttackMovement attackPrefab;
    [SerializeField] private float interval;
    [SerializeField] private Transform attackOrigin;
    [SerializeField] private bool startCharged = false;
    [SerializeField] private Animator shotAnimator;
    [SerializeField] private UnityEvent onLaunched;
    private float timePassed;
    private Combatant combatant;
    Combatant target;
    [SerializeField] private int nAttacks;
    private int nAttacksLeft;

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
        target = combatant.GetTarget();
        if (!target) return;
        timePassed += Time.fixedDeltaTime;
        if(timePassed >= interval)
        {
            timePassed -= interval;

            if (shotAnimator)
            {
                nAttacksLeft = nAttacks;
                shotAnimator.SetTrigger("Fire");
                
            }
            else { FireGun(); }
        }
    }

    public void FireGun()
    {
        Vector3 position = transform.position;
        if (attackOrigin) position = attackOrigin.position;
        AttackMovement attackInstance = GameObject.Instantiate(attackPrefab, position, Quaternion.identity);
        attackInstance.Launch(combatant, target);
        onLaunched.Invoke();
        if (shotAnimator) 
        {
            nAttacksLeft--;
            if (nAttacksLeft > 0)
            { shotAnimator.SetTrigger("Fire"); }
        }
    }

    }
}