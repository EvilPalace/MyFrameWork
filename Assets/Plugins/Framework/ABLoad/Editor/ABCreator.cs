/**
 * ABCreator
 *
 * Created by ZhangHuaGuang on 2019年11月19日
 */


using System;
using System.IO;
using UnityEditor;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

namespace ABLoad
{
    public class ABCreator
    {
        [MenuItem("Evil/Build AssetBundles/Clear && Build by Platform")]
        static void BuildAllAssetBundles()
        {
            string path = ABLoader.ABPath;
            if (Directory.Exists(ABLoader.ABRootPath))
                Directory.Delete(ABLoader.ABRootPath, true);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            EditorUtility.DisplayProgressBar("AssetBundle打包", "正在打包ing ...", 0);
            BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None,
#if UNITY_STANDALONE_WIN
                BuildTarget.StandaloneWindows
#elif UNITY_ANDROID
                BuildTarget.Android
#elif UNITY_IOS
                BuildTarget.iOS
#endif
            );
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            
            AssetDatabase.RemoveUnusedAssetBundleNames();
            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog("提醒", "打包AB包结束！\n[path:"+ path + "]", "OK");
        }
        
        [MenuItem("Evil/Build AssetBundles/Platform/Windows")]
        static void BuildAllAssetBundlesWin()
        {
            string path = ABLoader.GetABPath(BuildTarget.StandaloneWindows);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None,
                BuildTarget.StandaloneWindows);
            EditorUtility.DisplayDialog("提醒", "打包AB包结束！\n[path:"+ path + "]", "OK");
        }
        
        [MenuItem("Evil/Build AssetBundles/Platform/Android")]
        static void BuildAllAssetBundlesAndroid()
        {
            string path = ABLoader.GetABPath(BuildTarget.Android);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None,
                BuildTarget.Android);
            EditorUtility.DisplayDialog("提醒", "打包AB包结束！\n[path:"+ path + "]", "OK");
        }
        
        [MenuItem("Evil/Build AssetBundles/Platform/IOS")]
        static void BuildAllAssetBundlesIOS()
        {
            string path = ABLoader.GetABPath(BuildTarget.iOS);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None,
                BuildTarget.iOS);
            EditorUtility.DisplayDialog("提醒", "打包AB包结束！\n[path:"+ path + "]", "OK");
        }
        
        
    }
}