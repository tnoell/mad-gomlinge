using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MultiImage : MonoBehaviour
{
    [SerializeField] private List<Image> images;

    public void Display(List<Sprite> sprites)
    {
        if (sprites.Count > images.Count)
        {
            Debug.LogWarning("Can't show more than " + images.Count + " icons", this);
        }
        for (int i = 0; i < images.Count; i++)
        {
            if (sprites.Count > i)
            {
                images[i].enabled = true;
                images[i].sprite = sprites[i];
            }
            else
            {
                images[i].enabled = false;
            }
        }
    }
}
