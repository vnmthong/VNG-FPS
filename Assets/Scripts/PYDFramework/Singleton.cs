using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PYDFramework
{
    public class Singleton<T> where T : class
    {
        public static T instance { get; private set; }

        public static T Set(T _instance)
        {
            if (instance != null)
                Debug.LogErrorFormat("Singleton<{0}> is overided", typeof(T).Name);
            instance = _instance;
            return instance;
        }

        public static void Clear()
        {
            instance = null;
        }

        public static void Unset(T _instance)
        {
            if (_instance == instance)
                instance = null;
        }
    }
}
