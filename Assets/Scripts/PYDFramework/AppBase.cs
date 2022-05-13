using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

namespace PYDFramework.MVC
{
    /// <summary>
    /// MVC Framework 
    /// Author: Thong Vo
    /// </summary>
    public abstract class AppBase
    {
        public static EventTypeBase UpdateEvent = new EventTypeBase("AppBase.UpdateEvent");
        public static EventTypeBase FixedUpdateEvent = new EventTypeBase("AppBase.FixedUpdateEvent");
        public EventManager eventManager;
        private EventBase _updateEvent;
        private EventBase _fixedUpdateEvent;

        public AppBase()
        {
            eventManager = new EventManager();
            _updateEvent = new EventBase(UpdateEvent, this);
            _fixedUpdateEvent = new EventBase(FixedUpdateEvent, this);
            OnInit();
        }

        public void Update()
        {
            eventManager.Dispatch(_updateEvent);
            OnUpdate();
        }

        public void FixedUpdate()
        {
            eventManager.Dispatch(_fixedUpdateEvent);
            OnFixedUpdate();
        }

        public virtual void OnInit() { }
        public virtual void OnUpdate() { }
        public virtual void OnFixedUpdate() { }
    }

}