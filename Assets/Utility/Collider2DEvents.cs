using UnityEngine;
using UnityEngine.Events;

public class Collider2DEvents : MonoBehaviour
{
    public UnityEvent onTriggerEnter2D;
    void OnTriggerEnter2D(Collider2D collision)
    {
        onTriggerEnter2D.Invoke();
    }

    public UnityEvent onTriggerExit2D;
    void OnTriggerExit2D(Collider2D collision)
    {
        onTriggerExit2D.Invoke();
    }
}
