/**
 * UI_Code_Creator
 *
 * Created by ZhangHuaGuang on 2019年11月19日
 */


using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UIFramework.Editor
{
    public class UI_Code_Creator
    {
        private static string GeneratorPath = Path.Combine(Application.dataPath, "Generator/UI");

        private static string UI_Item = "UIItem";
        private static string UI_View = "UIView";
        private static string Export = "Export";
        private static string TempletePath = "TempleteUICode.txt";
        private static string TempleteVariable = "private {0} {1};";
        private static string TempleteAssign = "transform.Find(\"{0}\")";
        private static string TempleteGetComponent = ".GetComponent<{0}>()";

        private static string DateHolder = "{DateHolder}";
        private static string ClassHolder = "{ClassHolder}";
        private static string UIObjectHolder = "{UIObjectHolder}";
        private static string VariableHolder = "{VariableHolder}";
        private static string AssignHolder = "{AssignHolder}";
        

        [MenuItem("Assets/生成UI代码")]
        static void GenerateCode()
        {
            EditorUtility.ClearProgressBar();
            UnityEngine.Object[] selObjs = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
            if (!Directory.Exists(GeneratorPath))
            {
                Directory.CreateDirectory(GeneratorPath);
            }
            for (int i = 0, count = selObjs.Length; i < count; i++)
            {
                EditorUtility.DisplayProgressBar("生成代码中 : " + i + "/" + count, "", i * 1.0f / count);
                var item = selObjs[i];
                string path = AssetDatabase.GetAssetPath(item);
                if (path.EndsWith(".prefab") && item is GameObject)
                {
                    try
                    {
                        string filePath = Path.Combine(GeneratorPath, selObjs[i].name) + ".cs";
                        GenerateCode(filePath, selObjs[i] as GameObject);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                        EditorUtility.ClearProgressBar();
                        EditorUtility.DisplayDialog("", "生成失败, 看log", "ok");
                        return;
                    }
                }
            }

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            EditorUtility.DisplayDialog("", "生成成功, 路径 : " + GeneratorPath, "ok");
            EditorUtility.ClearProgressBar();
        }
        
        /// <summary>
        /// 不对子项的文本做去重处理
        /// </summary>
        static void GenerateCode(string path, GameObject go)
        {
            if (go == null)
                return;
            if (!go.CompareTag(UI_Item) && !go.CompareTag(UI_View))
            {
                EditorUtility.DisplayDialog(go.name, go.name + "该对象没有\"UIItem\"或者\"UIView\"的tag", "OK");
                return;
            }

            var templeteAsset = (EditorGUIUtility.Load(TempletePath) as TextAsset);
            if (templeteAsset == null)
                throw new Exception("文本模板加载失败");
            string templete = templeteAsset.text;
            string dateTime = DateTime.Now.ToString("u"); // 对应 {0}
            string className = go.name; // 对应 {1}
            string uiobjectName = go.CompareTag(UI_Item) ? UI_Item : UI_View; // 对应 {2}
            StringBuilder variableBuilder = new StringBuilder(); // 对应 {3}
            StringBuilder assignBuilder = new StringBuilder(); // 对应 {4}
            
            Dictionary<string, GameObject> exportItems = new Dictionary<string, GameObject>();
            GetAllExportChildren(string.Empty, go.transform, exportItems, false);
            foreach (var item in exportItems)
            {
                GetExpression(item.Key, item.Value, variableBuilder, assignBuilder);
            }

            //string content = string.Format(templete, dateTime, className, uiobjectName, variable, assign);
            StringBuilder content = new StringBuilder(templete, templete.Length + dateTime.Length + className.Length + uiobjectName.Length + variableBuilder.Length + assignBuilder.Length + 100);
            content.Replace(DateHolder, dateTime);
            content.Replace(ClassHolder, className);
            content.Replace(UIObjectHolder, uiobjectName);
            content.Replace(VariableHolder, variableBuilder.ToString());
            content.Replace(AssignHolder, assignBuilder.ToString());
            File.WriteAllText(path, content.ToString());
        }

        // 获取所有带有”Export“的子节点
        static void GetAllExportChildren(string parent, Transform root, Dictionary<string, GameObject> list, bool isCludeParent = true)
        {
            if (list == null) return;
            string key = isCludeParent ? string.IsNullOrEmpty(parent) ? root.name : parent + "/" + root.name : string.Empty;
            if (!string.IsNullOrEmpty(key) && root.CompareTag(Export))
            {
                list.Add(key, root.gameObject);
            }
            if (root.transform.childCount == 0)
                return;
            else
            {
                for (int i = 0, count = root.childCount; i < count; i++)
                {
                    GetAllExportChildren(key, root.GetChild(i), list);
                }
            }
        }

        static void GetExpression(string path, GameObject go, StringBuilder varBuilder, StringBuilder assignBuilder)
        {
            varBuilder.Append("\t\t");
            assignBuilder.Append("\t\t\t");
            if (go.GetComponent<Image>() != null)
            {
                varBuilder.Append(string.Format(TempleteVariable, "Image", go.name));
                assignBuilder.Append(string.Format(TempleteAssign, path));
                assignBuilder.Append(string.Format(TempleteGetComponent, "Image"));
            }
            else if (go.GetComponent<Text>() != null)
            {
                varBuilder.Append(string.Format(TempleteVariable, "Text", go.name));
                assignBuilder.Append(string.Format(TempleteAssign, path));
                assignBuilder.Append(string.Format(TempleteGetComponent, "Text"));
            }
            else if (go.GetComponent<Button>() != null)
            {
                varBuilder.Append(string.Format(TempleteVariable, "Button", go.name));
                assignBuilder.Append(string.Format(TempleteAssign, path));
                assignBuilder.Append(string.Format(TempleteGetComponent, "Button"));
            }
            else
            {
                varBuilder.Append(string.Format(TempleteVariable, "GameObject", go.name));
                assignBuilder.Append(string.Format(TempleteAssign, path));
            }

            varBuilder.Append("\n");
            assignBuilder.Append(";\n");
        }
    }
}