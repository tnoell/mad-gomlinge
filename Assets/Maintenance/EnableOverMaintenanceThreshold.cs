using UnityEngine;

public class EnableOverMaintenanceThreshold : MonoBehaviour
{
    [SerializeField] private float threshold = 0.5f;
    [SerializeField] private GameObject target;
    private MaintenanceTimer maintenanceTimer;

    void Awake()
    {
        maintenanceTimer = GetComponent<MaintenanceTimer>();
    }

    void Update()
    {
        target.SetActive(maintenanceTimer.GetProgress() >= threshold);
    }
}
