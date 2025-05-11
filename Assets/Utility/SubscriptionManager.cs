using System;
using System.Collections.Generic;

public class SubscriptionManager
{
    public class Subscription
    {
        private readonly Action _unsubscribe;

        public Subscription(Action unsubscribe)
        {
            _unsubscribe = unsubscribe ?? throw new ArgumentNullException(nameof(unsubscribe));
        }

        public void Unsubscribe() => _unsubscribe();
    }

    private readonly List<Subscription> _subscriptions = new();

    public Subscription Subscribe<T>(T handler, Action<T> subscribe, Action<T> unsubscribe)
        where T : Delegate
    {
        subscribe(handler);
        var sub = new Subscription(() => unsubscribe(handler));
        _subscriptions.Add(sub);
        return sub;
    }

    public void UnsubscribeAll()
    {
        foreach (var sub in _subscriptions)
            sub.Unsubscribe();
        _subscriptions.Clear();
    }
}
