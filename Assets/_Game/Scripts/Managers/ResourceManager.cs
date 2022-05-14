using PYDFramework;
using PYDFramework.MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VNGFPS
{
    public class ResourceManager : UIManagerBase<PopupType>
    {
        [Header("Transform parents")]
        [SerializeField] public Transform trfUINotInteractable;
        [SerializeField] public Transform trfRootGame;
        [Header("Popup prefabs")]
        [SerializeField] GameObject popupSelectCharacter;

        private GameApp _app => Singleton<GameApp>.instance; 

        private void Awake()
        {
            Singleton<ResourceManager>.Set(this);
            Init();
        }

        private void OnDestroy()
        {
            Singleton<ResourceManager>.Unset(this);
        }

        public void Init()
        {
            RegisterPopup(PopupType.SelectCharacter, popupSelectCharacter);
            
        }

        public override GameObject ShowPopup(PopupType type, Action<GameObject> onInit = null)
        {
            var popupGo = base.ShowPopup(type, onInit);
            if (!popupGo)
                return null;
            popupGo.GetOrAddComponent<GraphicRaycaster>();
            return popupGo;
        }

    }
}
