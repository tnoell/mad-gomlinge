using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EncounterManager : MonoBehaviour
{
    // [SerializeField] private int nEncounters = 10;
    [SerializeField] private List<GameObject> pf_testEncounterSeqs; // not gonna be a straight sequence later
    [SerializeField] private EncounterSignpost signpost;

    public UnityEvent onFinish;
    private GameObject currentEncounterSeq;
    private List<Encounter> currentEncounters;
    private ModuleGrid moduleGrid;

    // private int encountersLeft;

    void Start()
    {
        moduleGrid = GameObject.FindWithTag("ModuleGrid").GetComponent<ModuleGrid>();
        moduleGrid.MaintenanceTimeScale = 0;
        ShowNextEncounterSelection();
    }

    public void StartEncounterSequence(GameObject pf_encounterSeq)
    {
        if (currentEncounterSeq != null) throw new Exception("Already in encounter");
        currentEncounterSeq = GameObject.Instantiate(pf_encounterSeq, transform);
        currentEncounters = currentEncounterSeq.GetComponentsInChildren<Encounter>().ToList();
        StartNextEncounter();
    }

    private void StartNextEncounter()
    {
        if (currentEncounters.Count() == 0)
        {
            EndEncounterSequence();
            return;
        }
        currentEncounters[0].onFinish.AddListener(EndEncounter);
        currentEncounters[0].Begin();
        moduleGrid.MaintenanceTimeScale = 1;
    }

    void EndEncounter()
    {
        currentEncounters.RemoveAt(0);
        StartNextEncounter();
    }

    void EndEncounterSequence()
    {
        if (currentEncounterSeq)
        {
            GameObject.Destroy(currentEncounterSeq);
            currentEncounterSeq = null;
            currentEncounters = null;
        }
        moduleGrid.MaintenanceTimeScale = 0;
        ShowNextEncounterSelection();
    }

    void ShowNextEncounterSelection()
    {
        signpost.ShowEncounters(pf_testEncounterSeqs); // TODO: randomize
    }

    private void Finish()
    {
        onFinish.Invoke();
    }
}
