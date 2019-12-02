using System;
using System.Collections;
using System.Collections.Generic;
using ABLoad;
using UnityEngine;
using UniRx;

namespace UIFramework
{

	public static partial class UIManager
	{
		private static Dictionary<UILayer, Transform> LayerRoot = new Dictionary<UILayer, Transform>();
		private static Dictionary<Type, ABContainer> UIPrefabDic = new Dictionary<Type, ABContainer>();

		private static GameObject UIWindows;
		
		public static void Initialize()
		{
			var windowsAB = ABLoader.LoadAB("UI/UI_Windows");
			var windowsPrefab = windowsAB.Bundle.LoadAsset<GameObject>("UI_Windows");
			UIWindows = GameObject.Instantiate<GameObject>(windowsPrefab);
			GameObject.DontDestroyOnLoad(UIWindows);
			
			LayerRoot.Add(UILayer.bottom, UIWindows.transform.Find("Bottom"));
			LayerRoot.Add(UILayer.center, UIWindows.transform.Find("Center"));
			LayerRoot.Add(UILayer.cover, UIWindows.transform.Find("Cover"));
			
		}

		/// <summary>
		/// 【代码产生器使用】 初始注册的调用 
		/// </summary>
		/// <param name="abPath"></param>
		/// <typeparam name="T"></typeparam>
		/// <exception cref="Exception"></exception>
		public static void Register<T>(string abPath) where T : UIObject
		{
			var type = typeof(T);
			if (UIPrefabDic.ContainsKey(type))
			{
				throw new Exception("已存在对应的UI类型 : " + type);
			}
			UIPrefabDic.Add(type, ABLoader.LoadAB(abPath, false));
		}

		public static UI_Item CreateUIItem<UI_Item>(Transform parent) where UI_Item : UIItem
		{
			var type = typeof(UI_Item);
			var container = UIPrefabDic[type];
			ABLoader.LoadAB(container);
			if (container.Bundle == null) throw new NullReferenceException(string.Format("该路径下没有AB包: [{0}]", container.ABName));
			var prefab = container.Bundle.LoadAsset<GameObject>(type.Name);
			if (prefab == null) throw new NullReferenceException(string.Format("AB包中没有预制体：[{0}]", type.Name));
			var itemObj = GameObject.Instantiate<GameObject>(prefab, parent, false);
			var ret = itemObj.AddComponent<UI_Item>();
			UIObjectList.Add(ret);
			
			return ret;
		}
		
		private readonly static List<UIObject> UIObjectList = new List<UIObject>(); 
		public static UI_View CreateUIView<UI_View>(UILayer layer) where UI_View : UIView
		{
			var type = typeof(UI_View);
			var container = UIPrefabDic[type];
			ABLoader.LoadAB(container);
			if (container.Bundle == null) throw new NullReferenceException(string.Format("该路径下没有AB包: [{0}]", container.ABName));
			var prefab = container.Bundle.LoadAsset<GameObject>(type.Name);
			if (prefab == null) throw new NullReferenceException(string.Format("AB包中没有预制体：[{0}]", type.Name));
			var itemObj = GameObject.Instantiate<GameObject>(prefab, LayerRoot[layer], false);
			var ret = itemObj.AddComponent<UI_View>();
			UIObjectList.Add(ret);
			
			return ret;
		}

		public static void CloseUI<UI_Object>(UI_Object ui) where UI_Object : UIObject
		{
			if (ui == null)
				return;
			var type = typeof(UI_Object);
			UIObjectList.Remove(ui as UIView);
			GameObject.Destroy(ui.gameObject); // 此处销毁的时候，会执行UIObject里的OnDestroy

			ABLoader.UnLoadAB(UIPrefabDic[type]);
		}
	}

}