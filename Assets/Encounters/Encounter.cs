using UnityEngine;

public abstract class Encounter : SequenceElement
{
    [SerializeField] private Sprite icon;

    public Sprite GetIcon() { return icon; }
}
