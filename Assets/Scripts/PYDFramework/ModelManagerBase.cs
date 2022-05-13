using System.Collections.Generic;
using UnityEngine;

namespace PYDFramework.MVC
{
    public class ModelManagerBase
    {
        private List<ModelBase> _modelList = new List<ModelBase>();

        protected T RegisterModel<T>(out T model) where T: ModelBase, new()
        {
            model = GetModel<T>();
            if (model != null)
            {
                Debug.LogErrorFormat("Model {0} is dupplicate", model.GetType());
                return model;
            }

            model = new T();
            _modelList.Add(model);
            return model;
        }

        private T GetModel<T>() where T : ModelBase
        {
            return _modelList.Find((m) => m.GetType() == typeof(T)) as T;
        }

        public void WriteAll()
        {
            foreach(var model in _modelList)
            {
                WriteModel(model);
            }
        }

        protected void LoadData()
        {
            foreach(var model in _modelList)
            {
                if(!ReadModel(model))
                {
                    model.InitBaseData();
                    model.OnAfterDeserialized();
                }
            }
        }

        public void WriteModel<T>() where T: ModelBase
        {
            var model = GetModel<T>();
            WriteModel(model);
        }

        protected bool ReadModel<T>() where T: ModelBase
        {
            var model = GetModel<T>();
            return ReadModel(model);
        }

        private void WriteModel(ModelBase modelBase)
        {
            var json = modelBase.GetDataJson();
            PlayerPrefs.SetString(GetKey(modelBase), json);
            PlayerPrefs.Save();
        }

        private bool ReadModel(ModelBase modelBase)
        {
            var json = PlayerPrefs.GetString(GetKey(modelBase), "");
            if (json == "" || json == "{}")
                return false;

            modelBase.InitWithJsonData(json);
            return true;
        }

        private string GetKey(ModelBase modelBase)
        {
            return string.Format("PYD_MVC_{0}", modelBase.GetType().Name.ToUpper());
        }
    }
}