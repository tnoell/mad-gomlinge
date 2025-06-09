using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EncounterSignpost : MonoBehaviour
{
    [SerializeField] private List<Button> signs;
    [SerializeField] private EncounterManager encounterManager;

    private List<SequenceElement> encounterBySign;

    void Awake()
    {
        encounterBySign = Enumerable.Repeat<SequenceElement>(null, signs.Count()).ToList();
        
        for (int iSign = 0; iSign < signs.Count; iSign++)
        {
            Button button = signs[iSign];
            int iSignLocalCopy = iSign; // because the lambda captures by reference
            button.onClick.AddListener(() => OnSignClicked(iSignLocalCopy));
        }
    }

    public int GetSignCount()
    {
        return signs.Count;
    }

    public void SetSignEncounter(int iSign, SequenceElement encounter)
    {
        Button button = signs[iSign];
        encounterBySign[iSign] = encounter;

        if (encounter == null)
        {
            button.gameObject.SetActive(false);
            return;
        }
        button.gameObject.SetActive(true);
        MultiImage images = button.GetComponent<MultiImage>();
        IconSequence icons = encounter.GetComponent<IconSequence>();
        images.Display(icons.Get());
    }

    private void OnSignClicked(int iSign)
    {
        SequenceElement encounter = encounterBySign[iSign];
        if (encounter == null)
        {
            Debug.LogWarning("Signpost: Sign without corresponding encounter clicked");
            return;
        }
        encounterManager.StartEncounter(encounter);
        encounterBySign.Clear();
        gameObject.SetActive(false); // hide
    }
}
