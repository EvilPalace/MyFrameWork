/**
 * Singleton
 *
 * Created by ZhangHuaGuang on 2019年12月9日
 */


using System;

namespace Framework.DesignPattern
{
    public abstract class Singleton<T>  where T : Singleton<T>
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
                        instance = Activator.CreateInstance<T>();
                        if (instance.AutoBegin())
                        {
                            instance.BeginSingleton();
                        }
                    }
                }
                return instance;
            }
        }

        protected virtual bool AutoBegin()
        {
            return false;
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
}