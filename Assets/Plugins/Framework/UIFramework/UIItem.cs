using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIFramework
{
	public class UIItem : UIObject
	{
		protected Dictionary<Transform, List<UIObject>> mItems = new Dictionary<Transform, List<UIObject>>();
		
		protected UI_Item AddSubItemToNode<UI_Item>(Transform parent) where UI_Item : UIItem
		{
			var item = UIManager.CreateUIItem<UI_Item>(parent);
			List<UIObject> list = null;
			if (!mItems.ContainsKey(parent))
			{
				list = new List<UIObject>();
				mItems[parent] = list;
			}
			list.Add(item);
			return item;
		}

		protected void RemoveSubItemsToNode(Transform parent)
		{
			List<UIObject> list = null;
			if (mItems.TryGetValue(parent, out list))
			{
				foreach (var item in list)
				{
					UIManager.CloseUI(item);
				}
				list.Clear();
			}
		}
	}
}
