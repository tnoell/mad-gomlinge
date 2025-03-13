using UnityEngine;

public class EngineMinigame : Minigame
{
    [SerializeField] private RectTransform dropletSpawnPosition;
    [SerializeField] private RectTransform dropletParent;
    [SerializeField] private Collider2D dropletPrefab;
    [SerializeField] private int dropletsNeeded = 50;
    [SerializeField] private float dropletInterval = 0.1f;
    [SerializeField] private AnimationCurve tankSpeed;
    [SerializeField] private float timeUntilFinalTankSpeed = 6;

    private float timePassed;
    private Animator tankAnimator;
    private int dropletsCollected;
    private float nextDropletTime;

    void Awake()
    {
        tankAnimator = GetComponent<Animator>();
        timePassed = 0;
        dropletsCollected = 0;
        nextDropletTime = Time.fixedTime + dropletInterval;
    }

    void FixedUpdate()
    {
        if(dropletsCollected >= dropletsNeeded)
        {
            Complete();
        }

        timePassed += Time.fixedDeltaTime;
        float timePassedRelative = Mathf.Min(1, timePassed / timeUntilFinalTankSpeed);
        tankAnimator.speed = tankSpeed.Evaluate(timePassedRelative);

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
