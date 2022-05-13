using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PYDFramework
{
    public static class PYDHelper
    {
        /// <summary>
        /// Amount return float from 0.0 to 1.0
        /// </summary>
        /// <param name="currentValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="defaultValue">Value default from 0.0 to 1.0</param>
        /// <returns></returns>
        public static float GetFillAmount(float currentValue, float maxValue, float defaultValue = 0)
        {
            if (defaultValue > 1)
                defaultValue = 1f;

            if (defaultValue < 0)
                defaultValue = 0;

            var leftValue = 1f - defaultValue;
            var amount = currentValue / maxValue;

            return defaultValue + (amount * leftValue);
        }

        /// <summary>
        /// Get if available else add new
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <returns></returns>
        public static T GetOrAddComponent<T>(this GameObject go) where T: Component
        {
            var component = go.GetComponent<T>();
            if (!component)
                component = go.AddComponent<T>();

            return component;
        }

        /// <summary>
        /// Wait untill done async
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        public static IEnumerator WaitFor(AsyncOperation operation)
        {
            var isComplete = false;
            operation.completed += o =>
            {
                isComplete = true;
            };
            while (!isComplete)
                yield return new WaitForEndOfFrame();
        }

        /// <summary>
        /// Parse enum with fallback value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T EnumParse<T>(string str, T defaultValue) where T: struct
        {
            if (Enum.TryParse(str, out T result))
                return result;

            return defaultValue;
        }

        /// <summary>
        /// Format string replace regix not log catch exception
        /// </summary>
        /// <param name="str"></param>
        /// <param name="paramValues"></param>
        /// <returns></returns>
        public static string Format(string str, params string[] paramValues)
        {
            var result = str;

            for(int i = 0; i< paramValues.Length; i++)
            {
                var regix = "{" + i + "}";
                result = result.Replace(regix, paramValues[i]);
            }

            return result;
        }

        /// <summary>
        /// Clamp 0 to max value
        /// </summary>
        /// <param name="max"></param>
        /// <param name="maxClamp"></param>
        /// <param name="currentValue"></param>
        /// <returns></returns>
        public static float ClampMax(float max, float maxClamp, float currentValue)
        {
            return currentValue * maxClamp / max;
        }

        /// <summary>
        /// Random success float value
        /// </summary>
        /// <param name="percent"></param>
        /// <returns></returns>
        public static bool RandomSuccess(float percent)
        {
            return UnityEngine.Random.Range(1f, 100f) <= percent;
        }

        /// <summary>
        /// Random success int value
        /// </summary>
        /// <param name="percent"></param>
        /// <returns></returns>
        public static bool RandomSuccess(int percent)
        {
            return UnityEngine.Random.Range(1, 101) <= percent;
        }

        /// <summary>
        /// Rotate direction vector by angle
        /// </summary>
        /// <param name="direction">direction vector</param>
        /// <param name="angle">value angle</param>
        /// <returns></returns>
        public static Vector3 RotateAngle(Vector3 direction, float angle)
        {
            return Quaternion.AngleAxis(angle, Vector3.forward) * direction;
        }

        /// <summary>
        /// Return all rotate angle from direction vector
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="totalAngle"></param>
        /// <param name="directionOutputAmount"></param>
        /// <returns></returns>
        public static List<Vector3> RotateAngle(Vector3 direction, float totalAngle, int directionOutputAmount)
        {
            var difference = totalAngle / (directionOutputAmount - 1);
            var isHasCenter = directionOutputAmount % 2 != 0;
            var result = new List<Vector3>();
            var center = directionOutputAmount / 2;
            var angle = isHasCenter ? difference * center * -1 : (difference * center * -1) + difference / 2;
            
            for (int i = 0; i < directionOutputAmount; i++)
            {
                var dir = RotateAngle(direction, angle);
                result.Add(dir);
                angle += difference;
            }

            return result;
        }

        public static Color ColorParse(string colorHex)
        {
            if (ColorUtility.TryParseHtmlString(colorHex, out Color color))
                return color;

            return Color.white;
        }
    }
}
