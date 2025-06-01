using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EncounterManager : MonoBehaviour
{
    // [SerializeField] private int nEncounters = 10;
    [SerializeField] private List<SequenceElement> testEncounterPrefabs; // not gonna be a straight sequence later
    [SerializeField] private List<Button> signpostSigns;

    public UnityEvent onFinish;
    private SequenceElement currentEncounter;
    // private int iCurrentTestEncounter;

    // private int encountersLeft;

    void Start()
    {
        // encountersLeft = nEncounters;
        // iCurrentTestEncounter = -1;
        // StartNextEncounter();

        for (int iSign = 0; iSign < signpostSigns.Count; iSign++)
        {
            Button button = signpostSigns[iSign];
            if (iSign >= testEncounterPrefabs.Count)
            {
                button.gameObject.SetActive(false);
                continue;
            }
            MultiImage images = button.GetComponent<MultiImage>();
            IconSequence icons = testEncounterPrefabs[iSign].GetComponent<IconSequence>();
            images.Display(icons.Get());

            // TODO: button functionality
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    void ShowNextEncounterSelection()
    {

    }

    // void StartNextEncounter()
    // {
    //     // encountersLeft--;
    //     if (currentEncounter) throw new Exception("Already in encounter");
    //     iCurrentTestEncounter++;
    //     if (iCurrentTestEncounter >= testEncounterPrefabs.Count)
    //     {
    //         Finish();
    //         return;
    //     }
    //     currentEncounter = GameObject.Instantiate(testEncounterPrefabs[iCurrentTestEncounter], transform);
    //     currentEncounter.onFinish.AddListener(EndEncounter);
    //     currentEncounter.Begin();
    // }

    void EndEncounter()
    {
        if (currentEncounter)
        {
            GameObject.Destroy(currentEncounter.gameObject);
            currentEncounter = null;
        }
        // StartNextEncounter();
    }

    private void Finish()
    {
        onFinish.Invoke();
    }
}
