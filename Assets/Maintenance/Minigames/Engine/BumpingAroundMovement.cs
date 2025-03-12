using UnityEngine;

public class BumpingAroundMovement : MonoBehaviour
{
    [SerializeField] private Rect range;
    [SerializeField] private float maxImpulse;
    [SerializeField] private float maxInterval;

    private float nextInterval;
    private Vector2 velocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nextInterval = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
