using System.Collections.Generic;
using UnityEngine;

public class Tags : MonoBehaviour
{
    [SerializeField] private List<string> tagList;
    private HashSet<string> tags;

    void Awake()
    {
        tags = new HashSet<string>(tagList);
    }

    public bool Contains(string tag)
    {
        return tags.Contains(tag);
    }

    public static bool HasTag(GameObject obj, string tag)
    {
        return obj.GetComponent<Tags>()?.Contains(tag) ?? false;
    }

    public static bool HasTag(MonoBehaviour obj, string tag)
    {
        return HasTag(obj.gameObject, tag);
    }
}
