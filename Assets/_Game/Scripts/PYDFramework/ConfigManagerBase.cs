using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace PYDFramework.Config
{
    public class ConfigManagerBase
    {
        public List<IConfigs> _configList { get; private set; } = new List<IConfigs>();

        public T GetConfig<T>() where T : IConfigs
        {
            return (T)_configList.FirstOrDefault((m) => m.GetType() == typeof(T));
        }

        protected T Register<T>(out T config) where T : IConfigs, new()
        {
            config = GetConfig<T>();
            if (config != null)
            {
                Debug.LogAssertionFormat("Model '{0}' Duplicated!", typeof(T).Name);
                return config;
            }

            config = new T();

            _configList.Add(config);

            return config;
        }

        protected void LoadConfigs()
        {
            foreach (var config in _configList)
                config.Load();
        }
    }
}