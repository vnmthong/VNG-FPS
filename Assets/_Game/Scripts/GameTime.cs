using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VNGFPS
{
    public class GameTime
    {
        public static float timeScale { get; private set; } = 1f;
        public static float deltaTime { get; private set; }
        public static float fixedDeltaTime { get; private set; }

        public static void Update(float deltaTime)
        {
            GameTime.deltaTime = timeScale * deltaTime;
        }

        public static void FixedUpdate(float fixedDeltaTime)
        {
            GameTime.fixedDeltaTime = timeScale * fixedDeltaTime;
        }

        public static void SetTimeScale(float timeScale)
        {
            GameTime.timeScale = Mathf.Clamp(timeScale, 0f, 10f);
        }    

        public static IEnumerator WaitForEndOfFrame()
        {
            yield return WaitForSeconds(Time.unscaledDeltaTime);
        }
        
        public static IEnumerator WaitForSeconds(float second)
        {
            var elapse = 0f;
            while(elapse < second)
            {
                yield return null;
                elapse += deltaTime;
            }    
        }

        public static IEnumerator WaitForFixedUpdate()
        {
            yield return WaitForFixedUpdate(Time.fixedUnscaledDeltaTime);
        }

        public static IEnumerator WaitForFixedUpdate(float second)
        {
            var elapse = 0f;
            while (elapse < second)
            {
                yield return new WaitForFixedUpdate();
                elapse += fixedDeltaTime;
            }
        }

        public static IEnumerator WaitUntil(Func<bool> predicate)
        {
            while(!predicate.Invoke())
            {
                yield return WaitForEndOfFrame();
            }
        }    
    }
}
