using UnityEngine;

public class ColorFromMaintenanceTime : MonoBehaviour
{
    [SerializeField] private MaintenanceTimer timer;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;

    void Update()
    {
        spriteRenderer.color = Color.Lerp(startColor, endColor, timer.GetProgress());
    }
}
