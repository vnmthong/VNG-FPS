using System;
using System.Collections.Generic;

namespace PYDFramework.MVC
{
    public class EventInvoker<T>
    {
        public List<Action<T>> listeners { get; private set; } = new List<Action<T>>();

        public void AddListener(Action<T> listener)
        {
            listeners.Add(listener);
        }

        public void RemoveListener(Action<T> listener)
        {
            listeners.Remove(listener);
        }

        public void Invoke(T e)
        {
            listeners.ForEach(listener => listener.Invoke(e));
        }
    }

    public class EventInkoverBase: EventInvoker<EventBase> { }
}
