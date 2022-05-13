using PYDFramework.MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VNGFPS
{
    public class ModelManager : ModelManagerBase
    {
        //Models
        public SettingModel settingModel;

        public void Init()
        {
            //Register
            RegisterModel(out settingModel);

            LoadData();
        }
    }

}
