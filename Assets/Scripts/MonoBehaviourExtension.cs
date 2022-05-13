using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VNGFPS
{
    public class MonoBehaviourExtension : MonoBehaviour
    {
        public static void ClearAllChild(Transform parent)
        {
            foreach (Transform item in parent)
            {
                Destroy(item.gameObject);
            }
        }
    }
}
