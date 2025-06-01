using System.Collections.Generic;
using UnityEngine;

public class IconSequence : MonoBehaviour
{
    [SerializeField] private List<Sprite> icons;

    public List<Sprite> Get()
    {
        if (icons.Count == 0)
        {
            foreach (Transform child in transform)
            {
                IconSequence childIcons = child.GetComponent<IconSequence>();
                if (childIcons) icons.AddRange(childIcons.Get());
            }
        }
        return icons;
    }
}
