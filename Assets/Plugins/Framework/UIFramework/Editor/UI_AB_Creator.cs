/**
 * UI_AB_Creator
 *
 * Created by ZhangHuaGuang on 2019年11月19日
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace UIFramework.Editor
{
    public class UI_AB_Creator
    {
        // 如果量级少，统一放一个AB里，统一命名为这
        private static string UIABPath = "ui/total";
        
        [MenuItem("Evil/UI/SetABPath")]
        private static void SetABPath()
        {
            var allPath = AssetDatabase.GetAllAssetPaths();
            string root = "UI";
            string itemTag = "UIItem";
            string viewTag = "UIView";
            bool isDiff = EditorUtility.DisplayDialog("对于UI的AB路径处理", string.Format("对于{0},{1},是否用各自的名字及路径走AB名", itemTag, viewTag), "相异", "同一个");
            Dictionary<string, string> uiRegisterDic = new Dictionary<string, string>();
            float index = 0;
            int count = allPath.Length;
            foreach (var path in allPath)
            {
                EditorUtility.DisplayProgressBar("AB路径设置", path, index++ / count);
                if (path.EndsWith(".prefab"))
                {
                    GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    if (go && (go.CompareTag(itemTag) || go.CompareTag(viewTag)))
                    {
                        AssetImporter importer = AssetImporter.GetAtPath(path);
                        if (importer != null)
                        {
                            string abPath = path.Substring(path.IndexOf("UI", StringComparison.Ordinal));
                            abPath = isDiff ? abPath.Substring(0, abPath.LastIndexOf(".")) : UIABPath;
                            importer.assetBundleName = abPath;
                            uiRegisterDic[go.name] = abPath;
                        }                        
                    }
                }
            }
            AssetDatabase.RemoveUnusedAssetBundleNames();
            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog("提醒", "设置UI的AB名字结束！\n 是否相同：" + (isDiff ? "相异" : "相同"), "OK");
            // 去注册
            RegisterString(uiRegisterDic);
        }

        private static string UIManagerRegisterPath = Path.Combine(Application.dataPath, "Generator/UI/Register");
        private static string TempleteFunction = "UIManager.Register<{0}>(\"{1}\");";
        private static string DateHolder = "{Date}";
        private static string RegisterHolder = "{Register}";
        private static void RegisterString(Dictionary<string, string> data)
        {
            if (data == null) return;
            var tempObj = EditorGUIUtility.Load("TempleteUIManagerRegister.txt") as TextAsset;
            string uimanagerRegister = Path.Combine(UIManagerRegisterPath, "UIManager.Register.cs");
            if (tempObj != null)
            {
                var textObj = tempObj;
                StringBuilder templeteBuilder = new StringBuilder(textObj.text);
                string date = DateTime.Now.ToString("u");
                StringBuilder register = new StringBuilder();
                foreach (var item in data)
                {
                    register.Append("\t\t\t");
                    register.Append(string.Format(TempleteFunction, item.Key, item.Value));
                }

                templeteBuilder.Replace(DateHolder, date);
                templeteBuilder.Replace(RegisterHolder, register.ToString());
                
                if (!Directory.Exists(UIManagerRegisterPath))
                {
                    Directory.CreateDirectory(UIManagerRegisterPath);
                }
                File.WriteAllText(uimanagerRegister, templeteBuilder.ToString());
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
                EditorUtility.DisplayDialog("UI注册", "UIManager注册成功! \n个数 : " + data.Count, "ok");
            }
        
        }
    }
}