using Combat;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public enum Type
    {
        damageTaken
    }

    [SerializeField] private Type type;
    [SerializeField] private GameObject relevantGameObject;
    [SerializeField] private string relevantGameObjectTag;

    [SerializeReference] private GameAction action;

    private SubscriptionManager delegateSubscriptions;

    void Awake()
    {
        delegateSubscriptions = new();
    }

    void OnEnable()
    {
        delegateSubscriptions.UnsubscribeAll();
        if(relevantGameObjectTag != "")
        {
            GameObject newRelevantGameObject = GameObject.FindWithTag(relevantGameObjectTag);
            if(newRelevantGameObject) relevantGameObject = newRelevantGameObject;
        }

        switch(type)
        {
        case Type.damageTaken:
            var handler = new Combatant.OnHealthChanged(change => { if (change < 0) RunAction(); });
            Combat.Combatant combatant = relevantGameObject.GetComponent<Combatant>();
            delegateSubscriptions.Subscribe(handler,
                h => combatant.onHealthChanged += h,
                h => combatant.onHealthChanged -= h);
            break;
        default:
            break;
        }
    }

    void OnDisable()
    {
        delegateSubscriptions.UnsubscribeAll();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RunAction()
    {
        action.Execute();
    }
}
