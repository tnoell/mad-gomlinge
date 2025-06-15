using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EncounterSignpost : MonoBehaviour
{
    [SerializeField] private List<Button> signs;
    [SerializeField] private EncounterManager encounterManager;

    public class SignContent
    {
        public GameObject pf_encounterSeq { get; private set; } = null;
        public UnityAction action { get; private set; } = null;
        public List<Sprite> icons { get; private set; } = null;

        public SignContent(GameObject pf_encounterSeq)
        {
            this.pf_encounterSeq = pf_encounterSeq;
            List<Encounter> pf_encounters = pf_encounterSeq.GetComponentsInChildren<Encounter>().ToList();
            LoadIcons(pf_encounters);
        }

        public SignContent(UnityAction action, List<Encounter> iconSources)
        {
            this.action = action;
            LoadIcons(iconSources);
        }

        private void LoadIcons(List<Encounter> iconSources)
        {
            icons = new List<Sprite>();
            foreach (Encounter pf_encounter in iconSources)
            {
                Sprite icon = pf_encounter.GetIcon();
                if (icon != null) icons.Add(icon);
            }
        }
    }

    private List<SignContent> signContents;

    void Awake()
    {
        ClearSigns();

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

    public void ShowSigns(List<SignContent> signContents)
    {
        if (signContents.Count > signs.Count)
        {
            Debug.LogWarning("Can't show " + signContents.Count + " signs");
        }

        for (int iSign = 0; iSign < signs.Count; iSign++)
        {
            SignContent signContent = null;
            if (iSign < signContents.Count)
            {
                signContent = signContents[iSign];
            }
            SetSignContent(iSign, signContent);
        }

        Show(true);
    }

    public void Show(bool show)
    {
        gameObject.SetActive(show);
    }

    public void SetSignContent(int iSign, SignContent signContent)
    {
        Button button = signs[iSign];
        signContents[iSign] = signContent;

        if (signContent == null)
        {
            button.gameObject.SetActive(false);
            return;
        }
        button.gameObject.SetActive(true);
        MultiImage images = button.GetComponent<MultiImage>();
        images.Display(signContent.icons);
    }

    private void OnSignClicked(int iSign)
    {
        SignContent content = signContents[iSign];
        if (content == null)
        {
            Debug.LogWarning("Signpost: Sign without corresponding content clicked");
            return;
        }
        if (content.pf_encounterSeq != null)
        {
            encounterManager.StartEncounterSequence(content.pf_encounterSeq);
        }
        content.action?.Invoke();
        ClearSigns();
        Show(false);
    }

    private void ClearSigns()
    {
        signContents = Enumerable.Repeat<SignContent>(null, signs.Count()).ToList();
    }
}
