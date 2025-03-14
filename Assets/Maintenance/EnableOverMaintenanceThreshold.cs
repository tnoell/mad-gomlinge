using UnityEngine;

public class EnableOverMaintenanceThreshold : MonoBehaviour
{
    public delegate void OnChange(bool enabled);  // return true if cleanup was handled; if none returns true, object gets destroyed
    public OnChange onChange;
    [SerializeField] private float threshold = 0.5f;
    [SerializeField] private GameObject target;
    private MaintenanceTimer maintenanceTimer;
    private bool lastEnabled;

    void Awake()
    {
        onChange = null;
        Enable(false);
        maintenanceTimer = GetComponent<MaintenanceTimer>();
    }

    private void Enable(bool enable)
    {
        lastEnabled = enable;
        if(target) target.SetActive(enable); // target?.SetActive throws NullReferenceException
        onChange?.Invoke(enable);
    }

    void Update()
    {
        bool newEnabled = maintenanceTimer.GetProgress() >= threshold;
        if(newEnabled != lastEnabled) Enable(newEnabled);
    }
}
