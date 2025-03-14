using UnityEngine;

public class MachinegunMinigame : Minigame
{
    [SerializeField] private GameObject[] bullets;

    void FixedUpdate()
    {
        foreach(GameObject bullet in bullets)
        {
            if (bullet) return;
        }
        Complete();
    }
}
