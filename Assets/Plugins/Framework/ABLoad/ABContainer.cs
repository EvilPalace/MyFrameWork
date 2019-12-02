/**
 * ABContainer
 *
 * Created by ZhangHuaGuang on 2019年11月19日
 */


using System;
using System.IO;
using UniRx;
using UnityEngine;

namespace ABLoad
{
    public class ABContainer
    {
        public string ABName { get; private set; }
        public AssetBundle Bundle { get; private set; }
        public CountNotifier ReferenceCount { get; private set; }

        public ABContainer(string abname, bool isLoad)
        {
            this.ABName = Path.Combine(ABLoader.ABPath, abname);
            ReferenceCount = new CountNotifier();
            ReferenceCount.Subscribe(HandleAsset);
            if (isLoad)
                Bundle = AssetBundle.LoadFromFile(ABName);
        }

        private void HandleAsset(CountChangedStatus status)
        {
            if (status == CountChangedStatus.Decrement)
            {
                if (ReferenceCount.Count == 0)
                {
                    Bundle.Unload(true);
                    Bundle = null;
                }
            }
            else if (status == CountChangedStatus.Increment)
            {
                if (Bundle == null)
                {
                    Bundle = AssetBundle.LoadFromFile(ABName);
                }
            }
        }
    }
}