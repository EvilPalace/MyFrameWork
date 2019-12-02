/**
 * ProjectInitHandle
 *
 * Created by ZhangHuaGuang on 2019年11月19日
 */

using UnityEditor;

namespace UIFramework.Editor
{
    public class ProjectInitHandle
    {
        #region Tag检查

        private static string[] tags = new string[]
        {
            "Export",
            "UIItem",
            "UIView",
        };

        [MenuItem("Evil/Tools/CheckTags")]
        public static void CheckTag()
        {
            // Open tag manager
            SerializedObject tagManager =
                new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            // Tags Property
            SerializedProperty tagsProp = tagManager.FindProperty("tags");
            //Debug.Log("TagsPorp Size:" + tagsProp.arraySize);
            tagsProp.ClearArray();
            tagManager.ApplyModifiedProperties();
            //Debug.Log("TagsPorp Size:" + tagsProp.arraySize);
            for (int i = 0; i < tags.Length; i++)
            {
                // Insert new array element
                tagsProp.InsertArrayElementAtIndex(i);
                SerializedProperty sp = tagsProp.GetArrayElementAtIndex(i);
                // Set array element to tagName
                sp.stringValue = tags[i];
                tagManager.ApplyModifiedProperties();
            }

            EditorUtility.DisplayDialog("", "Tag设置成功", "ok");
        }

        #endregion
    }
}