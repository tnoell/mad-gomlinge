using Combat;
using UnityEngine;

public class AimingPhase : MonoBehaviour
{
    [SerializeField] private AttackLauncher attack;
    [SerializeField] private float duration;
    private PlayerStats playerStats;
    private float timePassed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        attack.enabled = false;
        timePassed = 0;
    }

    void Start()
    {
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timePassed += Time.fixedDeltaTime / playerStats.GetSpeed();
        if(timePassed > duration)
        {
            attack.enabled = true;
        }
    }
}
