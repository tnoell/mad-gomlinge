using UnityEngine;
using UnityEngine.UI;

public class ImageAlphaHitTestThreshold : MonoBehaviour
{
    [SerializeField] private float value;
    void Start()
    {
        Image image = GetComponent<Image>();
        image.alphaHitTestMinimumThreshold = value;
    }
}
