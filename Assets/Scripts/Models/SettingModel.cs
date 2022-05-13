using PYDFramework.MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VNGFPS
{
    public class SettingModel : Model<GameApp>
    {
        public static EventTypeBase DataChangedEvent = new EventTypeBase(nameof(SettingModel) + ".DataChangedEvent");
        
        public SettingModel(): base (DataChangedEvent)
        {

        }

        private bool _isMusic;

        public bool isMusic
        {
            get => _isMusic;
            set
            {
                if (value == _isMusic)
                    return;

                _isMusic = value;
                RaiseDataChanged(nameof(isMusic));
            }
        }
    }
}
