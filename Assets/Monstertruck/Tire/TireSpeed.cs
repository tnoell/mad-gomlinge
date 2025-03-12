using UnityEngine;

public class TireSpeed : MonoBehaviour
{
    [SerializeField] private float factor = 1;
    private Animator animator;
    private PlayerStats playerStats;

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();    
    }

    void Update()
    {
        animator.speed = playerStats.GetSpeed() * factor;
    }
}
