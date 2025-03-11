using UnityEngine;

public class DestroyWhenScrolledAway : MonoBehaviour
{
    private float yPos = 25;

    void Update()
    {
        if(transform.position.y > yPos)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
