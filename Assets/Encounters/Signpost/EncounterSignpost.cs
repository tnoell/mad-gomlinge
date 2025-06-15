using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EncounterSignpost : MonoBehaviour
{
    [SerializeField] private List<Button> signs;
    [SerializeField] private EncounterManager encounterManager;

    private List<GameObject> pf_encounterSeqBySign;

    void Awake()
    {
        ClearEncounters();

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

    public void ShowEncounters(List<GameObject> pf_encounterSeqs)
    {
        if (pf_encounterSeqs.Count > signs.Count)
        {
            Debug.LogWarning("More encounters than signs");
        }

        for (int iSign = 0; iSign < signs.Count; iSign++)
        {
            GameObject pf_encounterSeq = null;
            if (iSign < pf_encounterSeqs.Count)
            {
                pf_encounterSeq = pf_encounterSeqs[iSign];
            }
            SetSignEncounter(iSign, pf_encounterSeq);
        }

        Show(true);
    }

    public void Show(bool show)
    {
        gameObject.SetActive(show);
    }

    public void SetSignEncounter(int iSign, GameObject pf_encounterSeq)
    {
        Button button = signs[iSign];
        pf_encounterSeqBySign[iSign] = pf_encounterSeq;

        if (pf_encounterSeq == null)
        {
            button.gameObject.SetActive(false);
            return;
        }
        button.gameObject.SetActive(true);
        MultiImage images = button.GetComponent<MultiImage>();
        Encounter[] pf_encounters = pf_encounterSeq.GetComponentsInChildren<Encounter>();
        List<Sprite> icons = new List<Sprite>();
        foreach (Encounter pf_encounter in pf_encounters)
        {
            Sprite icon = pf_encounter.GetIcon();
            if (icon != null) icons.Add(icon);
        }
        images.Display(icons);
    }

    private void OnSignClicked(int iSign)
    {
        GameObject pf_encounterSeq = pf_encounterSeqBySign[iSign];
        if (pf_encounterSeq == null)
        {
            Debug.LogWarning("Signpost: Sign without corresponding encounter clicked");
            return;
        }
        encounterManager.StartEncounterSequence(pf_encounterSeq);
        ClearEncounters();
        Show(false);
    }

    private void ClearEncounters()
    {
        pf_encounterSeqBySign = Enumerable.Repeat<GameObject>(null, signs.Count()).ToList();
    }
}
