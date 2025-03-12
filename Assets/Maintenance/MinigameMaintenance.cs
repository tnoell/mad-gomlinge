using Ui;
using UnityEngine;

public class MinigameMaintenance : MaintenanceTask
{
    [SerializeField] private Minigame minigamePrefab;

    protected override void OnDoMaintenance()
    {
        Minigame minigameInstance = UiManager.GetInstance().StartMinigame(minigamePrefab);
        minigameInstance?.onCompleted.AddListener(Complete);
    }
}
