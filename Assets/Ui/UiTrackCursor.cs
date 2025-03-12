using UnityEngine;

public class UiTrackCursor : MonoBehaviour
{
    [SerializeField] private bool trackX = true;
    [SerializeField] private bool trackY = true;
    [SerializeField] private bool worldSpace = true;
    [SerializeField] private Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        if(!worldSpace) Debug.LogError("UI space not implemented");
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        if(!trackX) pos.x = transform.position.x;
        if(!trackY) pos.y = transform.position.y;
        transform.position = pos;
    }
}
