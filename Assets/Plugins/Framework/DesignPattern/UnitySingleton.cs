/**
 * UnitySingleton
 *
 * Created by ZhangHuaGuang on 2019年12月9日
 */


using UnityEngine;

namespace Framework.DesignPattern
{
    public class UnitySingleton<T> : MonoBehaviour where T : UnitySingleton<T>
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                lock (instance)
                {
                    if (instance == null)
                    {
                        var temp = new GameObject(typeof(T).Name);
                        instance = temp.AddComponent<T>();
                        temp.transform.SetParent(instance.GetParent());
                        if (instance.AutoBegin())
                        {
                            instance.BeginSingleton();
                        }
                    } 
                }

                return instance;
            }
        }

        // 是否自动初始化
        protected virtual bool AutoBegin()
        {
            return false;
        }

        // 挂点
        protected virtual Transform GetParent()
        {
            return UnitySingletonRoot.Instance.transform;
        }
        
        public virtual void BeginSingleton()
        {
            
        }

        public virtual void ResetSingleton()
        {
            
        }

        public virtual void StopSingleton()
        {
            
        }
    }

    public class UnitySingletonRoot : UnitySingleton<UnitySingletonRoot>
    {
        protected override bool AutoBegin()
        {
            return true;
        }

        protected override Transform GetParent()
        {
            return null;
        }

        public override void BeginSingleton()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}