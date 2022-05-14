using UnityEngine;

namespace PYDFramework.MVC
{
    public class AppRunner : MonoBehaviour
    {
        public AppBase app;

        protected virtual void Update()
        {
            app.Update();
        }

        protected virtual void FixedUpdate()
        {
            app.FixedUpdate();
        }
    }

    public abstract class AppRunner<_App>: AppRunner where _App: AppBase
    {
        protected abstract _App CreateApp();

        protected virtual void Awake()
        {
            //DontDestroyOnLoad(gameObject);
            app = CreateApp();
        }
    }

}