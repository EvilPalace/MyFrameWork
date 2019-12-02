/**
 * ABLoader
 *
 * Created by ZhangHuaGuang on 2019年11月19日
 */


using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ABLoad
{
    public static class ABLoader
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly string ABRootPath = Path.Combine(Application.streamingAssetsPath, "AssetBundles");
        
        /// <summary>
        /// 基于平台的AssetBundle的根路径
        /// </summary>
        public static readonly string ABPath = Path.Combine(ABRootPath,
#if UNITY_STANDALONE_WIN
            "win"
#elif UNITY_ANDROID
            "android"
#elif UNITY_IOS
            "ios"
#endif
            );

#if UNITY_EDITOR
        
        public static string GetABPath(BuildTarget buildTarget)
        {
            string tempPath = string.Empty;
            switch (buildTarget)
            {
                case BuildTarget.Android:
                    tempPath = "android";
                    break;
                case BuildTarget.iOS:
                    tempPath = "ios";
                    break;
                case BuildTarget.StandaloneWindows:
                    tempPath = "win";
                    break;
            }

            if (string.IsNullOrEmpty(tempPath))
            {
                throw new Exception("路径异常");
            }

            return Path.Combine(ABRootPath, tempPath);
        }
#endif

        #region 资源加载

        private static Dictionary<string, ABContainer> AbContainers = new Dictionary<string, ABContainer>();

        /// <summary>
        /// 获取AB包
        /// </summary>
        public static ABContainer LoadAB(string abName, bool isLoad = true)
        {
            ABContainer ret = null;
            abName = abName.ToLower();
            if (!AbContainers.TryGetValue(abName, out ret))
            {
                ret = new ABContainer(abName, isLoad);
            }

            ret.ReferenceCount.Increment(1);
            return ret;
        }

        public static void LoadAB(ABContainer container)
        {
            if (container == null)
                return;
            container.ReferenceCount.Increment(1);
        }

        /// <summary>
        /// 卸载AB，此处只是在总管理处做引用-1处理；当引用为0时，释放资源
        /// </summary>
        public static void UnLoadAB(ABContainer container)
        {
            if (container == null)
                return;
            container.ReferenceCount.Decrement(1);
        }

        #endregion


        public static void Initialize()
        {
            
        }
    }
}