using System;
using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.Events;

namespace VNGFPS
{
    public class HandView: MonoBehaviour
    {
        [SerializeField] WeaponController weaponController;
        [SerializeField] Animator anim;
        private void Start()
        {
            weaponController.OnShoot += OnShot;
            weaponController.OnCooling += OnCharging;
        }

        private void OnDestroy()
        {
            if (!weaponController)
                return;

            weaponController.OnShoot -= OnShot;
            weaponController.OnCooling -= OnCharging;
        }

        private void OnCharging(bool isCharging)
        {
            if (isCharging)
            {
                var rate = weaponController.AmmoReloadRate;
                var ammoCurrent = weaponController.CurrentAmmo;
                var timeReload = (weaponController.MaxAmmo - ammoCurrent) / rate;
                var timeOrigin = 2.133f;
                var mul = timeOrigin / timeReload;
                anim.SetFloat("ReloadSpeed", mul);
                anim.SetTrigger("Reload");
            }
        }

        private void OnShot()
        {
            //anim.SetTrigger("Fire");
        }
    }
}
