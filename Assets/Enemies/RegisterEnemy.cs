using Combat;
using UnityEngine;

public class RegisterEnemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Combatant player = GameObject.FindWithTag("Player").GetComponent<Combatant>();
        player.SetTarget(GetComponent<Combatant>());
    }
}
