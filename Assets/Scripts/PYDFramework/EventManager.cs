using System;
using System.Collections.Generic;

namespace PYDFramework.MVC
{
    public class EventManager
    {
        private Dictionary<EventTypeBase, EventInkoverBase> _invokerDic;

        public EventManager()
        {
            _invokerDic = new Dictionary<EventTypeBase, EventInkoverBase>();
        }
        public void Dispatch(EventBase e)
        {
            if(_invokerDic.TryGetValue(e.eventType,out EventInkoverBase thisEvent))
            {
                thisEvent.Invoke(e);
            }
        }

        public void Dispatch(EventTypeBase eventType, object sender)
        {
            Dispatch(new EventBase(eventType, sender));
        }

        public void Listen(EventTypeBase eventType, Action<EventBase> listener)
        {
            if(_invokerDic.TryGetValue(eventType, out EventInkoverBase thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new EventInkoverBase();
                thisEvent.AddListener(listener);
                _invokerDic.Add(eventType, thisEvent);
            }
        }

        public void UnListen(EventTypeBase eventType, Action<EventBase> listener)
        {
            if (_invokerDic.TryGetValue(eventType, out EventInkoverBase thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        public void Clear()
        {
            _invokerDic.Clear();
        }
    }

}