using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//using UnityEngine.UI;

namespace PYDFramework.MVC
{
    public abstract class ViewBase : MVCBase
    {
        public abstract class DataKeyBase: IDisposable
        {
            public readonly string key;
            public readonly Component control;

            public DataKeyBase(string key, Component control)
            {
                this.key = key;
                this.control = control;
            }

            public virtual void Dispose() { }

            public abstract void Notify(EventBase eventBase);
        }

        public class DataKey<Control> : DataKeyBase where Control: Component
        {
            public readonly Action<Control, EventBase> notifyCallback;

            public DataKey(string key, Component control, Action<Control, EventBase> notifyCallback) : base(key, control)
            {
                this.notifyCallback = notifyCallback;
            }

            public override void Notify(EventBase eventBase)
            {
                if (control)
                    notifyCallback.Invoke(control as Control, eventBase);
            }
        }

        private bool _isInited = false;
        private Dictionary<EventTypeBase, List<DataKeyBase>> _eventDataKeyDic;
        private Dictionary<string, DataKeyBase> _keyDataKeyDic;
        private EventTypeBase _eventTypeNotify = new EventTypeBase("Notify");
        protected void ViewInit()
        {
            _eventDataKeyDic = new Dictionary<EventTypeBase, List<DataKeyBase>>();
            _keyDataKeyDic = new Dictionary<string, DataKeyBase>();
            OnViewInit();
            NotifyAllDataChanged();
            _isInited = true;
        }

        //protected void AddClick(Button button,UnityAction action)
        //{
        //    button.onClick.AddListener(action);
        //}

        protected DataKeyBase AddDataKeyListen<T>(string key, T control, Action<T,EventBase> notifyCallback, params EventTypeBase[] eventTypes) where T: Component
        {
            Debug.AssertFormat(control, "Control view key:{0}-{1} cannot null", key, GetType().Name);

            if(_keyDataKeyDic.TryGetValue(key,out DataKeyBase dataKeyBase))
                Debug.AssertFormat(false, "Control view key:{0}-{1} is dupplicate", key, GetType().Name);
            else
            {
                var dataKey = new DataKey<T>(key, control, notifyCallback);
                _keyDataKeyDic.Add(key, dataKey);

                foreach(var eventType in eventTypes)
                {
                    RegisterEventListenner(eventType, dataKey);
                }
            }
            return dataKeyBase;
        }    

        protected void RegisterEventListenner(EventTypeBase eventType, DataKeyBase dataKeyBase)
        {
            if(!_eventDataKeyDic.TryGetValue(eventType,out List<DataKeyBase> dataKeys))
            {
                dataKeys = new List<DataKeyBase>();
                _eventDataKeyDic.Add(eventType, dataKeys);
                Listen(eventType, NotifyEvent);
            }

            dataKeys.Add(dataKeyBase);
        }

        private void NotifyEvent(EventBase eventBase)
        {
            if (!_eventDataKeyDic.TryGetValue(eventBase.eventType, out List<DataKeyBase> dataKeys))
                return;

            foreach(var dataKey in dataKeys)
            {
                dataKey.Notify(eventBase);
            }
        }

        protected void NotifyDataChanged(string key)
        {
            if(!_keyDataKeyDic.TryGetValue(key, out DataKeyBase dataKey))
            {
                Debug.LogErrorFormat("Key notify {0} not found", key);
                return;
            }

            dataKey.Notify(new EventBase(_eventTypeNotify, this));
        }

        protected void NotifyAllDataChanged()
        {
            foreach(var dataKey in _keyDataKeyDic.Values)
            {
                dataKey.Notify(new EventBase(_eventTypeNotify, this));
            }
        }

        protected virtual void OnViewInit() { }

        protected virtual void Start()
        {
            ViewInit();
        }

        protected virtual void OnEnable()
        {
            if(_isInited)
                NotifyAllDataChanged();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (!_isInited)
                return;

            foreach(var dataKey in _keyDataKeyDic.Values)
            {
                dataKey?.Dispose();
            }
            _keyDataKeyDic.Clear();
            _eventDataKeyDic.Clear();
        }

        protected bool IsInited => _isInited;
    }

    public class View<_App>: ViewBase where _App:AppBase
    {
        new public _App app { get { return base.app as _App; } }
        protected override AppBase GetApp() => Singleton<_App>.instance;
    }

}