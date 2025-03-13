using UnityEngine;

public class Detacher : MonoBehaviour
{
    [SerializeField] private Transform obj;
    [SerializeField] private Transform makeSiblingOf;
    [SerializeField] private bool activate = true;
    [SerializeField] private float destroyAfter = -1;

    public void Detach()
    {
        Transform parent = null;
        if(makeSiblingOf) parent = makeSiblingOf.parent;
        obj.parent = parent;
        if(activate)
        {
            obj.gameObject.SetActive(true);
        }
        if(destroyAfter >= 0)
        {
            GameObject.Destroy(obj.gameObject, destroyAfter);
        }
    }
}
