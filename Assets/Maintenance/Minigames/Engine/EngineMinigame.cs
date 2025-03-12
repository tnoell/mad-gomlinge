using UnityEngine;

public class EngineMinigame : Minigame
{
    [SerializeField] private RectTransform dropletSpawnPosition;
    [SerializeField] private RectTransform dropletParent;
    [SerializeField] private Collider2D dropletPrefab;
    [SerializeField] private int dropletsNeeded = 50;
    [SerializeField] private float dropletInterval = 0.1f;

    private int dropletsCollected;
    private float nextDropletTime;

    void Awake()
    {
        dropletsCollected = 0;
        nextDropletTime = Time.fixedTime + dropletInterval;
    }

    void FixedUpdate()
    {
        if(dropletsCollected >= dropletsNeeded)
        {
            Complete();
        }

        while(Time.fixedTime >= nextDropletTime)
        {
            nextDropletTime += dropletInterval;
            Collider2D droplet = GameObject.Instantiate(dropletPrefab,
                    dropletSpawnPosition.position, Quaternion.identity, dropletParent);
            droplet.GetComponent<Collider2DEvents>().onTriggerEnter2D.AddListener(() => {
                dropletsCollected++;
                GameObject.Destroy(droplet.gameObject);
            });
        }
    }

    public float GetProgress()
    {
        return dropletsCollected / (float)dropletsNeeded;
    }
}
