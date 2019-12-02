using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ABLoad;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UIFramework
{

	public abstract class UIObject : MonoBehaviour
	{

		protected readonly CompositeDisposable Disposables = new CompositeDisposable();

		#region 工具

		protected void BindComponent<T>(Component target, out T component) where T : Component
		{
			if (target == null)
				component = null;
			else
				component = target.GetComponent<T>();
		}

		protected void AddButton(Component target, UnityAction onClick)
		{
			var btn = target.GetComponent<Button>() ?? target.gameObject.AddComponent<Button>();
			btn.onClick.AddListener(onClick);
		}

		#endregion
		
		#region 资源

		private Dictionary<string, ABContainer> ABDic = new Dictionary<string, ABContainer>();
		

		protected GameObject LoadPrefab(string path, string prefabName)
		{
			GameObject ret = null;
			ABContainer container = null;
			if (!ABDic.TryGetValue(path, out container))
			{
				container = ABLoader.LoadAB(path);
				ABDic[path] = container;
			}

			container.Bundle.LoadAsset<GameObject>(name);
			return ret;
		}

		// 具体特效
		private Dictionary<string, Queue<GameObject>> EffectDic = new Dictionary<string, Queue<GameObject>>();
		// 特效容器 - 管播放等的
		private List<EffectContainer> EffectContainers = new List<EffectContainer>();

		/// <summary>
		/// 创建特效，当duration <= 0时，特效常驻
		/// </summary>
		protected void CreateEffectFromPools(string effectPath, string effectName, Transform parent, float duration = 0)
		{

			Queue<GameObject> particles = null;
			GameObject effect = null;
			if (!EffectDic.TryGetValue(effectPath, out particles))
			{
				GameObject prefab = LoadPrefab(effectPath, effectName);
				particles = new Queue<GameObject>();

				effect = Instantiate(prefab, parent, false);

			}
			else
			{
				effect = particles.Dequeue();
				effect.transform.parent = parent;
			}
			
			EffectContainer container = new EffectContainer(effect, particles);
			EffectContainers.Add(container);
			container.Play(duration);
		}

		/// <summary>
		/// 创建特效，当duration <= 0时，特效常驻
		/// </summary>
		protected void CreateSimpleEffect(string effectPath, string effectName, Transform parent, float duration = 0)
		{
			GameObject effect = null;
			GameObject prefab = LoadPrefab(effectPath, effectName);

			effect = Instantiate(prefab, parent, false);

			EffectContainer container = new EffectContainer(effect, null);
			EffectContainers.Add(container);
			container.Play(duration);
		}

		#endregion

		private void OnDestroy()
		{
			// 销毁特效
			EffectContainers.ForEach(obj =>
			{
				if (obj != null) obj.Dispose();
			});
			EffectContainers.Clear();
			foreach (var item in EffectDic)
			{
				if (item.Value != null)
				{
					Queue<GameObject> queue = item.Value;
					while (queue.Count > 0)
					{
						GameObject.Destroy(queue.Dequeue());
					}
				}
			}
			EffectDic.Clear();
			
			// 资源释放
			foreach (var ab in ABDic)
			{
				ABLoader.UnLoadAB(ab.Value);
			}
			ABDic.Clear();
			ABDic = null;
			
			Disposables.Clear();
		}
	}

}