using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace PYDFramework.MVC
{
    public class DataChangedEvent: EventBase
    {
        public string dataName { get; private set; }

        public DataChangedEvent(EventTypeBase eventType, object sender, string dataName) : base(eventType, sender)
        {
            this.dataName = dataName;
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public abstract class ModelBase
    {
        public AppBase app => GetApp();
        protected abstract AppBase GetApp();
        public EventTypeBase eventType { get; private set; }

        public ModelBase(EventTypeBase eventType)
        {
            this.eventType = eventType;
        }

        protected void RaiseDataChanged(string nameProperty)
        {
            if (app == null)
                return;

            app.eventManager.Dispatch(new DataChangedEvent(eventType, this, nameProperty));
        }

        public virtual void InitWithJsonData(string jsonData)
        {
            JsonConvert.PopulateObject(jsonData, this);
            RaiseDataChanged("");
        }

        public virtual string GetDataJson()
        {
            string dataJson = "";
            try
            {
                dataJson = JsonConvert.SerializeObject(this, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
            }
            catch (Exception e)
            {
                Debug.LogAssertionFormat("JsonConvert fail! {0}", GetType().Name);
                Debug.LogError(e);
            }

            return dataJson;
        }

        public virtual void InitBaseData() { }

        [OnDeserialized]
        private void ProcessOnDeserialized(StreamingContext context)
        {
            OnAfterDeserialized();
        }

        [OnSerializing]
        private void ProcessOnSerializing(StreamingContext context)
        {
            OnBeforeSerializing();
        }

        public virtual void OnAfterDeserialized() { }

        public virtual void OnBeforeSerializing() { }
    }

    public class Model<_App> : ModelBase where _App : AppBase
    {
        new public _App app { get { return base.app as _App; } }

        protected override AppBase GetApp() => Singleton<_App>.instance;

        public Model(EventTypeBase eventType) : base(eventType) { }
    }
}
