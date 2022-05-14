using System;
using System.Collections.Generic;
using UnityEngine;

namespace PYDFramework.MVC
{
    public class Listener
    {
        public EventTypeBase eventType { get; protected set; }
        public Action<EventBase> eventListener { get; protected set; }
        protected Listener() { }

        public Listener(EventTypeBase eventType, Action<EventBase> eventListener)
        {
            this.eventType = eventType;
            this.eventListener = eventListener;
        }
    }

    public class DataChangedListener: Listener
    {
        public string dataName { get; private set; }
        public Action<DataChangedEvent> dataChangedListener { get; private set; }

        public DataChangedListener(EventTypeBase eventType, string dataName, Action<DataChangedEvent> dataChangedListener)
        {
            this.eventType = eventType;
            this.eventListener = OnRaiseEvent;
            this.dataName = dataName;
            this.dataChangedListener = dataChangedListener;
        }

        private void OnRaiseEvent(EventBase e)
        {
            var dataChangedEvent = e as DataChangedEvent;
            if (dataChangedEvent == null)
                return;
            if (dataChangedEvent.dataName != dataName)
                return;

            dataChangedListener?.Invoke(dataChangedEvent);
        }
    }

    public abstract class MVCBase : MonoBehaviour
    {
        public AppBase app => GetApp();
        protected abstract AppBase GetApp();
        private List<Listener> _listeners = new List<Listener>();

        protected void ListenDataChanged(EventTypeBase eventType, string dataName, Action<DataChangedEvent> dataChangedEvent)
        {
            var dataChangedListener = new DataChangedListener(eventType, dataName, dataChangedEvent);
            _listeners.Add(dataChangedListener);
            app.eventManager.Listen(dataChangedListener.eventType, dataChangedListener.eventListener);
        }

        protected void Listen(EventTypeBase eventType, Action<EventBase> eventListenner)
        {
            app.eventManager.Listen(eventType, eventListenner);
            _listeners.Add(new Listener(eventType, eventListenner));
        }

        private void UnListen(EventTypeBase eventType, Action<EventBase> eventListener)
        {
            app.eventManager.UnListen(eventType, eventListener);
        }

        protected virtual void OnDestroy()
        {
            foreach(var listener in _listeners)
            {
                UnListen(listener.eventType, listener.eventListener);
            }

            _listeners.Clear();
        }
    }

    public class MVCBase<_App>: MVCBase where _App: AppBase
    {
        new public _App app { get { return base.app as _App; } }

        protected override AppBase GetApp() => Singleton<_App>.instance;
    }

}