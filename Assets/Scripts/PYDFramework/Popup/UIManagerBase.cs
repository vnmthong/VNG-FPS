using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PYDFramework.MVC
{
    public abstract class UIManagerBase<T>: MonoBehaviour
    {
        public Transform rootContainer;

        private Dictionary<T, GameObject> _resourcePathDic = new Dictionary<T, GameObject>();
        private Dictionary<T, IPopup> _popupDic = new Dictionary<T, IPopup>();

        public Action onEmptyPopup;
        protected bool RegisterPopup(T type, GameObject prefab)
        {
            if(HasPopup(type))
            {
                Debug.LogErrorFormat("Popup {0} is dupplicate", type);
                return false;
            }
            _resourcePathDic.Add(type, prefab);
            return true;
        }

        public virtual GameObject ShowPopup(T type, Action<GameObject> onInit = null)
        {
            if(_popupDic.ContainsKey(type))
            {
                Debug.LogErrorFormat("Popup {0} is showing", type);
                return null;
            }

            if(!_resourcePathDic.TryGetValue(type, out GameObject prefab))
            {
                Debug.LogErrorFormat("Popup {0} is not found", type);
                return null;
            }

            var popupPrefab = prefab;

            var popupGo = Instantiate(popupPrefab);
            onInit?.Invoke(popupGo);
            popupGo.transform.SetParent(rootContainer, false);

            var popupComponent = popupGo.GetComponent<IPopup>();
            Debug.AssertFormat(popupComponent != null, "Prefabs {0} not have Popup Component", popupPrefab.name);
            var destroyCallback = popupGo.GetOrAddComponent<DestroyCallback>();
            destroyCallback.onClose += () =>
              {
                  ClosePopup(type);
              };

            var sortingGroup = popupGo.GetOrAddComponent<Canvas>();
            sortingGroup.overrideSorting = true;
            sortingGroup.sortingOrder = _resourcePathDic.Keys.ToList().IndexOf(type);

            _popupDic.Add(type, popupComponent);

            return popupGo;
        }

        public bool IsAnyPopup()
        {
            return _popupDic.Count > 0;
        }

        public void CloseAllPopup()
        {
            foreach(KeyValuePair<T,IPopup> item in _popupDic.ToList())
            {
                item.Value.Close();
            }    
        }

        private void ClosePopup(T type)
        {
            if (!_popupDic.ContainsKey(type))
                return;

            _popupDic.Remove(type);

            if (!IsAnyPopup())
                onEmptyPopup?.Invoke();
        }

        private bool HasPopup(T type)
        {
            if (_resourcePathDic.ContainsKey(type))
                return true;
            return false;
        }
    }
}
