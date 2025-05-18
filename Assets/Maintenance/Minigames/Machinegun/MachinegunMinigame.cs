using System.Collections.Generic;
using UnityEngine;

public class MachinegunMinigame : Minigame
{
    [SerializeField] private List<GameObject> bullets;
    [SerializeField] private List<GameObject> bulletSlots;
    [SerializeField] private int initiallyCompleted;

    void Start()
    {
        for(int i = 0; i < initiallyCompleted; i++)
        {
            int iBullet = Random.Range(0, bullets.Count);
            GameObject.Destroy(bullets[iBullet]);
            bullets.RemoveAt(iBullet);

            int iSlot = Random.Range(0, bulletSlots.Count);
            bulletSlots[iSlot].GetComponent<DragReceiver>().onReceive.Invoke();
            bulletSlots.RemoveAt(iSlot);
        }
    }

    void FixedUpdate()
    {
        foreach(GameObject bullet in bullets)
        {
            if (bullet) return;
        }
        Complete();
    }
}
