using System.Collections.Generic;
using UnityEngine;

public class IconSequence : MonoBehaviour
{
    [SerializeField] private List<Sprite> icons;

    public List<Sprite> Get()
    {
        if (icons.Count > 0) return icons;

        List<Sprite> iconsFromChildren = new List<Sprite>(); // We don't cache this, because
            // this is likely to be called on a prefab, so the data might stick around after
            // the prefab gets edited. It wouldn't be a huge problem, but the performance cost
            // of not caching it also shouldn't be significant.
        foreach (Transform child in transform)
        {
            IconSequence childIcons = child.GetComponent<IconSequence>();
            if (childIcons) iconsFromChildren.AddRange(childIcons.Get());
        }
        return iconsFromChildren;
    }
}
