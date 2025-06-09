using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EncounterManager : MonoBehaviour
{
    // [SerializeField] private int nEncounters = 10;
    [SerializeField] private List<SequenceElement> testEncounterPrefabs; // not gonna be a straight sequence later
    [SerializeField] private EncounterSignpost signpost;

    public UnityEvent onFinish;
    private SequenceElement currentEncounter;
    private ModuleGrid moduleGrid;

    // private int encountersLeft;

    void Start()
    {
        moduleGrid = GameObject.FindWithTag("ModuleGrid").GetComponent<ModuleGrid>();
        moduleGrid.MaintenanceTimeScale = 0;
        for (int iSign = 0; iSign < signpost.GetSignCount(); iSign++)
        {
            SequenceElement encounter = null;
            if (testEncounterPrefabs.Count > iSign)
            {
                encounter = testEncounterPrefabs[iSign];
            }
            signpost.SetSignEncounter(iSign, encounter);
        }
    }

    public void StartEncounter(SequenceElement encounterPrefab)
    {
        if (currentEncounter) throw new Exception("Already in encounter");
        currentEncounter = GameObject.Instantiate(encounterPrefab, transform);
        currentEncounter.onFinish.AddListener(EndEncounter);
        currentEncounter.Begin();
        moduleGrid.MaintenanceTimeScale = 1;
    }

    void ShowNextEncounterSelection()
    {

    }

    void EndEncounter()
    {
        if (currentEncounter)
        {
            GameObject.Destroy(currentEncounter.gameObject);
            currentEncounter = null;
        }
        moduleGrid.MaintenanceTimeScale = 0;
    }

    private void Finish()
    {
        onFinish.Invoke();
    }
}
