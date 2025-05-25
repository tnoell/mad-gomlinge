using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EncounterManager : MonoBehaviour
{
    // [SerializeField] private int nEncounters = 10;
    [SerializeField] private List<SequenceElement> testEncounterPrefabs; // not gonna be a straight sequence later
    public UnityEvent onFinish;
    private SequenceElement currentEncounter;
    private int iCurrentTestEncounter;

    // private int encountersLeft;

    void Start()
    {
        // encountersLeft = nEncounters;
        iCurrentTestEncounter = -1;
        StartNextEncounter();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ShowNextEncounterSelection()
    {

    }

    void StartNextEncounter()
    {
        // encountersLeft--;
        if (currentEncounter) throw new Exception("Already in encounter");
        iCurrentTestEncounter++;
        if (iCurrentTestEncounter >= testEncounterPrefabs.Count)
        {
            Finish();
            return;
        }
        currentEncounter = GameObject.Instantiate(testEncounterPrefabs[iCurrentTestEncounter], transform);
        currentEncounter.onFinish.AddListener(EndEncounter);
        currentEncounter.Begin();
    }

    void EndEncounter()
    {
        if (currentEncounter)
        {
            GameObject.Destroy(currentEncounter.gameObject);
            currentEncounter = null;
        }
        StartNextEncounter();
    }

    private void Finish()
    {
        onFinish.Invoke();
    }
}
