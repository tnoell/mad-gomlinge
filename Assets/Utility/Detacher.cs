using UnityEngine;

public class Detacher : MonoBehaviour
{
    [SerializeField] private Transform obj;
    [SerializeField] private Transform makeSiblingOf;

    public void Detach()
    {
        obj.parent = makeSiblingOf?.parent;
    }
}
