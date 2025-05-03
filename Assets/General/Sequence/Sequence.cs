using System;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : SequenceElement
{
    [SerializeField] private List<SequenceElement> sequence;
    [SerializeField] private bool startAutomatically;

    private int iCurrentElement;

    protected override void SubAwake()
    {
        if(sequence.Count == 0)
        {
            foreach(Transform child in transform)
            {
                SequenceElement sequenceElement = child.GetComponent<SequenceElement>();
                if (sequenceElement) sequence.Add(sequenceElement);
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        iCurrentElement = -1;
        if(startAutomatically) Begin();
    }

    protected override void SubBegin()
    {
        StartNext();
    }

    protected override void SubUpdate()
    {
        if(iCurrentElement < 0 || iCurrentElement >= sequence.Count) return;
        if(sequence[iCurrentElement].IsFinished())
        {
            StartNext();
        }
    }

    protected override bool SubIsFinished()
    {
        return iCurrentElement >= sequence.Count;
    }

    private void StartNext()
    {
        iCurrentElement++;
        if(iCurrentElement >= sequence.Count) return;
        sequence[iCurrentElement].Begin();
    }
}
